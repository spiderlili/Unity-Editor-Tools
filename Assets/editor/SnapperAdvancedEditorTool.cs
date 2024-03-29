﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using TMPro;

public class SnapperAdvancedEditorTool : EditorWindow
{
    public enum GridType
    {
        Cartesian,
        Polar
    }

    public float gridSize = 1.0f; //meters per grid unit
    public GridType gridType = GridType.Cartesian;
    public int angularDivisions = 24; //unity's default unit for rotation snapping
    const float TAU = 6.28318530718f;

    //store saved data across sessions so the tool remembers the last settings
    const string savedGridSize = "SNAPPER_TOOL_gridSize";
    const string savedGridType = "SNAPPER_TOOL_gridType";
    const string savedAngularDivisions = "SNAPPER_TOOL_angularDivisions";

    [MenuItem("Tools/Advanced Snapper")]
    public static void OpenTool() => GetWindow<SnapperAdvancedEditorTool>("Advanced Snapper");
    SerializedObject so;
    SerializedProperty gridSizeProperty;
    SerializedProperty gridTypeProperty;
    SerializedProperty angularDivisionsProperty;

    //Vector3 point;

    private void OnEnable()
    {
        so = new SerializedObject(this);
        gridSizeProperty = so.FindProperty("gridSize");
        gridTypeProperty = so.FindProperty("gridType");
        angularDivisionsProperty = so.FindProperty("angularDivisions");
        Selection.selectionChanged += Repaint;
        SceneView.duringSceneGui += DuringSceneGUI;

        //load saved configuration data when open the window, use default values if data does not exist
        gridSize = EditorPrefs.GetFloat(savedGridSize, 1f);
        gridType = (GridType) EditorPrefs.GetInt(savedGridType, 0);
        angularDivisions = EditorPrefs.GetInt(savedAngularDivisions, 24); 
    }

    private void OnDisable()
    {
        //save configuration
        EditorPrefs.SetFloat(savedGridSize, gridSize);
        EditorPrefs.SetInt(savedGridType, (int)gridType);
        EditorPrefs.SetInt(savedAngularDivisions, angularDivisions);

        Selection.selectionChanged -= Repaint;
        SceneView.duringSceneGui -= DuringSceneGUI;
    }

    void DuringSceneGUI(SceneView sceneview)
    {
        //enable undo/redo move selection handle
        //so.Update();
        //point = Handles.PositionHandle(point, Quaternion.identity);
        //so.ApplyModifiedProperties();

        Handles.zTest = CompareFunction.LessEqual; //only draw if it's in front of other things but not if it's behind
        const float gridDrawExtent = 16;

        if(gridType == GridType.Cartesian)
        {
            DrawGridCartesian(gridDrawExtent);
        }
        else
        {
            DrawGridPolar(gridDrawExtent);
        }
    }

    //draw ring radial segments around the centre: only need this on 1 side as it wraps around the other side
    void DrawGridPolar(float gridDrawExtent)
    {
        int ringCount = Mathf.RoundToInt((gridDrawExtent) / gridSize);

        float maxOuterRadius = (ringCount - 1) * gridSize; //removes the extra extended line out of edge by -1

        //draw rings radial grid: skip the 1st one as it has 0 radius, change radius per iteration
        for (int i = 0; i < ringCount; i++) 
        {
            Handles.DrawWireDisc(Vector3.zero, Vector3.up, i * gridSize); //set normal to vector3.forward for y plane orientation
        }

        //draw angular grid lines with trigonometry
        for(int i = 0; i < angularDivisions; i++)
        {
            //need to make sure this is a float, don't do angularDivisions/1 to skip drawing the last line in circle twice
            float interpolator = i / ((float)angularDivisions);
            float angleRadians = interpolator * TAU; //percentage to radians

            //use trigonometry to get normalized vector from the centre point pointing towards target point in outer ring
            float x = Mathf.Cos(angleRadians);
            float y = Mathf.Sin(angleRadians);
            Vector3 direction = new Vector3(x, 0f, y); //set to z component to be on the correct plane
            Handles.DrawAAPolyLine(Vector3.zero, direction * maxOuterRadius); //extend radius from unit circle towards the outer ring edge
        }
    }

    void DrawGridCartesian(float gridDrawExtent)
    {
        int lineCount = Mathf.RoundToInt((gridDrawExtent * 2) / gridSize);
        int halfLineCount = lineCount / 2;

        if (lineCount % 2 == 0)
        {
            lineCount++;         //make sure line numbers are odd to have a centre line of symmetry on both sides
        }

        for (int i = 0; i < lineCount; i++)
        {
            int intOffset = i - halfLineCount;
            float xCoord = intOffset * gridSize;
            float zCoord0 = halfLineCount * gridSize;
            float zCoord1 = -halfLineCount * gridSize;
            Vector3 p0 = new Vector3(xCoord, 0f, zCoord0); //vertical lines along Z
            Vector3 p1 = new Vector3(xCoord, 0f, zCoord1);
            Handles.DrawPolyLine(p0, p1);
            p0 = new Vector3(zCoord0, 0f, xCoord); //horizontal lines along X
            p1 = new Vector3(zCoord1, 0f, xCoord);
            Handles.DrawPolyLine(p0, p1);
        }
    }

    private void OnGUI()
    {
        so.Update();
        EditorGUILayout.PropertyField(gridTypeProperty);
        EditorGUILayout.PropertyField(gridSizeProperty, GUILayout.Width(300));

        //for polar grids: draw angular divisions
        if(gridType == GridType.Polar)
        {
            EditorGUILayout.PropertyField(angularDivisionsProperty);

            //prevent angular divisions from being set to a negative value - clamp to at least 4
            if (angularDivisionsProperty.intValue < 4)
            {
                angularDivisionsProperty.intValue = 4;
            }
        }

        so.ApplyModifiedProperties(); //works with auto-undo system

        using(new EditorGUI.DisabledScope(Selection.gameObjects.Length == 0))
        {
            if(GUILayout.Button("Snap Selection", GUILayout.Width(100)))
            {
                SnapSelection();
            }
        }
    }

    void SnapSelection()
    {
        foreach (GameObject selectedObj in Selection.gameObjects)
        {
            Undo.RecordObject(selectedObj.transform, "Snap objects");
            selectedObj.transform.position = GetSnappedPosition(selectedObj.transform.position);
            //selectedObj.transform.position = selectedObj.transform.position.Round(gridSize);
        }
    }

    Vector3 GetSnappedPosition(Vector3 originalPosition) //branch based off which type of selection you have
    {
        if(gridType == GridType.Cartesian)
        {
            return originalPosition.Round(gridSize);
        }

        if(gridType == GridType.Polar)
        {   //polar coordinates are on the xz plane, ignore height - swizzle original position into vector2
            Vector2 positionVector = new Vector2(originalPosition.x, originalPosition.z); 
            float distance = positionVector.magnitude;
            float distanceSnapped = distance.Round(gridSize);

            //calculate theta angle for snapping, snap the angle
            float angleRadians = Mathf.Atan2(positionVector.y, positionVector.x); //0 to TAU
            float angleTurns = angleRadians / TAU; //convert from radians to turns: = 0 - 1
            float angleTurnsSnapped = angleTurns.Round(1f / angularDivisions);
            float angleRadiansSnapped = angleTurnsSnapped * TAU;

            //go back to Cartesian space, reconstruct the 3D position by remapping - don't want to change the height
            Vector2 directionSnapped = new Vector2(Mathf.Cos(angleRadiansSnapped), Mathf.Sin(angleRadiansSnapped));
            Vector2 vectorSnapped = directionSnapped * distanceSnapped;
            return new Vector3(vectorSnapped.x, originalPosition.y, vectorSnapped.y);
        }

        else
        {
            return default;
        }
    }

    //TODO
    //add different type of grid pattern: angled criss cross grid, isometric grid, triangular grid
    //show the grid in the scene view to visualise the grid snapping, ideally around the objects snapped and where the objects will move to
    //add support for a polar grid: instead of only having support for cartesian coordinates, have a polar grid with angular and radial units to snap to - in this case the grid size scale would also need an angular scale: radial size, angular size
    //ability to place grids in the scene and snap objects to those grids, rather than just having one grid in that editor window.have localised grids in various places in the scene view to snap to.
    //make the grid settings persist between unity sessions
    //set color based on how far away it is from the center

}

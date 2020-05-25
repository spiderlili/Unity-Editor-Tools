using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

public class SnapperAdvancedEditorTool : EditorWindow
{
    public float gridSize = 1.0f; //meters per grid unit

    [MenuItem("Tools/Advanced Snapper")]
    public static void OpenTool() => GetWindow<SnapperAdvancedEditorTool>("Advanced Snapper");
    SerializedObject so;
    SerializedProperty gridSizeProperty; 
    private void OnEnable()
    {
        so = new SerializedObject(this);
        gridSizeProperty = so.FindProperty("gridSize"); 
        Selection.selectionChanged += Repaint;
        SceneView.duringSceneGui += DuringSceneGUI;
    }

    private void OnDisable()
    {
        Selection.selectionChanged -= Repaint;
        SceneView.duringSceneGui -= DuringSceneGUI;
    }

    void DuringSceneGUI(SceneView sceneview)
    {
        Handles.zTest = CompareFunction.LessEqual; //only draw if it's in front of other things but not if it's behind
        const float gridDrawExtent = 16;
        int lineCount = Mathf.RoundToInt((gridDrawExtent * 2) / gridSize);
        int halfLineCount = lineCount / 2;

        if(lineCount %2 == 0)
        {
            lineCount++;         //make sure line numbers are odd to have a centre line of symmetry on both sides
        }
        
        for(int i = 0; i < lineCount; i++)
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
        EditorGUILayout.PropertyField(gridSizeProperty, GUILayout.Width(300));
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
            selectedObj.transform.position = selectedObj.transform.position.Round(gridSize);
            //Debug.Log("snapped");
        }
    }

    //TODO
    //add a property to change the granularity of the snapping - set grid size / scale button
    //show the grid in the scene view to visualise the grid snapping, ideally around the objects snapped and where the objects will move to
    //add support for a polar grid: instead of only having support for cartesian coordinates, have a polar grid with angular and radial units to snap to - in this case the grid size scale would also need an angular scale: radial size, angular size
    //ability to place grids in the scene and snap objects to those grids, rather than just having one grid in that editor window.have localised grids in various places in the scene view to snap to.
    //make the grid settings persist between unity sessions
    //set color based on how far away it is from the center

}

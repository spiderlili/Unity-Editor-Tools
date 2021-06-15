﻿using System.Collections;
using UnityEditor;
using UnityEngine;

public class VertexPainter_Window : EditorWindow
{
    #region Variables
    GUIStyle boxStyle;
    public Vector2 mousePos = Vector2.zero;
    public RaycastHit currentHit; //store the obj hit by ray
    #endregion

    #region MainMethod
    public static void LaunchVertexPainter() //init the editor window: normal window, title, focused on launch
    {
        var vertexPainterWindow = EditorWindow.GetWindow<VertexPainter_Window>(false, "Vertex Painter", true);
        vertexPainterWindow.GenerateStyles();
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui -= this.OnSceneGUI;
        SceneView.duringSceneGui += this.OnSceneGUI;
    }
    private void OnDestroy()
    {
        SceneView.duringSceneGui -= this.OnSceneGUI;
    }

    #endregion

    #region GUIMethods
    private void OnGUI()
    {
        //header
        EditorGUILayout.BeginHorizontal();
        //EditorGUILayout.LabelField("Vertex Painter", EditorStyles.boldLabel);
        GUILayout.Box("Vertex Painter", boxStyle, GUILayout.Height(60), GUILayout.ExpandWidth(true));
        EditorGUILayout.EndHorizontal();

        //body: styled using default box GUI skin to create box layout
        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.Space(10);

        //50/50 divide horizontally for 2 buttons
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Button1", GUILayout.Height(30)))
        {

        }

        if (GUILayout.Button("Button2", GUILayout.Height(30)))
        {

        }
        EditorGUILayout.EndHorizontal();

        //100% width for 3rd button
        if (GUILayout.Button("Update Styles", GUILayout.Height(30)))
        {
            GenerateStyles();
        }

        GUILayout.FlexibleSpace(); //make the content after FlexibleSpace() always stay at the bottom - great for footer
        EditorGUILayout.EndVertical();

        //footer
        //EditorGUILayout.LabelField("Title");
        GUILayout.Box("Title", boxStyle, GUILayout.Height(60), GUILayout.ExpandWidth(true));

        //update & repaint the UI in real time
        Repaint();
    }

    // Draw or do input/handle overriding in the scene view
    void OnSceneGUI(SceneView sceneView)
    {
        DrawBrushGUIOnMouseOver();
        ProcessInputs();//Get user inputs
        //Debug.Log(mousePos);
        sceneView.Repaint(); //update & repaint sceneView GUI
    }

    #endregion

    #region UtilityMethods
    private void DrawBrushGUIOnMouseOver()
    {
        //debug 3D UI in the sceneview
        Handles.BeginGUI();
        GUILayout.BeginArea(new Rect(10, 10, 200, 150), boxStyle); // block of GUI controls in a fixed screen area
        GUILayout.Button("Button", GUILayout.Height(25));
        GUILayout.Button("Button", GUILayout.Height(25));
        GUILayout.EndArea();
        Handles.EndGUI();

        if (currentHit.transform != null) //only show the brush GUI if it has hit something
        {
            Handles.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            Handles.DrawSolidDisc(currentHit.point, Vector3.up, 1.0f);
            Handles.color = new Color(1.0f, 1.0f, 1.0f, 1.0f); ;
            Handles.DrawWireDisc(currentHit.point, Vector3.up, 0.5f);
        }

        Ray worldRay = HandleUtility.GUIPointToWorldRay(mousePos); //Uses the current camera to convert 2D GUI position to a world space ray.
        if (Physics.Raycast(worldRay, out currentHit, 500f)) //500f = maxDistance the ray should check for collisions. try float.MaxValue
        {
            //BeginVertexPainting
            Debug.Log(currentHit.transform.name);
        }
        //Handles.Label(Vector3.zero, "Label at World Centre");
    }
    private void ProcessInputs() //controlled via OnSceneGUI()
    {
        // Event.current houses information on scene view input this cycle being processed right now
        Event currentEvt = Event.current;

        mousePos = currentEvt.mousePosition;

        if (currentEvt.type == EventType.KeyDown)
        {
            if (currentEvt.isKey && currentEvt.keyCode == KeyCode.T)
            {

            }
        }

        if (currentEvt.type == EventType.MouseDown)
        {
            if (currentEvt.button == 0)
            {

            }
        }
    }

    private void GenerateStyles()
    {
        boxStyle = new GUIStyle();
        boxStyle.normal.background = (Texture2D)Resources.Load("UIBackgrounds/Bg_BlueWhiteGradient");
        boxStyle.font = (Font)Resources.Load("Fonts/NotoSansCJKkr-Black");
        boxStyle.normal.textColor = Color.white;
        boxStyle.fontSize = 25;
        boxStyle.fontStyle = FontStyle.Bold;
        boxStyle.alignment = TextAnchor.MiddleCenter;
        boxStyle.border = new RectOffset(3, 3, 3, 3); //define in pixels the width of the border in the bg 
        boxStyle.margin = new RectOffset(5, 5, 5, 5);
    }

    #endregion
}

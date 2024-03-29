﻿using System.Collections;
using UnityEditor;
using UnityEngine;

public class VertexPainter_Window : EditorWindow
{
    #region Variables
    GUIStyle boxStyle;
    //TODO: set allowPainting to false on scene change
    public bool allowPainting = false;
    public bool isPaintingWithBrush = false; //only paint vertex color when the left mouse button is held down
    public bool changingBrushValue = false;
    public Vector2 mousePos = Vector2.zero;
    public RaycastHit currentHit; //store the obj hit by ray

    public float brushSize = 1.0f;
    public float fallOffPowerMultiplier = 3.0f;
    private float brushMultiplier = 0.005f;
    private float minBrushSize = 0.1f;
    private float maxBrushSize = 10.0f;
    public float brushOpacity = 1.0f;
    public float brushFalloff = 2.0f;
    public Color foregroundColor = Color.white;

    public GameObject currentGO; //current gameObject on mouse hover
    public Mesh currentMesh; //current mesh being painted
    public GameObject lastGO; //the last gameObject on mouse hover
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
        allowPainting = true;
    }
    private void OnDestroy()
    {
        SceneView.duringSceneGui -= this.OnSceneGUI;
        allowPainting = false;
    }

    private void Update()
    {
        if (allowPainting) //Deselect everything when in painting mode, prevent wireframe view of paintable mesh
        {
            Selection.activeGameObject = null;
        }
        else
        {
            currentGO = null;
            currentMesh = null;
            lastGO = null;
        }

        if (currentHit.transform != null) //if mouse is hovering over a piece of geometry
        {
            if (currentHit.transform.gameObject != lastGO) //don't need to keep running this if you've already got the obj (lastObj hasn't changed)
            {
                currentGO = currentHit.transform.gameObject;
                currentMesh = VertexPainter_Utils.GetMesh(currentGO);
                lastGO = currentGO;

                if (currentGO != null & currentMesh != null)
                {
                    Debug.Log(currentGO.name + ": " + currentMesh.name);
                }
            }
        }
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
        allowPainting = GUILayout.Toggle(allowPainting, "Allow Vertex Painting", GUI.skin.toggle, GUILayout.Height(60));

        EditorGUILayout.EndHorizontal();

        //100% width for 3rd button
        /* if (GUILayout.Button("Update Styles", GUILayout.Height(30)))
        {
            GenerateStyles();
        } */

        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Brush Size: ", GUILayout.ExpandWidth(false));
        brushSize = GUILayout.HorizontalSlider(brushSize, minBrushSize, maxBrushSize);
        GUILayout.Label("" + brushSize, GUILayout.ExpandWidth(false)); //TODO: add the ability to define value by textfield
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Brush Opacity: ", GUILayout.ExpandWidth(false));
        brushOpacity = GUILayout.HorizontalSlider(brushOpacity, 0.0f, 1.0f); //TODO: add the ability to define value by textfield
        GUILayout.Label("" + brushOpacity, GUILayout.ExpandWidth(false));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Brush Falloff: ", GUILayout.ExpandWidth(false));
        brushFalloff = GUILayout.HorizontalSlider(brushFalloff, minBrushSize, brushSize); //TODO: add the ability to define value by textfield
        GUILayout.Label("" + brushFalloff, GUILayout.ExpandWidth(false));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Falloff Power: ", GUILayout.ExpandWidth(false));
        fallOffPowerMultiplier = GUILayout.HorizontalSlider(fallOffPowerMultiplier, 1.0f, 5.0f); //TODO: add the ability to define value by textfield
        GUILayout.Label("" + fallOffPowerMultiplier, GUILayout.ExpandWidth(false));
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);
        foregroundColor = EditorGUILayout.ColorField("Foreground Color: ", foregroundColor);

        //TODO: add vertexcolor.r/g/b/a separate channel controls
        GUILayout.FlexibleSpace(); //make the content after FlexibleSpace() always stay at the bottom - great for footer
        EditorGUILayout.EndVertical();

        EditorGUILayout.LabelField("Vertex Painter Shortcuts");
        EditorGUILayout.HelpBox("Ctrl + Left Mouse Drag to Change brush size, Shift + Left Mouse Drag to change brush opacity, Ctrl + Shift + Left Mouse Drag to change brush falloff",
        MessageType.Info);

        Repaint();//update & repaint the UI in real time
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

    #region TempVertexPainterMethods
    private void PaintVertexColor()
    {
        if (currentMesh != null)
        {
            Vector3[] verts = currentMesh.vertices; //capture all vertices of the current mesh
            Color[] colors = new Color[0];

            if (currentMesh.colors.Length > 0)  //overwrite existing vertex colors of the mesh if it already has vertex colors
            {
                colors = currentMesh.colors;
            }
            else
            {
                colors = new Color[verts.Length]; //init vertex colors if the mesh does not have any
            }

            //determine which vertices are underneath the brush: detect each vertices' distance away from the centre of the brush(currentHit).
            for (int i = 0; i < verts.Length; i++)
            {
                Vector3 vertPos = currentGO.transform.TransformPoint(verts[i]);//transform vertex position from local to world space
                //get the squared magnitude(radius) = distance of the vertex to the centre of the brush
                float sqrMag = (vertPos - currentHit.point).sqrMagnitude; //currentHit.point = brush centre, sqrMag = radius
                if (sqrMag > brushSize)
                {
                    continue;
                }
                float falloff = VertexPainter_Utils.LinearFallOff(sqrMag, brushSize);
                falloff = Mathf.Pow(falloff, brushFalloff * fallOffPowerMultiplier) * brushOpacity;
                //set the vertex color if brush is within vertex's radius to brush centre, lerp between colours' edges
                colors[i] = VertexPainter_Utils.LerpVertexColor(colors[i], foregroundColor, falloff);
            }
            currentMesh.colors = colors;  //reassign the mesh's vertex colors with painted vertex colors
        }
        else
        {
            Debug.LogWarning("Can't paint vertex colors because there is no mesh available!");
        }
    }
    #endregion

    #region UtilityMethods
    private void ShowDebug3DUI()
    {
        //debug 3D UI in the sceneview. TODO: create utility button to show help
        Handles.BeginGUI();
        GUILayout.BeginArea(new Rect(10, 10, 200, 150), boxStyle); // block of GUI controls in a fixed screen area
        GUILayout.Button("Button", GUILayout.Height(25));
        GUILayout.Button("Button", GUILayout.Height(25));
        GUILayout.EndArea();
        Handles.EndGUI();
    }
    private void DrawBrushGUIOnMouseOver()
    {
        //ShowDebug3DUI();

        if (allowPainting)
        {
            if (currentHit.transform != null) //only get the brush GUI to follow the mouse if raycast from mouse has hit something
            {
                Handles.color = new Color(foregroundColor.r, foregroundColor.g, foregroundColor.b, brushOpacity);
                Handles.DrawSolidDisc(currentHit.point, currentHit.normal, brushSize);
                Handles.color = new Color(foregroundColor.r + 0.5f, foregroundColor.g + 0.5f, foregroundColor.b + 0.5f, 1.0f); //opaque brush outline color
                Handles.DrawWireDisc(currentHit.point, currentHit.normal, brushSize);
                Handles.DrawWireDisc(currentHit.point, currentHit.normal, brushFalloff);
            }

            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive)); //turns everything off in passive mode

            Ray worldRay = HandleUtility.GUIPointToWorldRay(mousePos); //Uses the current camera to convert 2D GUI position to a world space ray.
            if (!changingBrushValue)
            {
                if (Physics.Raycast(worldRay, out currentHit, 500f)) //500f = maxDistance the ray should check for collisions. try float.MaxValue
                {
                    if (isPaintingWithBrush) //only paint vertex color when the left mouse is held down
                    {
                        PaintVertexColor();
                    }
                    //Debug.Log(currentHit.transform.name);
                }
            }
        }
        else
        {
            //TODO: Try deleting this else block - may not be necessary to have this statement
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Keyboard)); //turns everything back on when not in painting mode
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
            if (currentEvt.isKey)
            {
                if (currentEvt.keyCode == KeyCode.V) //shortcut to toggle vertex painting
                {
                    allowPainting = !allowPainting;
                    if (!allowPainting)
                    {
                        Tools.current = Tool.View;
                    }
                    else
                    {
                        Tools.current = Tool.None;
                    }
                }
            }
        }

        //brush key combination
        if (allowPainting)
        {
            if (currentEvt.type == EventType.MouseDrag)
            {
                if (currentEvt.control && currentEvt.button == 0 && !currentEvt.shift) //if holding down ctrl & left mouse button &  not shift (contrls opacity)
                {
                    //single float value: relative side-to-side movement of the mouse compared to last event. 0.005f modifies brush change speed
                    brushSize += currentEvt.delta.x * brushMultiplier;
                    brushSize = Mathf.Clamp(brushSize, minBrushSize, maxBrushSize);
                    if (brushFalloff > brushSize) //make sure brush fall off is never bigger than brush size
                    {
                        brushFalloff = brushSize;
                    }
                    changingBrushValue = true;
                }

                if (currentEvt.shift && currentEvt.button == 0 && !currentEvt.control) //shift controls opacity
                {
                    brushOpacity += currentEvt.delta.x * brushMultiplier;
                    brushOpacity = Mathf.Clamp01(brushOpacity);
                    changingBrushValue = true;
                }

                if (currentEvt.control && currentEvt.button == 0 && currentEvt.shift) //shift + ctrl + left mouse controls falloff
                {
                    brushFalloff += currentEvt.delta.x * brushMultiplier;
                    brushFalloff = Mathf.Clamp(brushFalloff, minBrushSize, brushSize);
                    changingBrushValue = true;
                }

                if (currentEvt.button == 0 && !currentEvt.control && !currentEvt.shift && !currentEvt.alt) //don't paint & navigate at once (alt)
                {
                    isPaintingWithBrush = true;
                }
            }
        }

        if (currentEvt.type == EventType.MouseUp)
        {
            changingBrushValue = false;
            isPaintingWithBrush = false; //only paint vertex color when the left mouse is held down
        }
    }

    private void GenerateStyles()
    {
        boxStyle = new GUIStyle();
        //TODO: make bg for box background which is readable 
        //boxStyle.normal.background = (Texture2D)Resources.Load("UIBackgrounds/Bg_BlueWhiteGradient");
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

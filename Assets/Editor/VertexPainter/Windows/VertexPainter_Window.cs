using System.Collections;
using UnityEditor;
using UnityEngine;

public class VertexPainter_Window : EditorWindow
{
    #region Variables
    GUIStyle boxStyle;
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
        //debug 3D UI in the sceneview
        Handles.BeginGUI();
        GUILayout.BeginArea(new Rect(10, 10, 200, 150), boxStyle); // block of GUI controls in a fixed screen area
        GUILayout.Button("Button", GUILayout.Height(25));
        GUILayout.Button("Button", GUILayout.Height(25));
        GUILayout.EndArea();
        Handles.EndGUI();

        Handles.Label(Vector3.zero, "Label at World Centre");
        Handles.color = new Color(1.0f, 0.0f, 1.0f, 1.0f);
        Handles.DrawSolidDisc(Vector3.zero, Vector3.up, 2f);
        Handles.color = new Color(1.0f, 1.0f, 1.0f, 1.0f); ;
        Handles.DrawWireDisc(Vector3.zero, Vector3.up, 2f);
        Handles.DrawSolidDisc(Vector3.zero, Vector3.up, 0.5f);
        // Event.current houses information on scene view input this cycle
        Event current = Event.current;

        // If user has pressed the Left Mouse Button, do something and
        // swallow it so nothing else hears the event
        if (current.type == EventType.MouseDown && current.button == 0)
        {
            // While this tool is open, only allow the user to select scene
            // objects with a Collider component on them
            if (!Select<Collider>(current))
            {
                // If nothing with Collider found, unselect everything
                Selection.activeGameObject = null;
            }
        }

        // After you've done all your custom event interpreting and swallowing,
        // you have to call this code to make sure swallowed events don't bleed out.
        // Not sure why, but that's the rules.
        if (current.type == EventType.Layout)
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(GetHashCode(), FocusType.Passive));
    }

    // When user attempts to select an object, this sees if they selected an object with the given component. 
    // This will swallow the event and select the object if successful.
    /// <param name="e">Event from OnSceneGUI</param>
    /// <typeparam name="T">Component type</typeparam>
    /// <returns>Returns the object</returns>
    public static GameObject Select<T>(Event e) where T : UnityEngine.Component
    {
        Camera cam = Camera.current;

        if (cam != null)
        {
            RaycastHit hit;
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    GameObject gameObj = hit.collider.gameObject;
                    if (gameObj.GetComponent<T>() != null)
                    {
                        e.Use();
                        UnityEditor.Selection.activeGameObject = gameObj;
                        return gameObj;
                    }
                }
            }
        }

        return null;
    }

    #endregion

    #region UtilityMethods
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

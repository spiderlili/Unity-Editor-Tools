using System.Collections;
using UnityEditor;
using UnityEngine;

public class VertexPainter_Window : EditorWindow
{
    #region Variables
    #endregion

    #region MainMethod
    public static void LaunchVertexPainter() //init the editor window: normal window, title, focused on launch
    {
        var vertexPainterWindow = EditorWindow.GetWindow<VertexPainter_Window>(false, "Vertex Painter", true);
    }

    #endregion

    #region GUIMethods
    private void OnGUI()
    {
        //header
        EditorGUILayout.BeginHorizontal();
        //EditorGUILayout.LabelField("Vertex Painter", EditorStyles.boldLabel);
        GUILayout.Box("Vertex Painter", GUILayout.Height(60), GUILayout.ExpandWidth(true));
        EditorGUILayout.EndHorizontal();

        //body: styled using default box GUI skin to create box layout
        EditorGUILayout.BeginVertical(GUI.skin.box);
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
        if (GUILayout.Button("Button3", GUILayout.Height(30)))
        {

        }

        GUILayout.FlexibleSpace(); //make the content after FlexibleSpace() always stay at the bottom - great for footer
        EditorGUILayout.EndVertical();

        //footer
        //EditorGUILayout.LabelField("Title");
        GUILayout.Box("Title", GUILayout.Height(60), GUILayout.ExpandWidth(true));

        //update & repaint the UI in real time
        Repaint();
    }
    #endregion

    #region UtilityMethods
    #endregion
}

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
        EditorGUILayout.LabelField("Vertex Painter", EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();

        //body
        EditorGUILayout.BeginVertical();
        EditorGUILayout.Space(10);

        //50/50 divide horizontally for 2 buttons
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Button1"))
        {

        }

        if (GUILayout.Button("Button2"))
        {

        }
        EditorGUILayout.EndHorizontal();

        //100% width for 3rd button
        if (GUILayout.Button("Button3"))
        {

        }

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndVertical();

        //footer
        EditorGUILayout.LabelField("Title");

        //update & repaint the UI in real time
        Repaint();
    }
    #endregion

    #region UtilityMethods
    #endregion
}

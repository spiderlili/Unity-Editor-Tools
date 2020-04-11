using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Shapes;

public class ShapeDesignerWindow : EditorWindow
{
    private ShapeTypes shapeType;
    private Vector3 shapePosition;
    private string shapeName;

    [MenuItem("Tools/Shape Designer")]
    static void OpenWindow()
    {
        ShapeDesignerWindow window = (ShapeDesignerWindow)GetWindow(typeof(ShapeDesignerWindow));

        //Establish minimum size of window
        window.minSize = new Vector2(200f, 200f);
        window.Show();
    }

    private void OnGUI()
    {
        DrawShapeSettings();
    }

    private void DrawShapeSettings()
    {
        GUILayout.Label("Create Custom Shape"); //header

        EditorGUILayout.BeginHorizontal();
        shapeType = (ShapeTypes)EditorGUILayout.EnumPopup(shapeType);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        shapePosition = EditorGUILayout.Vector3Field("Shape Position", shapePosition);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        shapeName = EditorGUILayout.TextField("Shape Name", shapeName);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("Create Shape")) //create button
        {
            Debug.Log("Shape created");
        }
        EditorGUILayout.EndHorizontal();
    }
}

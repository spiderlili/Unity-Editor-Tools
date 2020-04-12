using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Shapes;

public class ShapeDesignerWindow : EditorWindow
{
    private ShapeTypes shapeType;
    private Vector3 shapePosition;
    private Vector3 shapeRotation;
    private float shapeScaleUniform;
    private Vector3 shapeScaleNonUniform;
    private string shapeName;
    private bool isScaleNonUniform = false;
    private bool isSpawnAtRandomPosition = false;
    private bool isSpawnAtRandomScale = false;
    private bool isSpawnAtRandomRotation = false;
    private int numberOfObjectsToSpawn;

    [MenuItem("Tools/Shape Designer")]
    static void OpenWindow()
    {
        ShapeDesignerWindow window = (ShapeDesignerWindow)GetWindow(typeof(ShapeDesignerWindow));

        //Establish minimum size of window
        window.minSize = new Vector2(200f, 300f);
        window.Show();
    }

    private void OnGUI()
    {
        DrawShapeSettings();
    }

    private void DrawShapeSettings()
    {
        GUILayout.Label("Spawn Unity Primitive Shape"); //header

        EditorGUILayout.BeginHorizontal();
        shapeType = (ShapeTypes)EditorGUILayout.EnumPopup(shapeType);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        shapePosition = EditorGUILayout.Vector3Field("Shape Position", shapePosition);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        shapeRotation = EditorGUILayout.Vector3Field("Shape Rotation", shapeRotation);
        EditorGUILayout.EndHorizontal();

        isScaleNonUniform = EditorGUILayout.Toggle("Non Uniform Scale", isScaleNonUniform);

        EditorGUILayout.BeginHorizontal();
        if (isScaleNonUniform == false)
        {
            GUILayout.Label("Shape Scale");
            shapeScaleUniform = EditorGUILayout.Slider(shapeScaleUniform, 1, 10);
        }
        else
        {
            shapeScaleNonUniform = EditorGUILayout.Vector3Field("Shape Scale", shapeScaleNonUniform);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        shapeName = EditorGUILayout.TextField("Object Name", shapeName);
        EditorGUILayout.EndHorizontal();

        //TODO: Increase spacing
        isSpawnAtRandomPosition = EditorGUILayout.Toggle("Spawn Objects at Random Position", isSpawnAtRandomPosition);
        if (isSpawnAtRandomPosition == false)
        {
            //TODO
        }
        else
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Number of Objects To Spawn");
            numberOfObjectsToSpawn = EditorGUILayout.IntSlider(numberOfObjectsToSpawn, 1, 100);
            EditorGUILayout.EndHorizontal();
            isSpawnAtRandomRotation = EditorGUILayout.Toggle("Spawn Objects at Random Rotation", isSpawnAtRandomRotation);
            isSpawnAtRandomScale = EditorGUILayout.Toggle("Spawn Objects at Random Scale", isSpawnAtRandomScale);
        }

        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("Spawn Shape")) //create button
        {
            CreateShape();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void CreateShape()
    {
        switch(shapeType)
        {
            case ShapeTypes.Cube:
                {
                    GameObject shape = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    SetTransform(shape);
                    break;
                }
            case ShapeTypes.Sphere:
                {
                    GameObject shape = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    SetTransform(shape);
                    break;
                }
            case ShapeTypes.Cylinder:
                {
                    GameObject shape = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    SetTransform(shape);
                    break;
                }
            case ShapeTypes.Capsule:
                {
                    GameObject shape = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    SetTransform(shape);
                    break;
                }
            case ShapeTypes.Quad:
                {
                    GameObject shape = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    SetTransform(shape);
                    break;
                }
            case ShapeTypes.Plane:
                {
                    GameObject shape = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    SetTransform(shape);
                    break;
                }
        }
    }

    private void SetTransform(GameObject NewShape)
    {
        NewShape.transform.position = shapePosition;
        NewShape.transform.eulerAngles = shapeRotation;
        if (isScaleNonUniform == false)
        {
            NewShape.transform.localScale = new Vector3(shapeScaleUniform, shapeScaleUniform, shapeScaleUniform);
        }
        else 
        {
            NewShape.transform.localScale = shapeScaleNonUniform;        
        }
        NewShape.name = shapeName;
    }
}

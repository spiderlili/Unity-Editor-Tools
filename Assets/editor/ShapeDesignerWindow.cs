using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Shapes;

public class ShapeDesignerWindow : EditorWindow
{
    private Rect shapeIconSection;
    private Texture2D shapeIconTexture;
    private Texture2D currentShape;
    private Texture2D cubeIcon;
    private Texture2D sphereIcon;
    private Texture2D capsuleIcon;
    private Texture2D cylinderIcon;
    private Texture2D quadIcon;
    private Texture2D planeIcon;

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

    private void OnEnable() //called everytime the window is opened
    {
        InitTextures();
    }

    private void InitTextures()
    {
        shapeIconTexture = new Texture2D(1, 1); //a blank canvas of a white square to be filled
        shapeIconTexture.SetPixel(0, 0, Color.black);
        shapeIconTexture.Apply();

        cubeIcon = Resources.Load<Texture2D>("icons/icon_cube"); //shortcut to resources folder
        sphereIcon = Resources.Load<Texture2D>("icons/icon_sphere"); //shortcut to resources folder
        capsuleIcon = Resources.Load<Texture2D>("icons/icon_capsule"); //shortcut to resources folder
        cylinderIcon = Resources.Load<Texture2D>("icons/icon_cylinder"); //shortcut to resources folder
        quadIcon = Resources.Load<Texture2D>("icons/icon_quad"); //shortcut to resources folder
        planeIcon = Resources.Load<Texture2D>("icons/icon_plane"); //shortcut to resources folder
    }

    private void OnGUI() //function order matters here
    {
        ChangeShape();
        DrawLayouts();
        DrawShapeSettings();
    }

    private void ChangeShape()
    {
        switch (shapeType)
        {
            case ShapeTypes.Cube:
                {
                    currentShape = cubeIcon;
                    break;
                }
            case ShapeTypes.Sphere:
                {
                    currentShape = sphereIcon;
                    break;
                }
            case ShapeTypes.Cylinder:
                {
                    currentShape = cylinderIcon;
                    break;
                }
            case ShapeTypes.Capsule:
                {
                    currentShape = capsuleIcon;
                    break;
                }
            case ShapeTypes.Quad:
                {
                    currentShape = quadIcon;
                    break;
                }
            case ShapeTypes.Plane:
                {
                    currentShape = planeIcon;
                    break;
                }
        }
    }

    private void DrawLayouts() //coordinate where the icons will be
    {
        shapeIconSection.x = Screen.width / 4f;
        shapeIconSection.y = Screen.height / 2.2f;
        shapeIconSection.width = 256;
        shapeIconSection.height = 256;
        GUI.DrawTexture(shapeIconSection, currentShape);

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

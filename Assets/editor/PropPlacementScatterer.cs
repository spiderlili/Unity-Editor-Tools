using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PropPlacementScatterer : EditorWindow
{
    [MenuItem("Tools/Prop Placement Scatterer")]
    public static void OpenPropPlacermentTool() => GetWindow<PropPlacementScatterer>();

    public float radius = 2f;
    public int spawnCount = 8;

    //boilerplate for undo/redo system
    SerializedProperty propRadius;
    SerializedProperty propSpawnCount;
    SerializedObject serializedObject;

    //generate random points in disc
    Vector2[] randomPoints; //just want 2D coordinates within the disk's coordinate system

    private void OnEnable()
    {
        serializedObject = new SerializedObject(this);
        propRadius = serializedObject.FindProperty("radius");
        propSpawnCount = serializedObject.FindProperty("spawnCount");

        //sign up to an event called in every scene's onGUI event when the window is opened
        SceneView.duringSceneGui += DuringSceneGUI;

    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= DuringSceneGUI; //unsubscribe from event
        GenerateRandomPoints();
    }

    private void DrawSphere(Vector3 pos) //requries a world space position but
    {
        Handles.SphereHandleCap(-1, pos, Quaternion.identity, 0.1f, EventType.Repaint); //1 repaint event is sent every frame
    }

    private void DuringSceneGUI(SceneView sceneView) //gui for sceneview window: called per scene view you have open: can have multiple scenes open
    {
        //scene view raycasting: assumes camera centre = where to place things, cache camera: place objects to where the camera is pointing
        Transform cameraTransform = sceneView.camera.transform;
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward); //help visualise where to scatter all the items
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            //set up a full tangent space coordinate system for the point you hit to get full orientation
            //use cross product between up vector & normal rather than forward for predicatable rotation as it's aligned with the camera
            Vector3 hitNormalVectorZ = hit.normal;
            Vector3 hitTangentVectorY = Vector3.Cross(hitNormalVectorZ, cameraTransform.up).normalized; //not normalised unless both inputs are normalised & orthogonal
            Vector3 hitBitangentVectorX = Vector3.Cross(hitNormalVectorZ, hitTangentVectorY);

            foreach (Vector2 pt in randomPoints) //needs to be transformed into a world space position for DrawSphere() as it's in its own tangent space coordinate system
            {
                //Vector3 ptWorldPos = hit.point;
                Vector3 ptWorldPos = hit.point + (hitTangentVectorY * pt.x + hitBitangentVectorX * pt.y) * radius; //scale the position to the radius
                DrawSphere(ptWorldPos); //disc is around the blue vector on the xy plane
            }

            //mark the area hit: draw normal, tangent, bitangent according to their colour convention
            Handles.color = Color.red;
            Handles.DrawAAPolyLine(5, hit.point, hit.point + hitTangentVectorY);
            Handles.color = Color.blue;
            Handles.DrawAAPolyLine(5, hit.point, hit.point + hitNormalVectorZ);
            Handles.color = Color.green;
            Handles.DrawAAPolyLine(5, hit.point, hit.point + hitBitangentVectorX);

            //visualise the radius to scatter objects in
            Handles.color = Color.white;
            Handles.DrawWireDisc(hit.point, hit.normal, radius);
        }
        Handles.DrawAAPolyLine(Vector3.zero, Vector3.one);
    }

    void OnGUI() //gui loop for editor window
    {
        serializedObject.Update(); //make serialized property update when parameters changed
        EditorGUILayout.PropertyField(propRadius);

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(propSpawnCount);
        if (EditorGUI.EndChangeCheck())
        {
            GenerateRandomPoints(); //update all points every time you change spawncount
        }
        //serializedObject.ApplyModifiedProperties();
        if (serializedObject.ApplyModifiedProperties()) //force repaint of the sceneview to make framerate smoother
        {
            SceneView.RepaintAll();
        }
    }

    void GenerateRandomPoints()
    {
        randomPoints = new Vector2[spawnCount];
        for (int i = 0; i < spawnCount; i++)
        {
            randomPoints[i] = Random.insideUnitCircle; //random points of spawnCount inside a unit circle
        }
    }
}
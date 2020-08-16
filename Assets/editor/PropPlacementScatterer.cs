using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PropPlacementScatterer : EditorWindow
{
    [MenuItem ("Tools/Prop Placement Scatterer")]
    public static void OpenPropPlacermentTool () => GetWindow<PropPlacementScatterer> ();

    public float radius = 2f;
    public int spawnCount = 8;

    //boilerplate for undo/redo system
    SerializedProperty propRadius;
    SerializedProperty propSpawnCount;
    SerializedObject serializedObject;

    //generate random points in disc
    Vector2[] randomPoints; //just want 2D coordinates within the disk's coordinate system

    private void OnEnable ()
    {
        serializedObject = new SerializedObject (this);
        propRadius = serializedObject.FindProperty ("radius");
        propSpawnCount = serializedObject.FindProperty ("spawnCount");

        //sign up to an event called in every scene's onGUI event when the window is opened
        SceneView.duringSceneGui += DuringSceneGUI;

    }

    private void OnDisable ()
    {
        SceneView.duringSceneGui -= DuringSceneGUI; //unsubscribe from event
        GenerateRandomPoints ();
    }

    private void DuringSceneGUI (SceneView sceneView) //gui for sceneview window: called per scene view you have open: can have multiple scenes open
    {
        //scene view raycasting: assumes camera centre = where to place things, cache camera: place objects to where the camera is pointing
        Transform cameraTransform = sceneView.camera.transform;
        Ray ray = new Ray (cameraTransform.position, cameraTransform.forward); //help visualise where to scatter all the items
        if (Physics.Raycast (ray, out RaycastHit hit))
        {
            //mark the area hit
            Handles.color = Color.green;
            Handles.DrawAAPolyLine (5, hit.point, hit.point + hit.normal);

            //visualise the radius to scatter objects in
            Handles.DrawWireDisc (hit.point, hit.normal, radius);
        }
        Handles.DrawAAPolyLine (Vector3.zero, Vector3.one);
    }

    void OnGUI () //gui loop for editor window
    {
        serializedObject.Update (); //make serialized property update when parameters changed
        EditorGUILayout.PropertyField (propRadius);
        EditorGUILayout.PropertyField (propSpawnCount);
        //serializedObject.ApplyModifiedProperties();
        if (serializedObject.ApplyModifiedProperties ()) //force repaint of the sceneview to make framerate smoother
        {
            SceneView.RepaintAll ();
        }
    }

    void GenerateRandomPoints ()
    {
        randomPoints = new Vector2[spawnCount];
        for (int i = 0; i < spawnCount; i++)
        {
            randomPoints[i] = Random.insideUnitCircle; //random points of spawnCount inside a unit circle
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PropPlacementScatterer : EditorWindow
{
    [MenuItem("Tools/Prop Placement Scatterer")]
    public static void OpenPropPlacermentTool() => GetWindow<PropPlacementScatterer>();

    public float radius = 2f;
    public int spawnCount = 8;

    private void OnEnable() 
    {
        //sign up to an event called in every scene's onGUI event when the window is opened
        SceneView.duringSceneGui += DuringSceneGUI;
        
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= DuringSceneGUI; //unsubscribe from event
    }

    private void DuringSceneGUI(SceneView sceneView) //called per scene view you have open: can have multiple scenes open
    {
        //scene view raycasting: assumes camera centre = where to place things, cache camera: place objects to where the camera is pointing
        Transform cameraTransform = sceneView.camera.transform;
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward); //help visualise where to scatter all the items
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            //mark the area hit
            Handles.color = Color.green;
            Handles.DrawAAPolyLine(5, hit.point, hit.point+ hit.normal);

            //visualise the radius to scatter objects in
            Handles.DrawWireDisc(hit.point, hit.normal, radius);
        }
        Handles.DrawAAPolyLine(Vector3.zero, Vector3.one);
    }
}

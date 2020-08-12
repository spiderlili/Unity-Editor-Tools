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

    private void DuringSceneGUI(SceneView) //called per scene view you have open: can have multiple scenes open
    {
        //scene view raycasting: assumes camera centre = where to place things
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= DuringSceneGUI; //unsubscribe from event
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace LevelCreator 
{ 
    [CustomEditor(typeof(Level))] //overwrite the inspector of all the Level class instances
    public class LevelInspector : Editor
    {
        private Level _TargetLevel;
        [Header("Visualization")]
        public int varVizTest;

        //save the new level size values to previsualize the changes
        private int _newTotalColumns; 
        private int _newTotalRows;

        //called every time the inspected object is selected & after script is loaded: all the init code goes here
        private void OnEnable() 
        {
            //target has a reference to the object inspected: used to access properties of that, manipulate them in custom inspector
            _TargetLevel = (Level)target; //use targets to support multi-object editing (returns object array)
            InitLevel();
            ResetResizeValues();
        }

        private void ResetResizeValues()
        {
            _newTotalColumns = _TargetLevel.TotalColumns;
            _newTotalRows = _TargetLevel.TotalRows;
        }

        private void InitLevel()
        {
            if(_TargetLevel.Pieces == null || _TargetLevel.Pieces.Length == 0)
            {
                Debug.Log("Initializing the Pieces array...");
                //_TargetLevel.Pieces = new LevelPiece[];
            }
        }

        //called when the inspected object goes out of scope & when the object is destroyed, all the cleanup code goes here
        private void OnDisable() 
        {
        
        }

        //called when the inspected object will be destroyed.
        private void OnDestroy()
        {
        
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.LabelField("The GUI of this inspector was modified.");
            //DrawDefaultInspector();
            DrawLevelDataGUI();
            DrawLevelSizeGUI();
        }

        private void DrawLevelSizeGUI() //modify OnInspectorGUI: use buttons to trigger actions
        {
            EditorGUILayout.LabelField("Size", EditorStyles.boldLabel);
        }

        private void DrawLevelDataGUI()
        {
            EditorGUILayout.LabelField("Data", EditorStyles.boldLabel);

            //create vertical box layout
            EditorGUILayout.BeginVertical("box"); //use GUIStyles in components
            EditorGUILayout.BeginVertical();
            //Ensure the allowSceneObjects parameter is false if the object reference is stored as part of an asset
            //since assets can't store references to objects in a Scene.
            _TargetLevel.BGM = (AudioClip)EditorGUILayout.ObjectField("BGM", _TargetLevel.BGM, typeof(AudioClip), false);
            _TargetLevel.Background = (Sprite)EditorGUILayout.ObjectField("Background", _TargetLevel.Background, typeof(Sprite), false);
            _TargetLevel.Gravity = EditorGUILayout.FloatField("Gravity", _TargetLevel.Gravity);
            _TargetLevel.TotalTime = EditorGUILayout.IntField("Total Time", Mathf.Max(0, _TargetLevel.TotalTime));
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            bool GUIEnabled = GUI.enabled;
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }
    }
}

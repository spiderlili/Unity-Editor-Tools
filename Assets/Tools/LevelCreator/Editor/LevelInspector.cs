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

        //called every time the inspected object is selected & after script is loaded: all the init code goes here
        private void OnEnable() 
        {
            //target has a reference to the object inspected: used to access properties of that, manipulate them in custom inspector
            _TargetLevel = (Level)target; //use targets to support multi-object editing (returns object array)
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
        }

        private void DrawLevelDataGUI()
        {
            EditorGUILayout.LabelField("Data", EditorStyles.boldLabel);
            _TargetLevel.Background = (Sprite)EditorGUILayout.ObjectField("Background", _TargetLevel.Background, typeof(Sprite), false);
        }
    }
}

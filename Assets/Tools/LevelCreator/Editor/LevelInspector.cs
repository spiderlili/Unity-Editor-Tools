using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace LevelCreator
{
    [CustomEditor(typeof(Level))] //overwrite the inspector of all the Level class instances
    // Responsible for the initialization of the Pieces array in an editor context
    public class LevelInspector : Editor
    {
        private Level _TargetLevel;
        [Header("Visualization")]
        public int varVizTest;

        //save the new level size values to previsualize the changes
        private int _newTotalColumns;
        private int _newTotalRows;

        private SerializedObject _serializedObj;
        private SerializedProperty _serializedTotalTime;

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
            _serializedObj = new SerializedObject(_TargetLevel);
            _serializedTotalTime = serializedObject.FindProperty("_totalTime");

            if (_TargetLevel.Pieces == null || _TargetLevel.Pieces.Length == 0)
            {
                Debug.Log("Initializing the Pieces array...");
                //_TargetLevel.Pieces = new LevelPiece[_TargetLevel.TotalColumns * _TargetLevel.TotalRows];
            }
        }

        //change the length of Pieces array 
        //remove all the LevelPiece instances out of level bounds, destroy the prefab associated to the instances

        private void ResizeLevel()
        {
         /*   
            Debug.Log("Level Resized");
            LevelPiece[] newPieces = new LevelPiece[_newTotalColumns * _newTotalRows];
            for(int col = 0; col < _TargetLevel.TotalColumns; col++)
            {
                for(int row = 0; row < _TargetLevel.TotalRows; row++)
                {
                    if(col < _newTotalColumns && row < _newTotalRows)
                    {
                        newPieces[col + row * _newTotalColumns] = _TargetLevel.Pieces[col + row * _TargetLevel.TotalColumns];
                    }
                    else
                    {
                        LevelPiece piece = _TargetLevel.Pieces[col + row * _TargetLevel.TotalColumns];
                        if (newPieces != null)
                        {
                            //must use DestroyImmediate in a Editor context
                            UnityEngine.Object.DestroyImmediate(newPieces.gameObject);
                        }
                    }
                }
            }
            _TargetLevel.Pieces = newPieces;
            _TargetLevel.TotalColumns = _newTotalColumns;
            _TargetLevel.TotalRows = _newTotalRows;
            */
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
            //base.OnInspectorGUI();
            EditorGUILayout.LabelField("The GUI of this inspector was modified.");
            //DrawDefaultInspector(); //display the default GUI on the inspector - good for debugging
            DrawLevelDataGUI();
            DrawLevelSizeGUI();

            if (GUI.changed) //true if there is any change to the inspector GUI
            {
                EditorUtility.SetDirty(_TargetLevel); //marks the target object as dirty and force Level class to redraw
            }
        }

        private void DrawLevelSizeGUI() //modify OnInspectorGUI: use buttons to trigger actions
        {
            EditorGUILayout.LabelField("Size", EditorStyles.boldLabel);
            _newTotalColumns = EditorGUILayout.IntField("Columns", Mathf.Max(1, _newTotalColumns));
            _newTotalRows = EditorGUILayout.IntField("Rows", Mathf.Max(1, _newTotalRows));

            //disable / enable GUI
            bool oldGUIEnabled = GUI.enabled;
            GUI.enabled = (_newTotalColumns != _TargetLevel.TotalColumns || _newTotalRows != _TargetLevel.TotalRows);

            //resize button will display a popup if clicked on
            bool buttonResize = GUILayout.Button("Resize", GUILayout.Height(2 * EditorGUIUtility.singleLineHeight));
            if (buttonResize)
            {
                if (
                    EditorUtility.DisplayDialog
                    (
                    "Level Creator",
                    "Are you sure you want to resize the level? \n This action cannot be undone.",
                    "Yes",
                    "No"
                    )
                   )
                {
                    ResizeLevel(); //if yes is clicked
                }
            }

            //restores the variables: _newTotalColumns and _newTotalRows to match the TotalColumns and TotalRows values
            bool buttonReset = GUILayout.Button("Reset");
            if (buttonReset)
            {
                ResetResizeValues();
            }

            //it makes no sense to press Resize / Reset if the values for the columns / rows don't differ: disable these buttons
            GUI.enabled = oldGUIEnabled; //all the interactive GUI components like buttons will be disabled if GUI.enabled = false
        }

        private void DrawLevelDataGUI()
        {
            EditorGUILayout.LabelField("Data", EditorStyles.boldLabel);

            //create vertical box layout
            EditorGUILayout.BeginVertical("box"); //use GUIStyles in components
            EditorGUILayout.BeginVertical();
            //Ensure the allowSceneObjects parameter is false if the object reference is stored as part of an asset
            //since assets can't store references to objects in a Scene.
            _TargetLevel.Bgm = (AudioClip)EditorGUILayout.ObjectField("BGM", _TargetLevel.Bgm, typeof(AudioClip), false);
            _TargetLevel.Background = (Sprite)EditorGUILayout.ObjectField("Background", _TargetLevel.Background, typeof(Sprite), false);
            _TargetLevel.Gravity = EditorGUILayout.FloatField("Gravity", _TargetLevel.Gravity);

            // Validation: do not allow negative value
            _TargetLevel.TotalTime = EditorGUILayout.IntField("Total Time", Mathf.Max(0, _TargetLevel.TotalTime));
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            bool GUIEnabled = GUI.enabled;
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }
    }
}

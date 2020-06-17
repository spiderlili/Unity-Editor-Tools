using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace LevelCreator
{
    public class EditorUtilsSceneAutomation 
    {
        //create a new scene
        public static void NewScene()
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
        }

        //remove all the elements of the scene
        public static void CleanScene()
        {
            GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();
            foreach(GameObject go in allObjects)
            {
                GameObject.DestroyImmediate(go);
            }
        }

        //create a new scene capable to be used as a level
        public static void NewLevel()
        {
            NewScene();
            CleanScene();
            GameObject levelG0 = new GameObject("Level");
            levelG0.transform.position = Vector3.zero;
            //create game object Level containing the Level script component
            //levelG0.AddComponent<Level>();

        }
    }
}


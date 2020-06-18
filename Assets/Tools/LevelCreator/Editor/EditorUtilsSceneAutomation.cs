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

        //use generics to maximize code reuse: receives a generic type and a path
        //search all prefabs relative to the path (string) that has a script corresponding to the generic type attached
        public static List<T> GetAssetsWithScript<T>(string path) where T : MonoBehaviour
        {
            T temp;
            string assetPath;
            GameObject asset;

            List<T> assetList = new List<T>(); //list with all the script instances
            //search the asset database using a search filter string, returns a list of GUIDs. 
            string[] guids = AssetDatabase.FindAssets("t:prefab", new string[] { path }); //focus the search to 1 path (can do array of paths)
            for (int i = 0; i < guids.Length; i++)
            {
                assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;
                temp = asset.GetComponent<T>(); //check whether the game object has the script attached
                if(temp != null)
                {
                    assetList.Add(temp); //if yes: add this to the list and returned by the method
                }
            }
            return assetList;
        }

        //easily list enums for further use: receive an enum as a generic type, returns a list with all the enum values in it
        public static List<T> GetListFromEnum<T>()
        {
            List<T> enumList = new List<T>();
            System.Array enums = System.Enum.GetValues(typeof(T));
            foreach(T e in enums)
            {
                enumList.Add(e);
            }
            return enumList;
        }
    }
}


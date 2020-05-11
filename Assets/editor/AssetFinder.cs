using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

//TODO: Search for more components
public class AssetFinder : EditorWindow
{
    [MenuItem("Tools/Asset Finder/Search Project For Prefabs with Particle Systems")]

    public static void FindAllPrefabsWithParticleSystems()
    {
        AssetFinder window = (AssetFinder)EditorWindow.GetWindow(typeof(AssetFinder));
        window.Show();
        window.position = new Rect(20, 80, 550, 500);
    }

    List<string> listResult;
    List<string> filteredListResult;
    string componentName = "ParticleSystem";
    Vector2 scroll;

    void OnGUI()
    {
        GUILayout.Label("Search for Prefabs with particle systems");
        GUILayout.Space(3);
        Rect windowRect = GUILayoutUtility.GetRect(1, 17);
        windowRect.x += 4;
        windowRect.width -= 7;


        if (GUILayout.Button("Search for Prefabs with " + componentName))
        {
            string[] allPrefabs = EditorCommonUtilities.GetAllPrefabs();
            listResult = new List<string>();
            foreach (string prefab in allPrefabs)
            {
                UnityEngine.Object obj = AssetDatabase.LoadMainAssetAtPath(prefab);
                GameObject go;
                try
                {
                    go = (GameObject)obj;
                    Component[] components = go.GetComponentsInChildren<ParticleSystem>(true);
                    foreach (ParticleSystem c in components)
                    {
                        if (c != null)
                        {
                            listResult.Add(prefab);
                        }
                    }
                }
                catch
                {
                    Debug.Log("For some reason, prefab " + prefab + " won't cast to GameObject");
                }
            }
        }

        //
        if (listResult != null)
        {
            filteredListResult = listResult.Distinct().ToList();
            //Debug.Log("Filtered list is down from "+listResult +" to " + filteredListResult.Count);
            filteredListResult.Sort();

            if (filteredListResult.Count == 0)
            {               
                GUILayout.Label("No prefabs were found with the component type: " + componentName);
            }
            else
            {
                GUILayout.Label("The following " + filteredListResult.Count + " prefabs use the component type " + componentName);
                
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Select All", GUILayout.Width(position.width / 4)))
                {
                    //TODO
                }
                if (GUILayout.Button("Remove " + componentName + " from All", GUILayout.Width(position.width / 3)))
                {
                    //TODO
                }
                GUILayout.EndHorizontal();

                //Show results in a scroll view
                scroll = GUILayout.BeginScrollView(scroll);
                foreach (string str in filteredListResult)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(str, GUILayout.Width(position.width / 2));
                    
                    //Add a select button for each prefab
                    if (GUILayout.Button("Select", GUILayout.Width(position.width / 4)))
                    {
                        Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(str);
                    }

                    //Add a remove button for each prefab
                    if (GUILayout.Button("Remove " + componentName, GUILayout.Width(150)))
                    {
                        Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(str);
                        GameObject activeGameObj = (GameObject)Selection.activeObject;
                        DestroyImmediate(activeGameObj.GetComponent<ParticleSystem>(), true);
                    }

                    GUILayout.EndHorizontal();
                }
                
                GUILayout.EndScrollView();
            }
        }

    }
}
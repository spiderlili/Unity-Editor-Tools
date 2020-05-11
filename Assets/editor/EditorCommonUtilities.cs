using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public static class EditorCommonUtilities
{
    public static string[] GetAllPrefabs()
    {
        string[] temp = AssetDatabase.GetAllAssetPaths();
        List<string> result = new List<string>();
        foreach (string s in temp)
        {
            if (s.Contains(".prefab")) result.Add(s);
        }
        return result.ToArray();
    }

    public static string GetScenePath(string sceneName, HashSet<string> scenes)
    {
        if (sceneName != null)
        {
            foreach (string scenePath in scenes)
            {
                if (scenePath.Contains(sceneName))
                {
                    return scenePath;
                }
            }
        }
        return sceneName;
    }

    public static string[] GetFullName(Transform go, float? trimSize = null)
    {
        string[] nameReturn = new string[2];
        string name = go.name;

        while (go.transform.parent != null)
        {
            go = go.parent;
            name = string.Concat(go.name, "/", name);
        }

        nameReturn[1] = name;
        nameReturn[0] = name;

        if (trimSize != null && name.Length > trimSize)
        {
            nameReturn[0] = string.Concat(".../", name.Substring(name.Length - (int)trimSize));
        }

        return nameReturn;
    }

    public static bool FindTransformInScene(string original, string name)
    {
        if (name.Length < original.Length)
        {
            return false;
        }
        if (original.Equals(name))
        {
            return true;
        }
        else
        {
            int lastIndex = name.LastIndexOf("/");
            if (lastIndex == -1)
            {
                return false;
            }
            FindTransformInScene(original, name.Substring(0, lastIndex));
        }
        return false;
    }
}
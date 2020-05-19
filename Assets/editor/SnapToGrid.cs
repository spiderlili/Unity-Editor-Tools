using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class SnapToGrid 
{
    [MenuItem("Edit/Snap Selected Object To Grid")]
    public static void SnapThings()
    {
        foreach(GameObject selectedObj in Selection.gameObjects)
        {
            selectedObj.transform.position = selectedObj.transform.position.Round();
            //Debug.Log("snapped");
        }
    }

    public static Vector3 Round(this Vector3 v)
    {
        v.x = Mathf.Round(v.x);
        v.y = Mathf.Round(v.y);
        v.z = Mathf.Round(v.z);
        return v;
    }
}

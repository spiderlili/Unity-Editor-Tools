using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class SnapToGrid 
{
    const string UNDO_STR_SNAP = "Snap Objects";

    [MenuItem("Edit/Snap Selected Object To Grid %&S", isValidateFunction: true)] //disabled if nothing is selected
    public static bool CanSnapThingsValidate()
    {
        return Selection.gameObjects.Length > 0;
    }

    [MenuItem("Edit/Snap Selected Object To Grid %&S")]
    public static void SnapThings()
    {
        foreach(GameObject selectedObj in Selection.gameObjects)
        {
            selectedObj.transform.position = selectedObj.transform.position.RoundToInt();
            Undo.RecordObject(selectedObj.transform, UNDO_STR_SNAP);
            //Debug.Log("snapped");
        }
    }

    public static Vector3 RoundToInt(this Vector3 v)
    {
        v.x = Mathf.Round(v.x);
        v.y = Mathf.Round(v.y);
        v.z = Mathf.Round(v.z);
        return v;
    }

    //TODO: Display message if already snapped
}

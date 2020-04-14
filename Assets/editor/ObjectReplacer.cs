using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectReplacer : ScriptableWizard
{
    public GameObject replacementPrefab;
    // priority to separate from other utilities under Tools
    [MenuItem("Tools/Replace Selected Objects", false, 1000)]
    static void CreateWizard()
    {
        //type of wizard = ObjectReplacer class, when Ynity instantiates a new ScriptableWizard it should be of this type
        ScriptableWizard.DisplayWizard("Object Replacer", typeof(ObjectReplacer), "Replace and close");
    }

    private void OnWizardCreate()
    {
        ReplaceAll();
    }

    private void ReplaceAll()
    {
        //iterate all objects selected. prevent selection from project / assets
        Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.ExcludePrefab);
        foreach(Transform t in transforms)
        {
            ReplaceObject(t);
        }
    }

    private void ReplaceObject(Transform transform)
    {
        GameObject newCopy;
        newCopy = PrefabUtility.InstantiatePrefab(replacementPrefab) as GameObject;
        newCopy.transform.position = transform.position;
        newCopy.transform.rotation = transform.rotation;
        newCopy.transform.localScale = transform.localScale;
        newCopy.transform.parent = transform.parent;

        Undo.RegisterCreatedObjectUndo(newCopy, "Replaced Object"); //registers undo action for the newly created object & label
        Undo.DestroyObjectImmediate(transform.gameObject); //need to pass in gameobject - can't delete based on transform
    }
}

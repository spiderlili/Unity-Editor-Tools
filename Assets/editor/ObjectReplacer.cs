using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectReplacer : ScriptableWizard
{
    public GameObject replacementPrefab;
    // priority to separate from other utilities under Tools
    [MenuItem("Tools/ReplaceSelection", false, 1000)]
    static void CreateWizard()
    {
        //type of wizard = ObjectReplacer class, when Ynity instantiates a new ScriptableWizard it should be of this type
        ScriptableWizard.DisplayWizard("Object Replacer", typeof(ObjectReplacer), "Replace and close");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

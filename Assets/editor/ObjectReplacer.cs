using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectReplacer : ScriptableWizard
{
    public GameObject replacementPrefab;
    public bool ShowHelperDialogs = true;
    public static string strShowDialogsKey = "ObjectReplacer.showDialogs";

    // priority to separate from other utilities under Tools
    [MenuItem("Tools/Replace Selected Objects", false, 1000)]
    static void CreateWizard()
    {
        //type of wizard = ObjectReplacer class, when Unity instantiates a new ScriptableWizard it should be of this type
        ScriptableWizard.DisplayWizard("Object Replacer", typeof(ObjectReplacer), "Replace and close", "Replace");
    }

    private void OnEnable()
    {
        EditorPrefs.GetBool(strShowDialogsKey, ShowHelperDialogs);
    }

    private void OnWizardCreate() //called when the user clicks on the Create button: check if the error string contains any errors before it continues to execute. 
    {
        if (errorString != "")
        {
            return;
        }
        ReplaceAll();
    }

    private void ReplaceAll()
    {
        if (ShowHelperDialogs)
        {
            if (!EditorUtility.DisplayDialog("Are you sure?", "Are you sure you wish to replace all the selected objects with \"" + replacementPrefab.name + "\"?", "Yes", "Cancel"))
            {
                return;
            }
        }

        //iterate all objects selected. prevent selection from project / assets by selecting topmost selected transform in the scene
        Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.ExcludePrefab);

        int countReplacedObjects = 0;
        foreach (Transform t in transforms)
        {
            if (EditorUtility.DisplayCancelableProgressBar("working..", "replacing " + t.name, countReplacedObjects / (float)transforms.Length))
            {
                break;
            }

            ReplaceObject(t);
            countReplacedObjects++;
        }

        EditorUtility.ClearProgressBar(); //removes the progress bar
        ShowNotification(new GUIContent("Done")); //notification will fade out automatically after some time. TODO: use GUIStyle to define its render style

        if (ShowHelperDialogs)
        {
            EditorUtility.DisplayDialog("Finished replacing", countReplacedObjects + " objects were successfully replaced!", "OK");
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

        //Register an undo operations for the newly created object & label
        Undo.RegisterCreatedObjectUndo(newCopy, "Replaced Object"); 
        //When the undo is performed the object will be destroyed: need to pass in gameobject - can't delete based on transform
        Undo.DestroyObjectImmediate(transform.gameObject);     
    }

    private void OnWizardOtherButton() //provide an action when the user clicks on the other button defined in CreateWizard("Replace")
    {
        if (errorString != "")
        {
            return;
        }
        ReplaceAll();

    }

    private void OnWizardUpdate() //called when the wizard is opened or whenever the user changes something in the wizard
    {
        Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.ExcludePrefab);
        errorString = "";
        helpString = transforms.Length + " objects selected for replacement";
        isValid = true;

        if (replacementPrefab == null)
        {
            errorString += "No replacement object is selected\n"; //add a new error msg on the next line
            isValid = false;
        }
        if (transforms.Length < 1)
        {
            errorString += "No object is selected for replacement\n";
            isValid = false;
        }

        EditorPrefs.SetBool(strShowDialogsKey, ShowHelperDialogs);
    }

    private void OnSelectionChange() //Called whenever the selection has changed
    {
        OnWizardUpdate();
    }
}

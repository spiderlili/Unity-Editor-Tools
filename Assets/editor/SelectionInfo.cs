using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SelectionInfo : MonoBehaviour
{
    [MenuItem("Tools/Selection Info %&i", false, 10)] //ctrl + alt + i
    public static void ShowInfo()
    {
        Debug.Log(Selection.objects.Length + " objects selected");
    }

    [MenuItem("Tools/Selection Info %&i", true)] //determine if the menu item should be enaled or not
    public static bool ShowInfoValidator() 
    {
        return Selection.objects.Length > 0;
    }

    [MenuItem("Tools/Clear Selection %END", false, 20)] //ctrl + backspace
    public static void ClearSelection()
    {
        Selection.activeObject = null;
    }

}

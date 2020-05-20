using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SnapperEditorTool : EditorWindow
{
    [MenuItem("Tools/Advanced Snapper")]
    public static void OpenTool() => GetWindow<SnapperEditorTool>("Advanced Snapper");
    private void OnEnable()
    {
        Selection.selectionChanged += Repaint;
    }

    private void OnDisable()
    {
        Selection.selectionChanged -= Repaint;
    }

    private void OnGUI()
    {
        using(new EditorGUI.DisabledScope(Selection.gameObjects.Length == 0))
        {
            if(GUILayout.Button("Snap Selection"))
            {
                SnapSelection();
            }
        }
    }

    void SnapSelection()
    {
        foreach (GameObject selectedObj in Selection.gameObjects)
        {
            Undo.RecordObject(selectedObj.transform, "Snap objects");
            selectedObj.transform.position = selectedObj.transform.position.Round();
            //Debug.Log("snapped");
        }
    }
}

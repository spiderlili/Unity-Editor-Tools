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

    //TODO
    //add a property to change the granularity of the snapping - set grid size / scale button
    //show the grid in the scene view to visualise the grid snapping, ideally around the objects snapped and where the objects will move to
    //add support for a polar grid: instead of only having support for cartesian coordinates, have a polar grid with angular and radial units to snap to - in this case the grid size scale would also need an angular scale: radial size, angular size
    //ability to place grids in the scene and snap objects to those grids, rather than just having one grid in that editor window.have localised grids in various places in the scene view to snap to.
    //make the grid settings persist between unity sessions
}

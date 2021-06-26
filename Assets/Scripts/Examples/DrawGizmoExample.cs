using UnityEditor;
using UnityEngine;

//implement the gizmo for gameobjects with TargetGizmoAddExample.cs
public class DrawGizmoExample : MonoBehaviour
{
    // This emulates OnDrawGizmos: flags that specify scenarios in which the gizmos will be rendered and their behavior
    [DrawGizmo(GizmoType.NotInSelectionHierarchy | //draws the gizmo if it is not selected and also no parent is selected 
        GizmoType.InSelectionHierarchy | //draws the gizmo if it is selected or if it is a child of the selected
        GizmoType.Selected | //draws the gizmo if it is selected
        GizmoType.Active | //draws the gizmo if it is active (shown in the inspector)
        GizmoType.Pickable)] //the gizmo to be drawn can be picked from the editor

    //any method used for gizmo rendering must be static & take 2 parameters: the object for which the gizmo is being drawn & GizmoType which indicates the context in which the gizmo is being drawn
    private static void CustomOnDrawGizmos(TargetGizmoAddExample targetGizmoAddExample, GizmoType gizmoType)
    {
        Gizmos.color = Color.white;
        Gizmos.DrawCube(targetGizmoAddExample.transform.position, Vector3.one);
    }

    //emulate the behavior of OnDrawGizmosSelected
    [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.Active)]

    private static void CustomDrawGizmosSelected(TargetGizmoAddExample targetGizmoAddExample, GizmoType gizmoType)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(targetGizmoAddExample.transform.position, Vector3.one);
    }

    //Draws a solid box with center and size
    // Method signature: public static void DrawCube(Vector3 center, Vector3 size);
    public Vector3 cubeCenter = Vector3.zero;
    public Vector3 cubeSize = Vector3.one;
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(cubeCenter, cubeSize);
        // Method signature: public static void DrawWireCube(Vector3 center, Vector3 size);
        Gizmos.DrawWireCube(cubeCenter, cubeCenter);
    }

}
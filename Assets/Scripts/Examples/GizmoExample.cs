using UnityEngine;

//goal: implement the OnDrawGizmos and OnDrawGizmosSelected methods to achieve a similar but more flexible solution to adding simple gizmos in Unity
public class GizmoExample : MonoBehaviour
{
    //The OnDrawGizmos method is not called if the component collapses in the inspector window.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawCube(transform.position, Vector3.one);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one);
    }
}
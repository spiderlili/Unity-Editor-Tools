using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ExplosiveObjectsManager : MonoBehaviour
{
    public static List<ExplosiveObjectVisualizer> allTheExplosives = new List<ExplosiveObjectVisualizer>();

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        foreach (ExplosiveObjectVisualizer explosive in allTheExplosives)
        {
            //Gizmos.DrawLine(transform.position, explosive.transform.position);
            Handles.DrawAAPolyLine(transform.position, explosive.transform.position); //antialised line
        }
    }
#endif

}

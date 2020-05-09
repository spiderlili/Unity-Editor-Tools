using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.Networking.PlayerConnection;
using EditorGUIUtility = UnityEditor.EditorGUIUtility;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ExplosiveObjectsManager : MonoBehaviour
{
    public static List<ExplosiveObjectVisualizer> allTheExplosives = new List<ExplosiveObjectVisualizer>();
    public Color visualizerLineColor = Color.white;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        foreach (ExplosiveObjectVisualizer explosive in allTheExplosives)
        {
            //vectors needed to set up the tangents - vertically and halfway between objects for a s-shaped bezier
            Vector3 managerPos = transform.position;
            Vector3 explosivePos = explosive.transform.position;
            float halfHeight = (managerPos.y - explosivePos.y) * 0.5f;
            Vector3 tangentOffset = Vector3.up * halfHeight; 

            //Gizmos.DrawLine(transform.position, explosive.transform.position);
            //Handles.DrawAAPolyLine(transform.position, explosive.transform.position); //antialised line
            Handles.DrawBezier(transform.position, explosive.transform.position, managerPos - tangentOffset, explosivePos + tangentOffset, visualizerLineColor, EditorGUIUtility.whiteTexture, 1f);
        }
    }
#endif

}

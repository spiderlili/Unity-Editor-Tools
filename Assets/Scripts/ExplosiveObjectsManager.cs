using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEditor.Experimental.Networking.PlayerConnection;
using EditorGUIUtility = UnityEditor.EditorGUIUtility;
using UnityEngine.Rendering;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ExplosiveObjectsManager : MonoBehaviour
{
    public static List<ExplosiveObjectVisualizer> allTheExplosives = new List<ExplosiveObjectVisualizer>();
    //public Color visualizerLineColor = Color.white;

    public static void UpdateAllExplosivesColors()
    {
        foreach(ExplosiveObjectVisualizer explosiveObj in allTheExplosives)
        {
            explosiveObj.ApplyColor();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.zTest = CompareFunction.LessEqual;

        foreach (ExplosiveObjectVisualizer explosive in allTheExplosives)
        {
            if (explosive.explosiveType == null)
            {
                continue;
            }
            //vectors needed to set up the tangents - vertically and halfway between objects for a s-shaped bezier
            Vector3 managerPos = transform.position;
            Vector3 explosivePos = explosive.transform.position;
            float halfHeight = (managerPos.y - explosivePos.y) * 0.5f;
            Vector3 tangentOffset = Vector3.up * halfHeight;

            //replace visualizerLineColor with explosive.meshColor to color the lines the same color as the object
            Handles.color = explosive.explosiveType.meshColor;

            //Gizmos.DrawLine(transform.position, explosive.transform.position);
            //Handles.DrawAAPolyLine(transform.position, explosive.transform.position); //antialised line
            Handles.DrawBezier(transform.position, explosive.transform.position, managerPos - tangentOffset, explosivePos + tangentOffset, explosive.explosiveType.meshColor, EditorGUIUtility.whiteTexture, 1f);
        }
        //reset to default to avoid the next objects being colored
        Handles.color = Color.white; 
    }
#endif

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveObjectsManager : MonoBehaviour
{
    public static List<ExplosiveObjectVisualizer> allTheExplosives = new List<ExplosiveObjectVisualizer>();

    private void OnDrawGizmos()
    {
        foreach (ExplosiveObjectVisualizer explosive in allTheExplosives)
        {
            Gizmos.DrawLine(transform.position, explosive.transform.position);
        }
    }

}

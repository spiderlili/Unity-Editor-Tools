using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ExplosiveObjectVisualizer : MonoBehaviour
{
    [Range(1f, 8f)]
    public float radiusOfExplosion = 1;
    public float damage = 10;
    public Color color = Color.red;

    private void OnEnable()
    {
        ExplosiveObjectsManager.allTheExplosives.Add(this);
    }

    private void OnDisable()
    {
        ExplosiveObjectsManager.allTheExplosives.Remove(this);
    }

    private void OnDrawGizmosSelected()
    {
        //display a radius wireframe
        Gizmos.DrawWireSphere(transform.position, radiusOfExplosion);        
    }
}

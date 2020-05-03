using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveObjectVisualizer : MonoBehaviour
{
    [Range(1f, 8f)]
    public float radiusOfExplosion = 1;
    public float damage = 10;
    public Color color = Color.red;

    private void OnDrawGizmosSelected()
    {
        //display a radius wireframe
        Gizmos.DrawWireSphere(transform.position, radiusOfExplosion);        
    }
}

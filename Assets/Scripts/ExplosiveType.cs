using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (order = 50)]
public class ExplosiveType : ScriptableObject
{
    [Range(1f, 8f)]
    public float radiusOfExplosion = 1;
    public float damage = 10;
    public Color meshColor = Color.white;
}

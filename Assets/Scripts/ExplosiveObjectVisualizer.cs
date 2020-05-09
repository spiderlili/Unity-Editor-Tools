using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ExplosiveObjectVisualizer : MonoBehaviour
{
    [Range(1f, 8f)]
    public float radiusOfExplosion = 1;
    public float damage = 10;
    public Color meshColor = Color.white;
    static readonly int shaderPropertyColor = Shader.PropertyToID("_Color");
    MaterialPropertyBlock mpb;

    public MaterialPropertyBlock materialPropertyblock
    {
        get
        {
            if (mpb == null)
            {
                mpb = new MaterialPropertyBlock();
            }
            return mpb;
        }
    }

    private void ApplyColor()
    {
        MeshRenderer rend = GetComponent<MeshRenderer>();
        materialPropertyblock.SetColor(shaderPropertyColor, meshColor);
        rend.SetPropertyBlock(materialPropertyblock);
    }

    //called on property change
    private void OnValidate()
    {
        ApplyColor();
    }

    private void OnEnable()
    {
        ExplosiveObjectsManager.allTheExplosives.Add(this);
        ApplyColor();
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

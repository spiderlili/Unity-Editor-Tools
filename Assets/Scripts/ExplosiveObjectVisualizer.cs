using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class ExplosiveObjectVisualizer : MonoBehaviour
{
    public ExplosiveType explosiveType;
    static readonly int shaderPropertyColor = Shader.PropertyToID("_Color");
    MaterialPropertyBlock mpb; //not serialized

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

    public void ApplyColor()
    {
        if (explosiveType == null)
        {
            return;
        }
        MeshRenderer rend = GetComponent<MeshRenderer>();
        materialPropertyblock.SetColor(shaderPropertyColor, explosiveType.meshColor);
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
        if(explosiveType == null)
        {
            return;
        }
        Gizmos.color = explosiveType.meshColor;
        //display radius circle horizontally
        //Handles.DrawWireDisc(transform.position, transform.up, radiusOfExplosion);
        //display a radius wireframe
        Gizmos.DrawWireSphere(transform.position, explosiveType.radiusOfExplosion);
        Gizmos.color = Color.white; //reset to default to avoid coloring next objects to be drawn
    }
}

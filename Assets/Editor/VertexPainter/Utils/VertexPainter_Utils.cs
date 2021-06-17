using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class VertexPainter_Utils
{
    //Retrieve mesh filter or a skinnedmeshrenderer & return the mesh attached to the geometry the user's mouse is hovering over currently
    public static Mesh GetMesh(GameObject objGeo)
    {
        Mesh currentMesh = null;
        if (objGeo != null)
        {
            MeshFilter currentFilter = objGeo.GetComponent<MeshFilter>();
            SkinnedMeshRenderer currentSkinnedMesh = objGeo.GetComponent<SkinnedMeshRenderer>();
            if (currentFilter && !currentSkinnedMesh)
            {
                currentMesh = currentFilter.sharedMesh;
            }
            else if (!currentFilter && currentSkinnedMesh)
            {
                currentMesh = currentSkinnedMesh.sharedMesh;
            }
            else
            {
                return null;
            }
        }
        return currentMesh;
    }
}

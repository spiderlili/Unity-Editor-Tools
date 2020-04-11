using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CubeEnhanced : MonoBehaviour
{
    [Header("Transform Variables")]

    [Space(10)]

    [Tooltip("Edit position of the cube")]
    public Vector3 position;
    [Tooltip("Edit rotation of the cube")]
    public Vector3 rotation;
    [Tooltip("Edit size of the cube")]
    [Range(1f, 10f)]
    public float size = 1f;

    // Update is called once per frame
    void Update()
    {
        transform.position = position;
        transform.eulerAngles = rotation;
        transform.localScale = new Vector3(size, size, size);
    }
}

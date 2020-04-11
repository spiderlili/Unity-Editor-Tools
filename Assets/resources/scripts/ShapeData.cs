using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Shapes;

[CreateAssetMenu(fileName = "New Custom Shape", menuName = "Custom Shape")]
public class ShapeData : ScriptableObject
{
    public ShapeTypes shapeTypes;
    public Vector3 position;
    public string shapeName;

}

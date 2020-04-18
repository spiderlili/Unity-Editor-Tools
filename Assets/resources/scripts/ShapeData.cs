using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Shapes;

[CreateAssetMenu(fileName = "New Custom Shape", menuName = "Custom Shape", order = 50)] //set priority in the asset create menu
public class ShapeData : ScriptableObject
{
    public ShapeTypes shapeTypes;
    public Vector3 position;
    public string shapeName;

}

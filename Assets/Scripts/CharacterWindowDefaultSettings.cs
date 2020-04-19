using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "DnD Character Window Settings", menuName = "DnD Character Window Settings Data", order = 50)]
public class CharacterWindowDefaultSettings : ScriptableObject
{
    public Color HighValueColor = Color.green;
    public Color MidValueColor = Color.yellow;
    public Color LowValueColor = Color.red;
}

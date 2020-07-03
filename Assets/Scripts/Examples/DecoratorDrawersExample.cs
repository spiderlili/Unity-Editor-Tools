using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//decorator drawers are designed to draw decoration in the inspector - unassociated with a specific field
public class DecoratorDrawersExample : MonoBehaviour
{
    [Header("Group of variables")]
    public int varA = 10;
    public int varB = 20;

    [Space(40)] 
    public int spacedOutVarD = 20;

    [Tooltip("This is a tooltip")]
    public int ReadToolTip = 30;
}

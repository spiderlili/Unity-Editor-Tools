using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeAttribute : PropertyAttribute
{
    public readonly bool DisplayHours;
    public TimeAttribute(bool displayHours = false) //default: Time attribute will display in m:s format
    {
        DisplayHours = displayHours; //if true: display time in h:m:s format
    }
}

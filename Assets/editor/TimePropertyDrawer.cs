using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(TimeAttribute))] //bind a drawer with a Property attribute
public class TimePropertyDrawer : PropertyDrawer //property drawer doesn't support layouts to create a GUI: use EditorGUI/GUI
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) //handle drawer height
    {
        return EditorGUI.GetPropertyHeight(property) * 2;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if(property.propertyType == SerializedPropertyType.Integer)
        {
            property.intValue = EditorGUI.IntField(new Rect(position.x, position.y, position.width, position.height / 2), 
            label, Mathf.Max(0, property.intValue));
            EditorGUI.LabelField(new Rect(position.x, position.y + position.height / 2, position.width, position.height / 2),
            " ", TimeFormat(property.intValue));
        }
        else
        {
            EditorGUI.HelpBox(position, "To use the Time attribute \"" + label.text + "\" must be an int!", MessageType.Error);
        }
    }

    private string TimeFormat(int totalSeconds)
    {
        TimeAttribute time = attribute as TimeAttribute;

        //use string.Format to add the variables in the string
        if (time.DisplayHours)
        {
            int hours = totalSeconds / (60 * 60);
            int minutes = ((totalSeconds % (60 * 60)) / 60);
            int seconds = (totalSeconds % 60);
            return string.Format("{0}:{1}:{2} (h:m:s)", hours, minutes.ToString().PadLeft(2, '0'), 
            seconds.ToString().PadLeft(2, '0'));
        }
        else
        {
            int minutes = (totalSeconds / 60);
            int seconds = (totalSeconds % 60);
            return string.Format("{0}:{1} (m:s)", minutes.ToString(), seconds.ToString().PadLeft(2, '0'));
        }
    }
}

using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TimeAttribute))]
public class TimeDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property) * 2;
    }
    /*

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label);
    {
        if (property.propertyType == SerializedPropertyType.Integer) {
            property.intValue = EditorGUI.IntField(new Rect(position.x, position.y, position.width, position.height / 2), label, Mathf.Max(0, property.intValue));
            EditorGUI.LabelField (new Rect (position.x, position.y + position.height / 2, position.width, position.height / 2), " ",
                TimeFormat (property.intValue));
        } else {
            EditorGUI.HelpBox (position, "To use the Time attribute \"" + label.text + "\" must be an int!", MessageType.Error);
        }
    }

    private string TimeFormat(int totalSeconds)
    {
        TimeAttribute time = attribute as TimeAttribute;
        
    }
    */
}

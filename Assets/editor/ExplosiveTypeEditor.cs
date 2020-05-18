using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//support the editing of multiple objects
[CanEditMultipleObjects]

//extend the inspector on scriptable object templates
[CustomEditor(typeof(ExplosiveType))]
public class ExplosiveTypeEditor : Editor
{
    SerializedObject so;
    SerializedProperty propRadius;
    SerializedProperty propDamage;
    SerializedProperty propColor;

    public enum ExplosiveObject 
    { 
        barrel,
        doll
    };

    private void OnEnable()
    {
        so = serializedObject;
        propRadius = so.FindProperty("radiusOfExplosion");
        propDamage = so.FindProperty("damage");
        propColor = so.FindProperty("meshColor");
    }

    public override void OnInspectorGUI()
    {
        so.Update();
        EditorGUILayout.PropertyField(propRadius);
        EditorGUILayout.PropertyField(propDamage);
        EditorGUILayout.PropertyField(propColor);

        if (so.ApplyModifiedProperties()) //if something changed
        {
            ExplosiveObjectsManager.UpdateAllExplosivesColors();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//extend the inspector on scriptable object templates
[CustomEditor(typeof(ExplosiveType))]
public class ExplosiveTypeEditor : Editor
{
    public enum ExplosiveObject 
    { 
        barrel,
        doll
    };

    ExplosiveObject explosiveObjects;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        using(new GUILayout.HorizontalScope())
        {
            GUILayout.Label("Explosive Object");
            explosiveObjects = (ExplosiveObject)EditorGUILayout.EnumPopup(explosiveObjects);
        }

        EditorGUILayout.ObjectField("Assign transform: ", null, typeof(Transform), true);
    }
}

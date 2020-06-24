using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Convert_Editor : MonoBehaviour
{
    [CustomEditor(typeof(Convert_Texture_HDRP))]
    public class ObjectBuilderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            //DrawDefaultInspector();

            Convert_Texture_HDRP myScript = (Convert_Texture_HDRP)target;
            //if (GUILayout.Button("Build Object"))
            //{
            //    myScript.Convert();
            //}
            //if (GUILayout.Button("Name_Checking"))
            //{
            //    myScript.Generate();
            //}
            //if (GUILayout.Button("Name_Checking"))
            //{
            //    myScript.CheckSize();
            //}
        }
    }
}

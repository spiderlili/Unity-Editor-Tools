using System;
using UnityEditor;
using UnityEngine;

public class ScreenshotExporterEditorWindow : EditorWindow
{
    private string fileName;
    private string filePath;
    private static readonly string DefaultFileName = "Screenshot";
    private static readonly string DefaultFileDirectory = "Assets/";
    
    [MenuItem("Tools/Screenshot Exporter")]
    private static void ShowWindow()
    {
        var window = GetWindow<ScreenshotExporterEditorWindow>();
        window.titleContent = new GUIContent("Screenshot Exporter");
        window.Show();
    }

    private void OnGUI()
    {
        fileName = EditorGUILayout.TextField("Screenshot File Name: ", DefaultFileName);
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Save to Folder: ");
        if (GUILayout.Button("...")) {
            filePath = EditorUtility.OpenFolderPanel("Choose Folder", DefaultFileDirectory, "");
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        if (GUILayout.Button("Take Screenshot")) {
            string path = $"{filePath}/{fileName}.png";
            ScreenCapture.CaptureScreenshot(path);
        }
        
    }
}

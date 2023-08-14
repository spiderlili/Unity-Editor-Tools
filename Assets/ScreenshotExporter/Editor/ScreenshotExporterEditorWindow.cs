using System;
using NUnit.Framework.Internal;
using UnityEditor;
using UnityEngine;
using System.IO;

public class ScreenshotExporterEditorWindow : EditorWindow
{
    [Tooltip("1 means default size")]
    private int superSize;
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
        superSize = EditorGUILayout.IntSlider("Enlarge Size", superSize, 1, 5);
        fileName = EditorGUILayout.TextField("Screenshot File Name: ", fileName);
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Save to Folder: ");
        if (GUILayout.Button("...")) {
            filePath = EditorUtility.OpenFolderPanel("Choose Folder", DefaultFileDirectory, "");
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.LabelField(filePath); // Visualise file path to save the screenshot
        EditorGUILayout.Space();

        if (GUILayout.Button("Take Screenshot")) {
            Camera cam = Camera.main;
            string path = $"{filePath}/{fileName}.png";
            // ScreenCapture.CaptureScreenshot(path, superSize);
            
            // Approach 1: saving a render texture
            RenderTexture rt = RenderTexture.GetTemporary(1208, 720, 0, RenderTextureFormat.ARGB32);
            cam.targetTexture = rt;
            cam.Render();
            RenderTexture.active = rt;
            
            // Saving a texture as Texture2D manually - can define custom size. no mipmaps  
            Texture2D tex = new Texture2D(1280, 720, TextureFormat.ARGB32, false);
            tex.ReadPixels(new Rect(0,0,1280,720), 0, 0);
            tex.Apply();
            // Encode texture as bytes
            byte[] bytes;
            bytes = tex.EncodeToPNG();  
            // Save bytes into texture
            File.WriteAllBytes(path, bytes);

            // Restore camera settings to default 
            cam.targetTexture = null;
            // Output results to screen
            RenderTexture.ReleaseTemporary(rt);
            // Refresh the project window to show the newly created screenshot
            AssetDatabase.Refresh();
        }
    }
}

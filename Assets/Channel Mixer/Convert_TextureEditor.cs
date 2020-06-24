using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class Convert_TextureEditor : EditorWindow
{
    [MenuItem("Window/Channel Mixer")]
    public static void ShowWindow()
    {
        Type inspectorType = Type.GetType("UnityEditor.InspectorWindow,UnityEditor.dll");
        Convert_TextureEditor MyWindow = EditorWindow.GetWindow<Convert_TextureEditor>(new Type[] { inspectorType });
        MyWindow.title = "Channel Mixer";
        MyWindow.minSize = new Vector2(438, 400);
        MyWindow.maxSize = new Vector2(600, 428);

        MyWindow.Show();
        
    }
    Shader cShader;

    Texture2D RedC;
    Texture2D BlueC;
    Texture2D GreenC;
    Texture2D AlphaC;

    bool RedCI = false;
    bool BlueCI = false;
    bool GreenCI = false;
    bool AlphaCI = false;

    bool AEnabled = false;

    GUILayout TexLayout;
    GUIStyle buttonS;
    GUIStyle NoPad;
    GUIStyle Toggle;
    GUIStyle Button_Middle;
    
    int tSize = 3;

    string iName = "Packed_Image";
    string iDir = "";
    bool Override = true;

    Convert_Texture_HDRP C_T_Script;
    GameObject C_T_O;
    Material C_T_M;


    string[] tSizeO;
    bool Checked = false;
    void CheckTexel()
    {
    tSizeO = new string[]
        {
            "128",
            "256",
            "512",
            "1024",
            "2048",
            "4096"
        };

        for (int x = 0; x < tSizeO.Length; x++)
        {
            if (C_T_Script.Size.ToString() == tSizeO[x])
            {
                tSize = x;
                break;
            }
        }
        Checked = true;
    }
    void ChangeMat()
    {
        Debug.Log("Change Tex");
        C_T_M.SetTexture("_Red_Channel", RedC);
        C_T_M.SetTexture("_Green_Channel", GreenC);
        C_T_M.SetTexture("_Blue_Channel", BlueC);
        C_T_M.SetTexture("_Alpha_Channel", AlphaC);
        C_T_M.SetInt("_R_Invert", Convert.ToInt32(RedCI));
        C_T_M.SetInt("_G_Invert", Convert.ToInt32(GreenCI));
        C_T_M.SetInt("_B_Invert", Convert.ToInt32(BlueCI));
        C_T_M.SetInt("_A_Invert", Convert.ToInt32(AlphaCI));
    }

    void GetScript()
    {
        Checked = false;
        if (C_T_O.GetComponent<Convert_Texture_HDRP>() == null)
        {
            C_T_Script = C_T_O.AddComponent<Convert_Texture_HDRP>();
        }
        C_T_Script = C_T_O.GetComponent<Convert_Texture_HDRP>();
        C_T_Script.Generate();
        C_T_M = C_T_Script.mat;
    }

    bool allowExport = true;
    private void OnGUI()
    {
        #region Title_Load
        LoadLooks();
        EditorGUILayout.BeginHorizontal("box", GUILayout.Height(60), GUILayout.ExpandHeight(false));
        GUILayout.FlexibleSpace();
        GUILayout.Label("Channel Mixer", buttonS, GUILayout.Height(50));
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        #endregion
        #region MainObjects
        EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField(C_T_O, typeof(GameObject), false);
        EditorGUILayout.ObjectField(C_T_M, typeof(Material), false);
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndHorizontal();
        #endregion
        
        if (C_T_O == null)
        {
            #region Fetch
            if (GameObject.FindObjectOfType<Convert_Texture_HDRP>() != null)
            {
                C_T_O = GameObject.FindObjectOfType<Convert_Texture_HDRP>().gameObject;
                GetScript();
            }
            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.BeginHorizontal("box");
            if(GUILayout.Button("Initialize", Button_Middle, GUILayout.Height(32)))
            {
                C_T_O = new GameObject();
                C_T_O.name = "Channel Mixer[Main]";
                GetScript();

            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.HelpBox("Press the Initlize button to start converting", MessageType.Error, true);
            EditorGUILayout.EndHorizontal();
            #endregion
        }
        else
        {
            #region LastCheck
            if(C_T_Script == null || C_T_Script)
            {
                GetScript();
            }
            if(!Checked)
            CheckTexel();
            if(C_T_Script.TName == "" || C_T_Script.Path == ""){
                 allowExport = false;
                 EditorGUILayout.HelpBox("Output Name nor Output Directory should not be empty", MessageType.Error, true);
             }
             else
             {
                 allowExport = true;
             }
            #endregion
            #region Upper_Pannel
            EditorGUILayout.BeginVertical("box");
        
            #region FileName
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Output Name: ", GUILayout.Width(190), GUILayout.ExpandWidth(false));
            C_T_Script.TName = EditorGUILayout.TextField(C_T_Script.TName, GUILayout.ExpandWidth(true));
            #endregion
            EditorGUILayout.EndHorizontal();
            #region Directory
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Output Directory: ", GUILayout.Width(190), GUILayout.ExpandWidth(false));
            C_T_Script.Path = EditorGUILayout.TextField(C_T_Script.Path, GUILayout.ExpandWidth(true), GUILayout.MaxWidth(1200));
            EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("D", smallButton, GUILayout.ExpandWidth(false), GUILayout.Width(16)))
                {
                    string nPath = EditorUtility.OpenFolderPanel("Set directory of Output Texture", "", "");
                    if (nPath != "") {
                        C_T_Script.Path = nPath;
                    }
                    C_T_Script.PathGen();
                    GUI.FocusControl(null);
                }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("R", smallButton, GUILayout.ExpandWidth(false), GUILayout.Width(16)))
                {
                    C_T_Script.rPath();
                    GUI.FocusControl(null);
                }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();
            #endregion
            TextureSize();
            #region Override     
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Overwrite", GUILayout.ExpandWidth(false));
            C_T_Script.Overwrite = GUILayout.Toggle(C_T_Script.Overwrite, "", GUILayout.MaxWidth(15));
            EditorGUILayout.EndHorizontal();
            #endregion
            EditorGUILayout.EndVertical();
            #endregion

            #region Bottom_Box
            EditorGUILayout.BeginHorizontal("box");
            #region Button
            EditorGUILayout.BeginVertical();
        
            RedCI = AddButton(RedCI, "Red", ref RedC);
            GreenCI = AddButton(GreenCI, "Green", ref GreenC);
            BlueCI = AddButton(BlueCI, "Blue", ref BlueC);
            AlphaCI = AddButton(AlphaCI, "Alpha", ref AlphaC);
            EditorGUILayout.EndVertical();
            #endregion
            EditorGUILayout.BeginHorizontal();
            #region Preview_Export
            EditorGUILayout.BeginVertical("box");
                EditorGUILayout.ObjectField(C_T_Script.NewT, typeof(Texture2D), false, GUILayout.Height(90), GUILayout.Width(90));
                //Real previewer
                //EditorGUI.DrawPreviewTexture(new Rect(0,0, 90, 90), C_T_Script.NewT, null, ScaleMode.StretchToFill);
                EditorGUILayout.BeginHorizontal("box");
            if (GUILayout.Button("Generate", "Label"))
                {
                    ChangeMat();
                    C_T_Script.Convert();
                }
            EditorGUILayout.EndHorizontal();
            if (allowExport)
                {
                EditorGUILayout.BeginHorizontal("box");
        
            if (GUILayout.Button("Export", "Label"))
                {
                
                    ChangeMat();
                    C_T_Script.Export();
                }
                EditorGUILayout.EndHorizontal();
                }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            #endregion
            EditorGUILayout.EndHorizontal();
            #endregion
            #region TODO_01
            /*TODO, RGBA Previewer
            if (previewTextrue == null)
            {
                GetMesh();
                Initialize();
            }
            Rect rect = new Rect(0, 400, 200, 200); 
            //previewTextrue.BeginPreview(rect, "");
            //previewTextrue.DrawMesh(rMesh, Vector3.one, Quaternion.identity, C_T_M, 0);
            //previewTextrue.EndAndDrawPreview(rect);
            //Texture2D nPreview = AssetPreview.GetAssetPreview(C_T_Script.mat);
            //EditorGUI.DrawPreviewTexture(new Rect(0, 0, 256, 256), nPreview); 
            */
            #endregion
        }
    }
    #region TODO_Preview RGBA
    /*TODO, RGBA Previewer
    Mesh rMesh;
    void GetMesh()
    {
        rMesh = new Mesh();
        rMesh = ( C_T_Script.rPlane.GetComponent<MeshFilter>().sharedMesh);
    }
    void Initialize()
    {
        previewTextrue = new PreviewRenderUtility();
        //GameObject rPrevew = C_T_Script.rPlane;
        //previewTextrue.AddSingleGO(rPrevew, true);
    }
     PreviewRenderUtility previewTextrue;
    */

    //Editor gameObjectEditor;
    #endregion
    bool AddButton(bool pAt, string Channel, ref Texture2D TextureC)
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.BeginHorizontal("box");

        TextureC = (Texture2D)EditorGUILayout.ObjectField(TextureC, typeof(Texture2D), false, GUILayout.MaxWidth(200));
        GUILayout.Space(2);
        GUILayout.Label(Channel + " Channel");
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal(NoPad);
        pAt = GUILayout.Toggle((bool)pAt, "", Toggle, GUILayout.MaxWidth(10));
        GUILayout.Label("Invert", GUILayout.ExpandWidth(false));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        return pAt;
    }


    GUIStyle smallButton;

    void LoadLooks()
    {
        smallButton = new GUIStyle();

        buttonS = new GUIStyle();
        NoPad = new GUIStyle();
        Toggle = new GUIStyle(EditorStyles.toggle);
        Button_Middle = new GUIStyle();
        Button_Middle.alignment = TextAnchor.MiddleCenter;
        Button_Middle.normal.textColor = Color.white;
        Button_Middle.padding = new RectOffset(5, 5, 5, 5);

        buttonS.normal.background = null;
        buttonS.alignment = TextAnchor.MiddleCenter;
        buttonS.normal.textColor = Color.white;
        buttonS.fontSize = 16;
        NoPad.margin = new RectOffset(6, 5, 1, 1);
        NoPad.stretchWidth = false;
        Toggle.normal.textColor = Color.white;
        Toggle.onNormal.textColor = Color.red;

        smallButton = new GUIStyle(EditorStyles.numberField);
        smallButton.fontSize = 9;
        smallButton.normal.textColor = Color.white;
        smallButton.margin = new RectOffset(1, 4, 2, 3);
        smallButton.padding = new RectOffset(1, 1, 3, 2);
        smallButton.alignment = TextAnchor.MiddleCenter;

    }
    void TextureSize()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Output Size: ", GUILayout.Width(190), GUILayout.ExpandWidth(false));
        
        
        tSize = EditorGUILayout.Popup(tSize, tSizeO);
        
        C_T_Script.Size = int.Parse(tSizeO[tSize]);
        EditorGUILayout.EndHorizontal();
    }
}

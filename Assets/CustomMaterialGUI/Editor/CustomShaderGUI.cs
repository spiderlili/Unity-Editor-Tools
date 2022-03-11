﻿using UnityEditor;
using UnityEngine;

public class CustomShaderGUI : ShaderGUI
{
    private MaterialProperty debugFloatProp;
    private MaterialProperty debugRangeProp;
    private MaterialProperty debugVectorProp;
    private bool isFloatEnabled;
    private bool isVectorEnabled;
    private int debugVectorToIntPorpX;
    private float debugVectorToFloatPropY;
    private float debugVectorToFloatPropZ;
    private float debugVectorToFloatPropW;
    private MaterialProperty debugColorProp;
    private MaterialProperty mainTexProp;
    private Material mat;
    
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        mat = materialEditor.target as Material; // Get current material
        
        // Float & Range
        isFloatEnabled = EditorGUILayout.Foldout(isFloatEnabled, "Foldout Label");
        EditorGUILayout.LabelField("Float Header Label", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        debugRangeProp = FindProperty("_DebugRange", properties);
        materialEditor.RangeProperty(debugRangeProp, "Debug Float Custom Slider");
        materialEditor.FloatProperty(debugRangeProp, "Debug Float Custom Label");
        
        // Warning if user exceeds the recommended value range in the slider
        if (debugRangeProp.floatValue > 1 || debugRangeProp.floatValue < 0) {
            EditorGUILayout.HelpBox("Warning: It's best to use a float value in range of [0,1]", MessageType.Warning);
        }
        EditorGUILayout.EndVertical();
        
        // Vector4 to IntField for readability, group them into a Toggle Group to manage shader variants & control parameter interactibility.
        EditorGUILayout.Space();
        isVectorEnabled =  EditorGUILayout.BeginToggleGroup("Toggle Group", isVectorEnabled);
        if (mat != null) {
            isVectorEnabled = mat.IsKeywordEnabled("_VECTOR_ENABLED_ON");
            if (isVectorEnabled) {
                mat.EnableKeyword("_VECTOR_ENABLED_ON");
            } else {
                mat.DisableKeyword("_VECTOR_ENABLED_ON");
            }
        }
        
        EditorGUILayout.LabelField("Vector Header Label", EditorStyles.boldLabel);
        debugVectorProp = FindProperty("_DebugVector", properties);
        materialEditor.VectorProperty(debugVectorProp, "Vector4");
        
        EditorGUILayout.HelpBox("A vector4 can be split into 4 different float values using EditorGUILayout", MessageType.Info);
        
        // Read values set in the material property: otherwise they will be overwritten by default values on UI refresh!
        debugVectorToIntPorpX = (int)debugVectorProp.vectorValue.x;
        debugVectorToFloatPropY = (float)debugVectorProp.vectorValue.y;
        debugVectorToFloatPropZ = (int)debugVectorProp.vectorValue.z;
        debugVectorToFloatPropW = (int)debugVectorProp.vectorValue.w;
        
        debugVectorToIntPorpX = EditorGUILayout.IntField("Vector4ToInt.x", debugVectorToIntPorpX);
        debugVectorToFloatPropY = EditorGUILayout.Slider("Vector4ToFloatSlider.y", debugVectorToFloatPropY, -5,5);
        EditorGUILayout.MinMaxSlider("Vector4.zw To MinMax Slider", ref debugVectorToFloatPropZ, ref debugVectorToFloatPropW, -10, 10);
        Vector4 newVector = new Vector4(debugVectorToIntPorpX,debugVectorToFloatPropY,debugVectorToFloatPropZ,debugVectorToFloatPropW);
        debugVectorProp.vectorValue = newVector;
        EditorGUILayout.EndToggleGroup();
        
        // Color & Texture: wider color field & narrower texture field by default
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Color & Texture Header Label", EditorStyles.boldLabel);
        debugColorProp = FindProperty("_DebugColor", properties);
        materialEditor.ColorProperty(debugColorProp, "Color");
        mainTexProp = FindProperty("_MainTex", properties);
        materialEditor.TextureProperty(mainTexProp, "Main Texture (Default Tiling Offset)");
        materialEditor.TexturePropertySingleLine(new GUIContent("Single Line Texture (No Tiling Offset)"), mainTexProp);
        materialEditor.TexturePropertyTwoLines(new GUIContent("Main Texture 2 Lines"), mainTexProp, debugColorProp, 
            new GUIContent("Line 2 Test (Vetor4 Debug)"), debugVectorProp);
        materialEditor.TexturePropertyWithHDRColor(new GUIContent("HDR + Texture"), mainTexProp, debugColorProp, true);
        materialEditor.TextureScaleOffsetProperty(mainTexProp);
    } 
}

using UnityEditor;
using UnityEngine;
using UnityEditor.AnimatedValues; // For fold out animations
using System;
using UnityEngine.Rendering;

public class CustomShaderGUI : ShaderGUI
{
    #region [Properties]
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
    private MaterialProperty _saveFoldoutProp01;
    private Material mat;
    private MaterialProperty saveBlendProp, srcBlendProp, dstBlendProp;
    private enum BlendMode
    {
        Additive,
        AlphaBlend
    }
    private string[] blendModeNames = System.Enum.GetNames(typeof(BlendMode));

    // Animation
    private AnimBool animBool01 = new AnimBool(true);
    #endregion

    #region [Redraw Custom UI]
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        mat = materialEditor.target as Material; // Get current material
        
        // Custom Blend Modes
        #region [BlendModes]
        saveBlendProp = FindProperty("_SaveBlendState", properties);
        srcBlendProp = FindProperty("_SrcBlend", properties);
        dstBlendProp = FindProperty("_DstBlend", properties);
        saveBlendProp.floatValue = EditorGUILayout.Popup("Blend Mode", (int)saveBlendProp.floatValue, blendModeNames);
        
        switch (saveBlendProp.floatValue) {
            case 0: // Additive
                srcBlendProp.floatValue = (int)UnityEngine.Rendering.BlendMode.One;
                dstBlendProp.floatValue = (int)UnityEngine.Rendering.BlendMode.One;
                break; 
            case 1: // Alpha Blend
                srcBlendProp.floatValue = (int)UnityEngine.Rendering.BlendMode.SrcAlpha;
                dstBlendProp.floatValue = (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha;
                break;
        }
        #endregion
        // Save the foldout enabled value so it stays consistent in MaterialProperties & remembers the user's choice of foldout rather than switching to default value
        _saveFoldoutProp01 = FindProperty("_SaveDebugVectorFoldoutVal01", properties);
        isFloatEnabled = _saveFoldoutProp01.vectorValue.x != 0;

        // Float & Range
        isFloatEnabled = EditorGUILayout.Foldout(isFloatEnabled, "Foldout Label");
        if (isFloatEnabled) {
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
        }
        // Vector4 to IntField for readability, group them into a Toggle Group to manage shader variants & control parameter interactibility.
        EditorGUILayout.Space();
        if (mat != null) {
            isVectorEnabled = mat.IsKeywordEnabled("_VECTOR_ENABLED_ON");
            isVectorEnabled =  EditorGUILayout.BeginToggleGroup("Vector Toggle Group", isVectorEnabled);
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
        
        // Add Animation to foldout (Target to tween towards)
        EditorGUILayout.Space();
        animBool01.target = _saveFoldoutProp01.vectorValue.y != 0;
        animBool01.target = EditorGUILayout.Foldout(animBool01.target, "Foldout Group with Animation (Texture & Color)", EditorStyles.boldFont);
        if (EditorGUILayout.BeginFadeGroup(animBool01.faded)) {
            // Color & Texture: wider color field & narrower texture field by default
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
        EditorGUILayout.EndFadeGroup();
        
        // Save the foldout enabled value so it stays consistent in MaterialProperties & remembers the user's choice of foldout rather than switching to default value
        float _saveValue01XFloatEnabled = isFloatEnabled ? 1 : 0;
        float _saveValue01YAnimBool = animBool01.target ? 1 : 0;
        Vector4 _saveValue01 = new Vector4(_saveValue01XFloatEnabled,_saveValue01YAnimBool,0,0);
        _saveFoldoutProp01.vectorValue = _saveValue01;
        
        // Extra options: Queue, GPU Instancing, Double Sided GI
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        // Alternative: Custom appearance using GUIStyle
        // EditorGUILayout.BeginVertical(new GUIStyle("window"));
        EditorGUILayout.LabelField("Advanced Extra Options", EditorStyles.boldLabel);
        materialEditor.RenderQueueField();
        materialEditor.EnableInstancingField();
        materialEditor.DoubleSidedGIField();
        EditorGUILayout.EndVertical();
    } 
    #endregion
}

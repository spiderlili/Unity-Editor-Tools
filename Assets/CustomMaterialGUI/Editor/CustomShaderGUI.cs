using UnityEditor;
using UnityEngine;

public class CustomShaderGUI : ShaderGUI
{
    private MaterialProperty debugFloatProp;
    private MaterialProperty debugRangeProp;
    private MaterialProperty debugVectorProp;
    private int debugVectorToIntPorpX;
    private float debugVectorToFloatPropY;
    private float debugVectorToFloatPropZ;
    private float debugVectorToFloatPropW;
    private MaterialProperty debugColorProp;
    private MaterialProperty mainTexProp;
    
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        debugRangeProp = FindProperty("_DebugRange", properties);
        materialEditor.RangeProperty(debugRangeProp, "Debug Float Custom Slider");
        materialEditor.FloatProperty(debugRangeProp, "Debug Float Custom Label");
        
        //vector4 to IntField for readability
        debugVectorProp = FindProperty("_DebugVector", properties);
        materialEditor.VectorProperty(debugVectorProp, "Vector4");
        
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
        
        // Color & Texture: wider color field & narrower texture field by default
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

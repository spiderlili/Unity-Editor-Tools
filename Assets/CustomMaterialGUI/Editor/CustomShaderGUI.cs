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
    private MaterialProperty baseColorProp;
    
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
    }
}

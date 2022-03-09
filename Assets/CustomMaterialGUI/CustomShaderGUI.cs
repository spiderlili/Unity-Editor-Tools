using UnityEditor;

public class CustomShaderGUI : ShaderGUI
{
    private MaterialProperty debugFloatProp;
    private MaterialProperty debugRangeProp;
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        debugRangeProp = FindProperty("_DebugRange", properties);
        //materialEditor.FloatProperty(debugFloatProp, "Debug Float Custom Label: ");
        materialEditor.RangeProperty(debugRangeProp, "Debug Float Custom Slider: ");
        materialEditor.FloatProperty(debugRangeProp, "Debug Float: ");
    }
}

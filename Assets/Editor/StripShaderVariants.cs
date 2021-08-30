using System.Collections.Generic;
using UnityEditor.Build;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

// Strip the normal map keyword from all shader variants. After making a build, normal maps will not appear on materials
// TODO: make the user define the shader keyword variant to strip

public class StripShaderVariants : IPreprocessShaders
{
    // Returns the relative callback order for callbacks. Callbacks with lower values are called before ones with higher values.
    // Useful if you have several scripts to determine which one has priority
    public int callbackOrder { get; }

    public void OnProcessShader(Shader shader, ShaderSnippetData snippet, IList<ShaderCompilerData> shaderCompilerDataEntries)
    {
        for (var i = shaderCompilerDataEntries.Count - 1; i >= 0; --i) {
            // Check if a certain local keyword is enabled on this particular shader variants in the loop
            var shaderLocalKeyword = new ShaderKeyword(shader,"_NORMALMAP");
            if (shaderCompilerDataEntries[i].shaderKeywordSet.IsEnabled(shaderLocalKeyword)) {
                Debug.Log("Shader keyword: _NORMALMAP is removed from Shader: " + shader);
                shaderCompilerDataEntries.RemoveAt(i);
            }
        }
    }
}

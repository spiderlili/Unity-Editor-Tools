using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class StripShaderVariants : IPreprocessShaders
{
    // Returns the relative callback order for callbacks. Callbacks with lower values are called before ones with higher values.
    // Useful if you have several scripts to determine which one has priority
    public int callbackOrder { get; }
}

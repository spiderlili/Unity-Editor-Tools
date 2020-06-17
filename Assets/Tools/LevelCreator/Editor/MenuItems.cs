using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LevelCreator;

//display all the future menu items that the level creator tool requires
public class MenuItems 
{
    [MenuItem("Tools/Level Creator/New Level Scene")]
    [MenuItem("Tools/Level Creator/Show Palette")]
    private static void NewLevel()
    {
        EditorUtilsSceneAutomation.NewLevel();
    }

    private static void ShowPalette()
    {
        PaletteWindow.ShowPalette();
    }
}

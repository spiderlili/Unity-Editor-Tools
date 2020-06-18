using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LevelCreator;

//display all the future menu items that the level creator tool requires
public class MenuItems 
{
    [MenuItem("Tools/Level Creator/New Level Scene")]
    private static void NewLevel()
    {
        EditorUtilsSceneAutomation.NewLevel();
    }

    [MenuItem("Tools/Level Creator/Show Palette _p")]
    private static void ShowPalette()
    {
        PaletteWindow.ShowPalette();
    }
}

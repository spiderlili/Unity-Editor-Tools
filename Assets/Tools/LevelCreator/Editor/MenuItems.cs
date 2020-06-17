using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LevelCreator;

//all the future menu items that the level creator tool requires
public class MenuItems 
{
    [MenuItem("Tools/Level Creator/New Level Scene")]
    private static void NewLevel()
    {
        EditorUtilsSceneAutomation.NewLevel();
    }
}

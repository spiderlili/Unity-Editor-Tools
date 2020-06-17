using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LevelCreator { 
public class PaletteWindow : EditorWindow
{
        public static PaletteWindow instance; //save the reference to the PaletteWindow instance, follows a singleton pattern

        public static void ShowPalette()
        {
            //GetWindow: responsible for getting an instance of the specified type of window (PaletteWindow type)
            //each time ShowPalette() is called: current live window instance will be returned
            instance = (PaletteWindow)EditorWindow.GetWindow(typeof(PaletteWindow));
            instance.titleContent = new GUIContent("Palette");
        }
}
}
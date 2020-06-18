using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace LevelCreator { 
public class PaletteWindow : EditorWindow
{
        public static PaletteWindow instance; //save the reference to the PaletteWindow instance, follows a singleton pattern
        private List<PaletteItem.Category> _categories;
        private List<string> _categoryLabels;
        private PaletteItem.Category _categorySelected;

        public static void ShowPalette()
        {
            //GetWindow: responsible for getting an instance of the specified type of window (PaletteWindow type)
            //each time ShowPalette() is called: current live window instance will be returned
            instance = (PaletteWindow)EditorWindow.GetWindow(typeof(PaletteWindow));
            instance.titleContent = new GUIContent("Palette");
        }

        private void OnEnable()
        {
            if (_categories == null)
            {
                InitCategories();
            }
        }

        private void InitCategories() //get and save categories using a string array => used to set up the labels of the tabs
        {
            Debug.Log("InitCategories called");
            _categories = EditorUtilsSceneAutomation.GetListFromEnum<PaletteItem.Category>();
            _categoryLabels = new List<string>();
            foreach(PaletteItem.Category category in _categories)
            {
                _categoryLabels.Add(category.ToString());
            }
        }

        private void DrawTabs()
        {
            int index = (int)_categorySelected;
            index = GUILayout.Toolbar(index, _categoryLabels.ToArray()); //GUI component = array of buttons
            //depending on the button pressed: return the number representing that button. 
            _categorySelected = _categories[index]; //each time tab pressed: save category to categorySelected 
        }

        private void OnDisable() //called when the behaviour becomes disabled / the object is destroyed
        {
            
        }

        private void OnDestroy() //occurs when a Scene or game ends
        {
            
        }

        private void OnGUI() //for rendering and handling GUI events.
        {
            DrawTabs();
        }

    }
}
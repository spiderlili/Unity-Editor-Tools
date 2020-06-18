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

        private string _searchPfPath = "Assets/Prefabs/LevelPieces"; //where the window will search the prefabs
        private List<PaletteItem> _items;
        private Dictionary<PaletteItem.Category, List<PaletteItem>> _categorizedItems; //lists of PaletteItem instances
        private Dictionary<PaletteItem, Texture2D> _previews;  //previews of the item
        private Vector2 _scrollPosition;
        private const float ButtonWidth = 80;
        private const float buttonHeight = 90;

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
            if(_categorizedItems == null)
            {
                InitContent();
            }
        }

        private void InitContent()
        {
            //set the scrollList
            _items = EditorUtilsSceneAutomation.GetAssetsWithScript<PaletteItem>(_searchPfPath);
            _categorizedItems = new Dictionary<PaletteItem.Category, List<PaletteItem>>();
            _previews = new Dictionary<PaletteItem, Texture2D>();

            //init dictionary
            foreach(PaletteItem.Category category in _categories)
            {
                _categorizedItems.Add(category, new List<PaletteItem>());
            }

            //assign items to each category
            foreach(PaletteItem item in _items)
            {
                _categorizedItems[item.category].Add(item);
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

        private void DrawScroll()
        {
            if (_categorizedItems[_categorySelected].Count == 0)
            {
                EditorGUILayout.HelpBox("This category is empty!", MessageType.Info);
                return;
            }
            int rowCapacity = Mathf.FloorToInt(position.width / (ButtonWidth));
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
//avoid SelectionGrid's toggle behaviour: always clean the index returned, set result to -1 before passing it again to the method
            int selectionGridIndex = -1; 
            selectionGridIndex = GUILayout.SelectionGrid(selectionGridIndex, GetGUIContentsFromItems(), rowCapacity, GetGUIStyle());
            GetSelectedItem(selectionGridIndex);
            GUILayout.EndScrollView();
        }

        private void GeneratePreviews()
        {
            foreach(PaletteItem item in _items)
            {
                if (!_previews.ContainsKey(item))
                {
                    Texture2D preview = AssetPreview.GetAssetPreview(item.gameObject);
                    if(preview != null)
                    {
                        _previews.Add(item, preview);
                    }
                }
            }
        }

        //convert the index returned by SelectionGrid GUI component to a level piece
        private void GetSelectedItem(int index)
        {
            if(index != -1)
            {
                PaletteItem paletteItem = _categorizedItems[_categorySelected][index];
            }
        }

        private GUILayoutOption[] GetGUIStyle()
        {
            throw new NotImplementedException();
        }

        private string[] GetGUIContentsFromItems()
        {
            throw new NotImplementedException();
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
            DrawScroll();
        }

    }
}
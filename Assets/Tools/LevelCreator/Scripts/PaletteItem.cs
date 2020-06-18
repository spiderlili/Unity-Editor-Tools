using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelCreator {
// save information about the level piece prefabs (category). attach this to each level piece prefab, assign properties
    public class PaletteItem : MonoBehaviour
{
#if UNITY_EDITOR //only necessary in the Editor context
        public enum Category
        {
            Misc,
            Collectables,
            Enemies,
            Blocks
        }

     public Category category = Category.Misc; //default
     public string itemName = "";
     //reference to the script that gives the main behavior to the piece. used to access the specific properties of the level piece prefab
     public object inspectedScript; //attach the main script to inspected script
#endif
}
}

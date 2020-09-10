using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyDesignerWindow : EditorWindow
{
    [MenuItem("Tools/Enemy Designer")]
    static void OpenWindow()
    {
        EnemyDesignerWindow window = (EnemyDesignerWindow) GetWindow(typeof(EnemyDesignerWindow));
        window.minSize = new Vector2(600, 300); //make sure the window is not too small
    }
}
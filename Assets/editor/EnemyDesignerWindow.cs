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
        window.Show();
    }

    private void OnEnable() //similar to start() in game scripting
    {

    }

    void InitTextures() //init Texture2D values
    {

    }

    private void OnGUI() //similar to update() in game scripting BUT called >=1 times per interaction - anytime mouse goes over the window/interaction
    {

    }

    void DrawLayouts() //called inside onGUI: Define rect values & paints textures based on rects
    {

    }

    void DrawHeader()
    {

    }

    void DrawMageSettings()
    {

    }

    void DrawWarriorSettings()
    {

    }

    void DrawRogueSettings()
    {

    }
}
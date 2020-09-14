using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyDesignerWindow : EditorWindow
{
    Texture2D headerSectionTexture;
    Texture2D mageSectionTexture;
    Texture2D warriorSectionTexture;
    Texture2D rogueSectionTexture;

    Color headerSectionColor = new Color(13f / 255f, 32f / 255f, 44f / 255f, 1f);

    Rect headerSection;
    Rect mageSection;
    Rect warriorSection;
    Rect rogueSection;

    [MenuItem("Tools/Enemy Designer")]
    static void OpenWindow()
    {
        EnemyDesignerWindow window = (EnemyDesignerWindow) GetWindow(typeof(EnemyDesignerWindow));
        window.minSize = new Vector2(600, 300); //make sure the window is not too small
        window.Show();
    }

    private void OnEnable() //similar to start() in game scripting
    {
        InitTextures(); //define all texture2d values
    }

    void InitTextures() //init Texture2D values
    {
        headerSectionTexture = new Texture2D(1, 1); //define texture2d as 1 color, 1px wide x 1px high without using any image
        headerSectionTexture.SetPixel(0, 0, headerSectionColor);
        headerSectionTexture.Apply();
    }

    private void OnGUI() //similar to update() in game scripting BUT called >=1 times per interaction - anytime mouse goes over the window/interaction
    {
        DrawLayouts();
        DrawHeader();
        DrawMageSettings();
        DrawWarriorSettings();
        DrawRogueSettings();
    }

    //called inside onGUI: Define rect values & paints textures based on rects
    void DrawLayouts()
    {
        headerSection.x = 0;
        headerSection.y = 0;
        headerSection.width = Screen.width;
        headerSection.height = 50;

        GUI.DrawTexture(headerSection, headerSectionTexture);
    }

    void DrawHeader()
    {

    }

    //control the contents of each region: if have different input fields for enemy classes, drawing them within these functions will restrict those fields
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
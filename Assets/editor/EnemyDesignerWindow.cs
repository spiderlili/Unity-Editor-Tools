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

    [MenuItem("Tools/Design Prefab Creator/Enemy Designer")]
    static void OpenWindow()
    {
        EnemyDesignerWindow window = (EnemyDesignerWindow)GetWindow(typeof(EnemyDesignerWindow));
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

        mageSectionTexture = Resources.Load<Texture2D>("icons/enemyDesignerIcons/mage-gradient-bg");
        warriorSectionTexture = Resources.Load<Texture2D>("icons/enemyDesignerIcons/warrior-gradient-bg");
        rogueSectionTexture = Resources.Load<Texture2D>("icons/enemyDesignerIcons/rogue-gradient-bg");
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

        mageSection.x = 0;
        mageSection.y = 50;
        mageSection.width = Screen.width / 3f;
        mageSection.height = Screen.width - 50; //below the header section height

        warriorSection.x = Screen.width / 3f;
        warriorSection.y = 50;
        warriorSection.width = Screen.width / 3f;
        warriorSection.height = Screen.width - 50; //below the header section height

        rogueSection.x = (Screen.width / 3f) * 2;
        rogueSection.y = 50;
        rogueSection.width = Screen.width / 3f;
        rogueSection.height = Screen.width - 50; //below the header section height

        GUI.DrawTexture(headerSection, headerSectionTexture);
        GUI.DrawTexture(mageSection, mageSectionTexture);
        GUI.DrawTexture(warriorSection, warriorSectionTexture);
        GUI.DrawTexture(rogueSection, rogueSectionTexture);
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
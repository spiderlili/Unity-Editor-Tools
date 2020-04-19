using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DnDCharacter;

public class CharacterSelect : EditorWindow
{
    private CharacterClass _characterClass;
    private string _characterName;
    private Gender _gender;

    private int _healthPoints;
    private int _magicPoints;
    private int _strength;
    private int _dexterity;
    private int _constitution; //endurance
    private int _intelligence; //memory and reasoning
    private int _wisdom; //perception and insight
    private int _charisma;

    private FighterSpecialMove1 _fighterSpecialMove1;
    private BardSpecialMove1 _bardSpecialMove1;
    private ClericSpecialMove1 _clericSpecialMove1;
    private WizardSpecialMove1 _wizardSpecialMove1;
    private RangerSpecialMove1 _rangerSpecialMove1;
    private RogueSpecialMove1 _rogueSpecialMove1;

    [MenuItem("Window/DnD Character Select")] //link window to window tab
    private static void OpenWindow() 
    {
        CharacterSelect window = (CharacterSelect)GetWindow(typeof(CharacterSelect));
        window.minSize = new Vector2(600f, 600f);
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Class");
        _characterClass = (CharacterClass)EditorGUILayout.EnumPopup(_characterClass);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        _characterName = EditorGUILayout.TextField("Character Name", _characterName);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Gender");
        _gender = (Gender)EditorGUILayout.EnumPopup(_gender);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Health Points");
        _healthPoints = EditorGUILayout.IntSlider(_healthPoints, 0, 100);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Magic Points");
        _magicPoints = EditorGUILayout.IntSlider(_magicPoints, 0, 100); 
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Strength");
        _strength = EditorGUILayout.IntSlider(_magicPoints, 0, 100); 
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Dexterity");
        _dexterity = EditorGUILayout.IntSlider(_dexterity, 0, 100); 
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Constitution");
        _constitution = EditorGUILayout.IntSlider(_constitution, 0, 100); 
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Intelligence");
        _intelligence = EditorGUILayout.IntSlider(_intelligence, 0, 100); 
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Wisdom");
        _wisdom = EditorGUILayout.IntSlider(_wisdom, 0, 100); 
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Charisma");
        _charisma = EditorGUILayout.IntSlider(_charisma, 0, 100); 
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Special Move 1");
        _fighterSpecialMove1 = (FighterSpecialMove1)EditorGUILayout.EnumPopup(_fighterSpecialMove1);
        EditorGUILayout.EndHorizontal();
    }
}

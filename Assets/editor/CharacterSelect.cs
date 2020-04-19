using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DnDCharacter;

public class CharacterSelect : EditorWindow
{
    private CharacterWindowDefaultSettings _characterWindowSettings;

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

    private Texture2D _currentCharacterImage;
    private Rect _characterImageSection; //size of canvas for portrait
    private Texture2D _characterImageTexture;

    [Multiline]
    private string _characterBio;

    private Color _highValueColor = Color.green;
    private Color _midValueColor = Color.yellow;
    private Color _lowValueColor = Color.red;

    [MenuItem("Window/DnD Character Select")] //link window to window tab
    private static void OpenWindow() 
    {
        CharacterSelect window = (CharacterSelect)GetWindow(typeof(CharacterSelect));
        window.minSize = new Vector2(600f, 600f);
        window.Show();
    }

    private void OnEnable()
    {
        _characterWindowSettings = Resources.Load<CharacterWindowDefaultSettings>("DnD Character Settings Data");
        _highValueColor = _characterWindowSettings.HighValueColor;
        _midValueColor = _characterWindowSettings.MidValueColor;
        _lowValueColor = _characterWindowSettings.LowValueColor;
    }

    private void CheckClass()
    {
        switch (_characterClass)
        {
            case CharacterClass.FIGHTER:
                _currentCharacterImage = Resources.Load<Texture2D>("icons/DnDCharacters/icon_fighter");
            if(_gender == Gender.MALE)
            {

            }
                break;
            case CharacterClass.BARD:
                _currentCharacterImage = Resources.Load<Texture2D>("icons/DnDCharacters/icon_fighter");
                break;

            case CharacterClass.ROGUE:
                _currentCharacterImage = Resources.Load<Texture2D>("icons/DnDCharacters/icon_rogue");
                break;
            
            case CharacterClass.CLERIC:
                _currentCharacterImage = Resources.Load<Texture2D>("icons/DnDCharacters/icon_cleric");
                break;
            
            case CharacterClass.RANGER:
                _currentCharacterImage = Resources.Load<Texture2D>("icons/DnDCharacters/icon_ranger");
                break;
            
            case CharacterClass.WIZARD:
                _currentCharacterImage = Resources.Load<Texture2D>("icons/DnDCharacters/icon_wizard");
                break;
        }

    }

    private void DrawLayouts()
    {
        _characterImageSection.x = Screen.width / 2.4f;
        _characterImageSection.y = 50;
        _characterImageSection.width = 100;
        _characterImageSection.height = 100;
        GUI.DrawTexture(_characterImageSection, _currentCharacterImage);
    }

    private Color GetVariableColor(int value)
    {
        Color currentColor = Color.white;
        if(value >= 66)
        {
            currentColor = _characterWindowSettings.HighValueColor;
        }
        else if(value < 66 && value > 33)
        {
            currentColor = _characterWindowSettings.MidValueColor;
        }
        else if(value <= 33)
        {
            currentColor = _characterWindowSettings.LowValueColor;
        }
        return currentColor;
    }

    private void OnGUI()
    {
        CheckClass();
        DrawLayouts();
        GUILayout.Space(200);

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
        GUI.color = GetVariableColor(_healthPoints);
        _healthPoints = EditorGUILayout.IntSlider(_healthPoints, 0, 100);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Magic Points");
        GUI.color = GetVariableColor(_magicPoints);
        _magicPoints = EditorGUILayout.IntSlider(_magicPoints, 0, 100); 
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Strength");
        GUI.color = GetVariableColor(_strength);
        _strength = EditorGUILayout.IntSlider(_strength, 0, 100); 
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Dexterity");
        GUI.color = GetVariableColor(_dexterity);
        _dexterity = EditorGUILayout.IntSlider(_dexterity, 0, 100); 
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Constitution");
        GUI.color = GetVariableColor(_constitution);
        _constitution = EditorGUILayout.IntSlider(_constitution, 0, 100); 
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Intelligence");
        GUI.color = GetVariableColor(_intelligence);
        _intelligence = EditorGUILayout.IntSlider(_intelligence, 0, 100); 
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Wisdom");
        GUI.color = GetVariableColor(_wisdom);
        _wisdom = EditorGUILayout.IntSlider(_wisdom, 0, 100); 
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Charisma");
        GUI.color = GetVariableColor(_charisma);
        _charisma = EditorGUILayout.IntSlider(_charisma, 0, 100); 
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Character Bio");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        _characterBio = EditorGUILayout.TextArea(_characterBio, GUILayout.Width(position.width-10), GUILayout.Height(60));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Special Move 1");
        _fighterSpecialMove1 = (FighterSpecialMove1)EditorGUILayout.EnumPopup(_fighterSpecialMove1);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        _highValueColor = EditorGUILayout.ColorField("High Value Color", _highValueColor);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        _midValueColor = EditorGUILayout.ColorField("Mid Value Color", _midValueColor);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        _lowValueColor = EditorGUILayout.ColorField("Low Value Color", _lowValueColor);
        EditorGUILayout.EndHorizontal();

    }
}

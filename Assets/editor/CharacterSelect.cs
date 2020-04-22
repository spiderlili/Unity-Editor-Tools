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
    private Vector2 _scroll;

    [Multiline]
    private string _characterBio;

    private CharacterDefaultData _characterDefaultData;
    private CharacterClass _currentClass;

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
        _characterWindowSettings = Resources.Load<CharacterWindowDefaultSettings>("DnDCharacterTemplates/DnD Character Settings Data");
        _highValueColor = _characterWindowSettings.HighValueColor;
        _midValueColor = _characterWindowSettings.MidValueColor;
        _lowValueColor = _characterWindowSettings.LowValueColor;
        CheckData();
    }

    private void CheckHeaderStyle()
    {
        switch (_characterClass)
        {
            case CharacterClass.FIGHTER:
                GUILayout.Label("Fighter", _characterWindowSettings.ClassFonts.GetStyle("Fighter Style"));
                break;
            case CharacterClass.BARD:
                GUILayout.Label("Bard", _characterWindowSettings.ClassFonts.GetStyle("Bard Style"));
                break;

            case CharacterClass.ROGUE:
                GUILayout.Label("Rogue", _characterWindowSettings.ClassFonts.GetStyle("Rogue Style"));
                break;

            case CharacterClass.CLERIC:
                GUILayout.Label("Cleric", _characterWindowSettings.ClassFonts.GetStyle("Cleric Style"));
                break;

            case CharacterClass.RANGER:
                GUILayout.Label("Ranger", _characterWindowSettings.ClassFonts.GetStyle("Ranger Style"));
                break;

            case CharacterClass.WIZARD:
                GUILayout.Label("Wizard", _characterWindowSettings.ClassFonts.GetStyle("Wizard Style"));
                break;
        }
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
                _currentCharacterImage = Resources.Load<Texture2D>("icons/DnDCharacters/icon_bard");
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

    private void CheckData()
    {
        switch (_characterClass)
        {
            case CharacterClass.FIGHTER:
                _characterDefaultData = Resources.Load<CharacterDefaultData>("DnDCharacterTemplates/Character_Fighter_Default");
                _fighterSpecialMove1 = _characterDefaultData._fighterSpecialMove1;
                break;
            case CharacterClass.BARD:
                _characterDefaultData = Resources.Load<CharacterDefaultData>("DnDCharacterTemplates/Character_Bard_Default");
                _bardSpecialMove1 = _characterDefaultData._bardSpecialMove1;
                break;

            case CharacterClass.ROGUE:
                _characterDefaultData = Resources.Load<CharacterDefaultData>("DnDCharacterTemplates/Character_Rogue_Default");
                _rogueSpecialMove1 = _characterDefaultData._rogueSpecialMove1;
                break;

            case CharacterClass.CLERIC:
                _characterDefaultData = Resources.Load<CharacterDefaultData>("DnDCharacterTemplates/Character_Cleric_Default");
                _clericSpecialMove1 = _characterDefaultData._clericSpecialMove1;
                break;

            case CharacterClass.RANGER:
                _characterDefaultData = Resources.Load<CharacterDefaultData>("DnDCharacterTemplates/Character_Ranger_Default");
                _rangerSpecialMove1 = _characterDefaultData._rangerSpecialMove1;
                break;

            case CharacterClass.WIZARD:
                _characterDefaultData = Resources.Load<CharacterDefaultData>("DnDCharacterTemplates/Character_Wizard_Default");
                _wizardSpecialMove1 = _characterDefaultData._wizardSpecialMove1;
                break;
        }
        _characterName = _characterDefaultData._characterName;
        _gender = _characterDefaultData._gender;
        _healthPoints = _characterDefaultData._healthPoints;
        _magicPoints = _characterDefaultData._magicPoints;
        _strength = _characterDefaultData._strength;
        _dexterity = _characterDefaultData._dexterity;
        _constitution = _characterDefaultData._constitution;
        _intelligence = _characterDefaultData._intelligence;
        _wisdom = _characterDefaultData._wisdom;
        _charisma = _characterDefaultData._charisma;
        _characterBio = _characterDefaultData._characterBio;
    }

    private void DrawLayouts()
    {
        _characterImageSection.x = Screen.width / 2.5f;
        _characterImageSection.y = 50;
        _characterImageSection.width = 128;
        _characterImageSection.height = 128;
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

    private void CreateCharacterScriptableObject()
    {
        CharacterDefaultData scrpitableObjectAsset = ScriptableObject.CreateInstance<CharacterDefaultData>();
        scrpitableObjectAsset._characterName = _characterName;
        scrpitableObjectAsset._gender = _gender;
        scrpitableObjectAsset._healthPoints = _healthPoints;
        scrpitableObjectAsset._magicPoints = _magicPoints;
        scrpitableObjectAsset._strength = _strength;
        scrpitableObjectAsset._dexterity = _dexterity;
        scrpitableObjectAsset._constitution = _constitution;
        scrpitableObjectAsset._intelligence = _intelligence;
        scrpitableObjectAsset._wisdom = _wisdom;
        scrpitableObjectAsset._charisma = _charisma;
        scrpitableObjectAsset._characterBio = _characterBio;
        scrpitableObjectAsset._fighterSpecialMove1 = _characterDefaultData._fighterSpecialMove1;
        scrpitableObjectAsset._rangerSpecialMove1 = _characterDefaultData._rangerSpecialMove1;
        scrpitableObjectAsset._bardSpecialMove1 = _characterDefaultData._bardSpecialMove1;
        scrpitableObjectAsset._rogueSpecialMove1 = _characterDefaultData._rogueSpecialMove1;
        scrpitableObjectAsset._wizardSpecialMove1 = _characterDefaultData._wizardSpecialMove1;
        scrpitableObjectAsset._clericSpecialMove1 = _characterDefaultData._clericSpecialMove1;

        AssetDatabase.CreateAsset(scrpitableObjectAsset, "Assets/ScriptableObjects/" + _characterName + ".asset");
        AssetDatabase.SaveAssets();
    }
    private void OnGUI()
    {
        CheckClass();
        DrawLayouts();
        CheckHeaderStyle();
        GUILayout.Space(200);

        //make sure it's only called once
        if (_currentClass != _characterClass)
        {
            _currentClass = _characterClass;
            CheckData();
        }

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
        GUI.color = Color.white;
        GUILayout.Label("Character Bio");
        EditorGUILayout.EndHorizontal();

        _scroll = EditorGUILayout.BeginScrollView(_scroll);
        EditorStyles.textField.wordWrap = true;
        _characterBio = EditorGUILayout.TextArea(_characterBio, GUILayout.Height(60));
        EditorGUILayout.EndScrollView();

        EditorGUILayout.BeginHorizontal();
        switch (_characterClass)
        {
            case CharacterClass.FIGHTER:
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Special Move 1");
                _fighterSpecialMove1 = (FighterSpecialMove1)EditorGUILayout.EnumPopup(_fighterSpecialMove1);
                EditorGUILayout.EndHorizontal();
                break;
            case CharacterClass.BARD:
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Special Move 1");
                _fighterSpecialMove1 = (FighterSpecialMove1)EditorGUILayout.EnumPopup(_bardSpecialMove1);
                EditorGUILayout.EndHorizontal();
                break;

            case CharacterClass.ROGUE:
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Special Move 1");
                _fighterSpecialMove1 = (FighterSpecialMove1)EditorGUILayout.EnumPopup(_rogueSpecialMove1);
                EditorGUILayout.EndHorizontal();
                break;

            case CharacterClass.CLERIC:
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Special Move 1");
                _fighterSpecialMove1 = (FighterSpecialMove1)EditorGUILayout.EnumPopup(_clericSpecialMove1);
                EditorGUILayout.EndHorizontal();
                break;

            case CharacterClass.RANGER:
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Special Move 1");
                _fighterSpecialMove1 = (FighterSpecialMove1)EditorGUILayout.EnumPopup(_rangerSpecialMove1);
                EditorGUILayout.EndHorizontal();
                break;

            case CharacterClass.WIZARD:
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Special Move 1");
                _fighterSpecialMove1 = (FighterSpecialMove1)EditorGUILayout.EnumPopup(_wizardSpecialMove1);
                EditorGUILayout.EndHorizontal();
                break;
        }
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

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Create Character Template"))
        {
            CreateCharacterScriptableObject();
        }
        EditorGUILayout.EndHorizontal();
    }
}

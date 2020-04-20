using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DnDCharacter;

[CreateAssetMenu(fileName = "New DnD Character Data", menuName = "DnD Character Template", order = 50)]
public class CharacterDefaultData : ScriptableObject
{
    public CharacterClass _characterClass;
    public string _characterName;
    public Gender _gender;

    [Range(0, 100)]
    public int _healthPoints;
    [Range(0, 100)]
    public int _magicPoints;
    [Range(0, 100)]
    public int _strength;
    [Range(0, 100)]
    public int _dexterity;
    [Range(0, 100)]
    public int _constitution; //endurance
    [Range(0, 100)]
    public int _intelligence; //memory and reasoning
    [Range(0, 100)]
    public int _wisdom; //perception and insight
    [Range(0, 100)] 
    public int _charisma;

    public FighterSpecialMove1 _fighterSpecialMove1;
    public BardSpecialMove1 _bardSpecialMove1;
    public ClericSpecialMove1 _clericSpecialMove1;
    public WizardSpecialMove1 _wizardSpecialMove1;
    public RangerSpecialMove1 _rangerSpecialMove1;
    public RogueSpecialMove1 _rogueSpecialMove1;

    [Multiline(10)]
    [TextArea(10, 100)]
    public string _characterBio;
}

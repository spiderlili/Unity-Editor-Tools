using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DnDCharacter 
{
    public enum CharacterClass
    { 
        FIGHTER,
        BARD,
        CLERIC,
        RANGER,
        ROGUE,
        WIZARD
    };

    public enum Gender
    { 
        MALE,
        FEMALE,
        NON_BINARY    
    };

    public enum FighterSpecialMove1
    { 
        INDOMITABLE,
        SECOND_WIND
    };

    public enum BardSpecialMove1
    {
        COUNTERCHARM,
        SONG_OF_REST
    };

    public enum ClericSpecialMove1
    {
        SPELLCASTING,
        DESTROY_UNDEAD
    };

    public enum RangerSpecialMove1
    {
        FERAL_SENSES,
        VANISH
    };

    public enum RogueSpecialMove1
    {
        EVASION,
        BLINDSENSE
    };

    public enum WizardSpecialMove1
    {
        ARCANE_TRADITION,
        SPELL_MASTERY
    };
}

using System.Collections;
using System.Collections.Generic;
using EnemyTypes; //derive ability enums
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "New Rogue Data", menuName = "Enemy Character/Rogue")]
public class RogueData : EnemyCharacterData
{
    public RogueWeaponType weaponType;
    public RogueStrategyType strategyType;
}
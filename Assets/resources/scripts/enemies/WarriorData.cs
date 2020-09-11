using System.Collections;
using System.Collections.Generic;
using EnemyTypes; //derive ability enums
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "New Warrior Data", menuName = "Enemy Character/Warrior")]
public class WarriorData : EnemyCharacterData
{
    public WarriorClassType classType;
    public WarriorWeaponType weaponType;
}
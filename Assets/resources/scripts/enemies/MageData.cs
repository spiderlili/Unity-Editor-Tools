using System.Collections;
using System.Collections.Generic;
using EnemyTypes; //derive ability enums
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "New Mage Data", menuName = "Enemy Character/Mage")]
public class MageData : EnemyCharacterData
{
    public MageDamageType damageType;
    public MageWeaponType weaponType;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//generic attributes shared among all enemy characters
public class EnemyCharacterData : ScriptableObject
{
    public GameObject prefab;
    public float maxHealth;
    public float maxEnergy;
    public float criticalChance;
    public float power;
    public string characterName;
}
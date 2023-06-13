using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Space Survivor 2D/CharacterData", order = 0)]
public class CharacterData : ScriptableObject
{
    public int maxHealth = 100;
    public float baseSpeed = 3.5f;
    public float damageResistance = 0f;

}

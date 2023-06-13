using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Space Survivor 2D/WeaponData", order = 0)]
public class WeaponData : ScriptableObject
{

    public Vector2Int baseDamage = new Vector2Int(80, 120);
    public int attackSpeed = 10;
    public float range = 10;

    public float bulletReloadTime = 0.2f;
    public float magazineReloadTime = 1;

    public float criticalChance = 0.03f;
    public float criticalMultiplier = 1.5f;

    public int magazine = 6;
    public int projectiles = 1;
    public float spread = 10f;

    public int pierce = 0;
    public float speedWhileAiming = 0.7f;

    public Weapon weapon;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    internal Vector2Int baseDamage;
    internal int attackSpeed;
    internal float range;

    internal float bulletReloadTime;
    internal float magazineReloadTime;
    internal float criticalChance;    //between 0 and 1
    internal float criticalMultiplier;  //superior to 1

    internal int pierce;
    internal float speed_aimingDemultiplier;
    internal SoundManager soundManager;
    internal Transform aimTransform;

    //Guns
    internal int magazine;
    internal int currentMagazine;
    internal bool reloadingMagazine = false;
    internal PlayerController player;


    public virtual void Setup(Vector2Int baseDamage,
                                int attackSpeed,
                                float range,
                                float bulletReloadTime,
                                float magazineReloadTime,
                                float criticalChance,
                                float criticalMultiplier,
                                int pierce,
                                float speed_aimingDemultiplier,
                                Transform aimTransform,
                                int magazine)
    {
        this.baseDamage = baseDamage;
        this.attackSpeed = attackSpeed;
        this.range = range;
        this.bulletReloadTime = bulletReloadTime;
        this.magazineReloadTime = magazineReloadTime;
        this.criticalChance = criticalChance;
        this.pierce = pierce;
        this.speed_aimingDemultiplier = speed_aimingDemultiplier;
        this.aimTransform = aimTransform;
        this.magazine = magazine;
        soundManager = SoundManager.instance;
        currentMagazine = magazine;
        player = PlayerController.instance;
    }

    public virtual void Shoot() { }

    public virtual void Reload() { }
}

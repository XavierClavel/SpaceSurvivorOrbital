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

    internal virtual void Start()
    {
        baseDamage = PlayerManager.baseDamage;
        attackSpeed = PlayerManager.attackSpeed;
        range = PlayerManager.range;
        bulletReloadTime = PlayerManager.bulletReloadTime;
        magazineReloadTime = PlayerManager.magazineReloadTime;
        criticalChance = PlayerManager.criticalChance;
        pierce = PlayerManager.pierce;
        speed_aimingDemultiplier = PlayerManager.speed_aimingDemultiplier;
        magazine = PlayerManager.magazine;

        soundManager = SoundManager.instance;
        currentMagazine = magazine;
        player = PlayerController.instance;

        aimTransform = player.arrowTransform;

    }

    public virtual void Shoot() { }

    public virtual void Reload() { }
}

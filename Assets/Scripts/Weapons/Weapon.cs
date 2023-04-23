using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected Vector2Int baseDamage;
    protected int attackSpeed;
    protected float range;

    protected float bulletReloadTime;
    protected float magazineReloadTime;
    protected float criticalChance;    //between 0 and 1
    protected float criticalMultiplier;  //superior to 1

    protected int pierce;
    protected float speed_aimingDemultiplier;
    protected SoundManager soundManager;
    protected Transform aimTransform;

    //Guns
    protected int magazine;
    protected int currentMagazine;
    protected bool reloadingMagazine = false;
    protected PlayerController player;

    protected virtual void Start()
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

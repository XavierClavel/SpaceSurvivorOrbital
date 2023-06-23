using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer spriteRenderer;
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
    [HideInInspector] public Slider reloadSlider;

    public bool firing = false;
    protected bool reloading = false;

    public void DisplayWeapon()
    {
        spriteRenderer.enabled = true;
    }

    public void HideWeapon()
    {
        spriteRenderer.enabled = false;
    }

    protected virtual void Start()
    {
        baseDamage = PlayerManager.baseDamage;
        attackSpeed = PlayerManager.attackSpeed;
        range = PlayerManager.range;
        bulletReloadTime = PlayerManager.cooldown;
        magazineReloadTime = PlayerManager.magazineReloadTime;
        criticalChance = PlayerManager.criticalChance;
        pierce = PlayerManager.pierce;
        speed_aimingDemultiplier = PlayerManager.speedWhileAiming;
        magazine = PlayerManager.magazine;

        soundManager = SoundManager.instance;
        currentMagazine = magazine;
        player = PlayerController.instance;

        aimTransform = player.arrowTransform;

    }

    protected abstract void Shoot();

    protected IEnumerator Reload()
    {
        reloading = true;
        yield return new WaitForSeconds(bulletReloadTime);
        reloading = false;
        if (firing) Shoot();
    }

    public virtual void StartFiring()
    {
        firing = true;
        if (!reloading) Shoot();
    }

    public virtual void StopFiring()
    {
        firing = false;
    }
}

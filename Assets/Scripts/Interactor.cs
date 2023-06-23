using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Interactor : MonoBehaviour
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
    protected SoundManager soundManager;

    [HideInInspector] public float speedWhileAiming;

    public bool isUsing = false;
    protected bool reloading = false;
    float cooldown = 0.1f;
    WaitForSeconds waitCooldown;

    //Guns
    protected int magazine;
    protected int currentMagazine;
    protected bool reloadingMagazine = false;

    static LayerMask weaponLayerMask;
    static LayerMask toolLayerMask;
    public LayerMask currentLayerMask;
    protected Transform aimTransform;

    public Slider reloadSlider;

    protected PlayerController player;
    protected bool autoCooldown;

    protected virtual void Start()
    {
        baseDamage = PlayerManager.baseDamage;
        attackSpeed = PlayerManager.attackSpeed;
        range = PlayerManager.range;
        bulletReloadTime = PlayerManager.cooldown;
        magazineReloadTime = PlayerManager.magazineReloadTime;
        criticalChance = PlayerManager.criticalChance;
        pierce = PlayerManager.pierce;
        speedWhileAiming = PlayerManager.speedWhileAiming;
        magazine = PlayerManager.magazine;

        soundManager = SoundManager.instance;
        currentMagazine = magazine;
        player = PlayerController.instance;

        aimTransform = ObjectManager.instance.armTransform;

        waitCooldown = Helpers.GetWait(cooldown);
    }

    public void SwitchMode()
    {
        currentLayerMask = currentLayerMask == weaponLayerMask ? toolLayerMask : weaponLayerMask;
    }

    public virtual void StartUsing()
    {
        isUsing = true;
        if (reloading) return;
        onStartUsing();
        if (cooldown == 0f) return;
        Use();
    }

    public void Use()
    {
        onUse();
        if (cooldown == 0f) return;
        if (autoCooldown) StartCoroutine(nameof(Cooldown));
    }

    public void StopUsing()
    {
        isUsing = false;
        onStopUsing();
    }

    protected IEnumerator Cooldown()
    {
        reloading = true;
        yield return waitCooldown;
        reloading = false;
        if (isUsing) Use();
    }

    protected abstract void onStartUsing();
    protected abstract void onStopUsing();
    protected abstract void onUse();
}

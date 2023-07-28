using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Interactor : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    protected Vector2Int baseDamage;
    protected int attackSpeed;
    protected float range;

    protected float bulletReloadTime;
    protected float magazineReloadTime;
    protected float criticalChance;    //between 0 and 1
    protected float criticalMultiplier;  //superior to 1

    protected int pierce;
    protected int projectiles;
    protected float spread;

    protected SoundManager soundManager;

    [HideInInspector] public float speedWhileAiming;

    [HideInInspector] public bool isUsing = false;
    protected bool reloading = false;
    float cooldown;
    WaitForSeconds waitCooldown;

    //Guns
    protected int magazine;
    protected int currentMagazine;
    protected bool reloadingMagazine = false;

    [HideInInspector] public int dps;

    static LayerMask weaponLayer;
    static LayerMask toolLayerMask;
    [HideInInspector] public string currentLayer;
    protected int currentLayerMask;
    protected Transform aimTransform;

    [HideInInspector] public Slider reloadSlider;

    protected PlayerController player;
    protected bool autoCooldown; //whether the interactor or the inheritor should handle cooldown
    [HideInInspector] public bool playerInteractor;
    bool dualUse = false;

    protected virtual void Start()
    {
        soundManager = SoundManager.instance;
        player = PlayerController.instance;

        aimTransform = ObjectManager.instance.armTransform;
    }

    public void Setup(PlayerData interactorData, bool dualUse = false)
    {
        baseDamage = interactorData.interactor.baseDamage;
        attackSpeed = interactorData.interactor.attackSpeed;
        range = interactorData.interactor.range;
        bulletReloadTime = interactorData.interactor.cooldown;
        magazineReloadTime = interactorData.interactor.magazineReloadTime;
        criticalChance = interactorData.interactor.criticalChance;
        pierce = interactorData.interactor.pierce;
        speedWhileAiming = interactorData.interactor.speedWhileAiming;
        magazine = interactorData.interactor.magazine;
        projectiles = interactorData.interactor.projectiles;
        spread = interactorData.interactor.spread;
        cooldown = interactorData.interactor.cooldown;
        dps = interactorData.interactor.dps;

        waitCooldown = Helpers.GetWait(cooldown);
        currentMagazine = magazine;

        this.dualUse = dualUse;
        if (dualUse)
        {
            currentLayer = Vault.layer.ObstaclesAndEnnemiesAndResources;
            currentLayerMask = LayerMask.GetMask(Vault.layer.Resources, Vault.layer.Ennemies, Vault.layer.Obstacles);
        }
        else
        {
            currentLayer = Vault.layer.ObstaclesAndEnnemies;
            currentLayerMask = LayerMask.GetMask(Vault.layer.Ennemies, Vault.layer.Obstacles);
        }
    }

    public void SwitchMode()
    {
        currentLayer = currentLayer.Switch(Vault.layer.ObstaclesAndEnnemies, Vault.layer.ObstaclesAndResources);
        currentLayerMask = currentLayerMask.Switch(LayerMask.GetMask(Vault.layer.Ennemies, Vault.layer.Obstacles), LayerMask.GetMask(Vault.layer.Resources, Vault.layer.Obstacles));
    }

    public virtual void StartUsing()
    {
        isUsing = true;
        Debug.Log(isUsing);
        Debug.Log(reloading);
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

    private void Update()
    {
        spriteRenderer.flipY = transform.eulerAngles.z > 90 && transform.eulerAngles.z <= 270;
    }
}

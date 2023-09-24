using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Interactor : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;


    protected SoundManager soundManager;


    [HideInInspector] public bool isUsing = false;
    protected bool reloading = false;
    WaitForSeconds waitCooldown;

    //Guns
    protected int currentMagazine;
    protected bool reloadingMagazine = false;

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
    public interactorStats stats;

    protected virtual void Start()
    {
        soundManager = SoundManager.instance;
        player = PlayerController.instance;

        aimTransform = ObjectManager.instance.armTransform;
    }

    public void Setup(interactorStats stats, bool dualUse = false)
    {
        this.stats = stats;

        waitCooldown = Helpers.GetWait(stats.cooldown);
        currentMagazine = stats.magazine;

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
        if (reloading) return;
        onStartUsing();
        if (stats.cooldown == 0f) return;
        Use();

    }

    public void Hit(List<Collider2D> targets, bool individualDamage = false) {
        if (targets.Count == 0) return;

        int damage;
        bool critical;
        status effect = status.none;
        getDamage(out damage, out critical, out effect);

        foreach (Collider2D target in targets)
        {
            if (individualDamage) {
                Hit(target);
            }
            else {
                ObjectManager.dictObjectToHitable[target.gameObject].Hit(damage, player.effect, critical);
            }
        }
    }

    public void Hit(Collider2D target) {
        Hit(target.gameObject);
    }

    public void Hit(GameObject target) {
        int damage;
        bool critical;
        status effect = status.none;
        getDamage(out damage, out critical, out effect);

        ObjectManager.dictObjectToHitable[target].Hit(damage, player.effect, critical);
    }

    private void getDamage(out int damage, out bool critical, out status effect) {
        damage = stats.baseDamage.getRandom();
        critical = Helpers.ProbabilisticBool(stats.criticalChance);
        if (critical) damage = (int)((float)damage * stats.criticalMultiplier);
        effect = player.effect;
    }

    public void Use()
    {
        onUse();
        if (stats.cooldown == 0f) return;
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

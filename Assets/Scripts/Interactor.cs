using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Introduces onStartUsing and onStopUsing events
public abstract class Interactor : Damager
{
    public SpriteRenderer spriteRenderer;
    public HashSet<status> bonusStatuses = new HashSet<status>();

    //Guns
    protected int currentMagazine;
    protected bool reloadingMagazine = false;

    static LayerMask weaponLayer;
    static LayerMask toolLayerMask;
    [HideInInspector] public string currentLayer;
    protected int currentLayerMask;
    [HideInInspector] public Transform aimTransform;

    [HideInInspector] public Slider reloadSlider;

    [HideInInspector] public bool playerInteractor;

    protected override void Start()
    {
        base.Start();
    }

    public virtual bool isDamageAbsorbed()
    {
        return false;
    }

    public override void Setup(PlayerData fullStats)
    {
        base.Setup(fullStats);
        currentMagazine = stats.magazine;

        currentLayer = Vault.layer.ObstaclesAndEnnemiesAndResources;
        currentLayerMask = LayerMask.GetMask(Vault.layer.Resources, Vault.layer.Ennemies, Vault.layer.Obstacles, Vault.layer.EnnemiesOnly);
    }

    public void SwitchMode()
    {
        currentLayer = currentLayer.Switch(Vault.layer.ObstaclesAndEnnemies, Vault.layer.ObstaclesAndResources);
        currentLayerMask = currentLayerMask.Switch(LayerMask.GetMask(Vault.layer.Ennemies, Vault.layer.Obstacles), LayerMask.GetMask(Vault.layer.Resources, Vault.layer.Obstacles));
    }

    public void StartUsing()
    {
        isUsing = true;
        if (reloading) return;
        onStartUsing();
        if (stats.cooldown == 0f) return;
        Use();

    }

    public void StopUsing()
    {
        isUsing = false;
        onStopUsing();
    }

    protected abstract void onStartUsing();
    protected abstract void onStopUsing();

    protected virtual void Update()
    {
        spriteRenderer.flipY = transform.eulerAngles.z is > 90 and <= 270;
    }
}

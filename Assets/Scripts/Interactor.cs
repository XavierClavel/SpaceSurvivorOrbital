using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Introduces onStartUsing and onStopUsing events
public abstract class Interactor : Damager
{
    public SpriteRenderer spriteRenderer;

    //Guns
    protected int currentMagazine;
    protected bool reloadingMagazine = false;

    static LayerMask weaponLayer;
    static LayerMask toolLayerMask;
    [HideInInspector] public string currentLayer;
    protected int currentLayerMask;
    protected Transform aimTransform;

    [HideInInspector] public Slider reloadSlider;

    [HideInInspector] public bool playerInteractor;
    bool dualUse = false;

    protected override void Start()
    {
        base.Start();

        aimTransform = ObjectManager.instance.armTransform;
    }

    public override void Setup(PlayerData fullStats)
    {
        base.Setup(fullStats);
        currentMagazine = stats.magazine;

        currentLayer = Vault.layer.ObstaclesAndEnnemiesAndResources;
        currentLayerMask = LayerMask.GetMask(Vault.layer.Resources, Vault.layer.Ennemies, Vault.layer.Obstacles);
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

    public void StopUsing()
    {
        isUsing = false;
        onStopUsing();
    }

    protected abstract void onStartUsing();
    protected abstract void onStopUsing();

    private void Update()
    {
        spriteRenderer.flipY = transform.eulerAngles.z > 90 && transform.eulerAngles.z <= 270;
    }
}

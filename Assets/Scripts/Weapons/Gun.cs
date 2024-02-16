using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Pool;

public abstract class Gun : Interactor
{
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected Bullet bulletPrefab;
    protected float bulletLifetime;
    WaitForSeconds magazineReloadWindow;
    Tween sliderTween;
    protected ComponentPool<Bullet> pool;
    private bool isReloadingMagazine = false;
    


    protected override void Start()
    {
        base.Start();

        bulletLifetime = stats.range / stats.attackSpeed;
        if (playerInteractor)
        {
            ObjectManager.UpdateBulletsDisplay(stats.magazine);
        }
        magazineReloadWindow = Helpers.getWait(stats.magazineReloadTime);

        autoCooldown = false;
        bulletPrefab.gameObject.layer = LayerMask.NameToLayer(currentLayer);

        pool = new ComponentPool<Bullet>(bulletPrefab);

    }


    protected override void onUse()
    {
        if (currentMagazine == 0 || reloading) return;

        StopReloadingMagazine();
        
        Fire();

        currentMagazine--;
        if (playerInteractor) ObjectManager.UpdateBulletsDisplay(currentMagazine);

        StartCoroutine(currentMagazine == 0 ? nameof(ReloadMagazine) : nameof(Cooldown));
    }

    void StopReloadingMagazine()
    {
        StopCoroutine(nameof(ReloadMagazine));
        isReloadingMagazine = false;
        sliderTween?.Kill();
        if (playerInteractor) reloadSlider.gameObject.SetActive(false);
    }


    protected abstract void Fire();

    protected override void onStartUsing()
    {

    }

    protected override void onStopUsing()
    {
        if (currentMagazine != stats.magazine && !isReloadingMagazine)
        {
            StartCoroutine(nameof(ReloadMagazine));
        }
    }

    IEnumerator ReloadMagazine()
    {
        isReloadingMagazine = true;
        if (playerInteractor)
        {
            reloadSlider.gameObject.SetActive(true);
            reloadSlider.value = 0f;
            sliderTween = reloadSlider.DOValue(1f, stats.magazineReloadTime).SetEase(Ease.Linear);
        }
        yield return magazineReloadWindow;
        isReloadingMagazine = false;
        currentMagazine = stats.magazine;
        if (playerInteractor)
        {
            reloadSlider.gameObject.SetActive(false);
            ObjectManager.UpdateBulletsDisplay(currentMagazine);
        }
        SoundManager.PlaySfx(transform, key: "Gun_Reload");

        if (isUsing) onUse();
    }
}



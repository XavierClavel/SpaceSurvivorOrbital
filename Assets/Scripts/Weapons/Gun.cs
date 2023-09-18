using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public abstract class Gun : Interactor
{
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected Bullet bulletPrefab;
    protected float bulletLifetime;
    LayoutManager bulletsLayoutManager;
    WaitForSeconds magazineReloadWindow;
    Tween sliderTween;


    protected override void Start()
    {
        base.Start();

        bulletLifetime = stats.range / stats.attackSpeed;
        if (playerInteractor)
        {
            bulletsLayoutManager = player.bulletsLayoutManager;
            bulletsLayoutManager.Setup(stats.magazine);
        }
        magazineReloadWindow = Helpers.GetWait(stats.magazineReloadTime);

        autoCooldown = false;
        bulletPrefab.gameObject.layer = LayerMask.NameToLayer(currentLayer);

    }


    protected override void onUse()
    {
        if (currentMagazine == 0) return;

        StopCoroutine(nameof(ReloadMagazine));
        if (sliderTween != null) sliderTween.Kill();
        if (playerInteractor) reloadSlider.gameObject.SetActive(false);

        StartCoroutine(nameof(Cooldown));
        Fire();

        currentMagazine--;
        if (playerInteractor) bulletsLayoutManager.DecreaseAmount();

        if (currentMagazine == 0) StartCoroutine(nameof(ReloadMagazine));
    }


    protected abstract void Fire();

    protected override void onStartUsing()
    {

    }

    protected override void onStopUsing()
    {
        if (currentMagazine != stats.magazine) StartCoroutine(nameof(ReloadMagazine));
    }

    IEnumerator ReloadMagazine()
    {
        if (playerInteractor)
        {
            reloadSlider.gameObject.SetActive(true);
            reloadSlider.value = 0f;
            sliderTween = reloadSlider.DOValue(1f, stats.magazineReloadTime).SetEase(Ease.Linear);
        }
        yield return magazineReloadWindow;
        if (playerInteractor)
        {
            reloadSlider.gameObject.SetActive(false);
            bulletsLayoutManager.SetAmount(stats.magazine);
        }
        currentMagazine = stats.magazine;
        SoundManager.instance.PlaySfx(transform, sfx.reload);

        if (isUsing) onUse();
    }
}



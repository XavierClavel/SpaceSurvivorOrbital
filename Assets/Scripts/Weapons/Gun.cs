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
        bulletLifetime = range / attackSpeed;
        bulletsLayoutManager = player.bulletsLayoutManager;
        bulletsLayoutManager.Setup(magazine);
        magazineReloadWindow = Helpers.GetWait(magazineReloadTime);
        currentMagazine = magazine;

        autoCooldown = false;
        bulletPrefab.gameObject.layer = LayerMask.NameToLayer("ResourcesAndEnnemies");

    }


    protected override void onUse()
    {
        if (currentMagazine == 0) return;

        StopCoroutine(nameof(ReloadMagazine));
        if (sliderTween != null) sliderTween.Kill();
        reloadSlider.gameObject.SetActive(false);

        StartCoroutine(nameof(Cooldown));
        Fire();

        currentMagazine--;
        bulletsLayoutManager.DecreaseAmount();

        if (currentMagazine == 0) StartCoroutine(nameof(ReloadMagazine));
    }


    protected abstract void Fire();

    protected override void onStartUsing()
    {

    }

    protected override void onStopUsing()
    {
        if (currentMagazine != magazine) StartCoroutine(nameof(ReloadMagazine));
    }

    IEnumerator ReloadMagazine()
    {
        reloadSlider.gameObject.SetActive(true);
        reloadSlider.value = 0f;
        sliderTween = reloadSlider.DOValue(1f, magazineReloadTime).SetEase(Ease.Linear);
        yield return magazineReloadWindow;
        reloadSlider.gameObject.SetActive(false);
        bulletsLayoutManager.SetAmount(magazine);
        currentMagazine = magazine;
        SoundManager.instance.PlaySfx(transform, sfx.reload);

        if (isUsing) onUse();
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public abstract class Gun : Weapon
{
    protected float bulletLifetime;
    LayoutManager bulletsLayoutManager;
    WaitForSeconds magazineReloadWindow;
    bool autoReload = true;
    Tween sliderTween;
    [SerializeField] protected Transform firePoint;


    protected override void Start()
    {
        base.Start();
        bulletLifetime = range / attackSpeed;
        bulletsLayoutManager = player.bulletsLayoutManager;
        bulletsLayoutManager.Setup(magazine);
        magazineReloadWindow = Helpers.GetWait(magazineReloadTime);
        currentMagazine = magazine;
    }


    protected override void Shoot()
    {
        if (currentMagazine == 0) return;
        if (autoReload)
        {
            StopCoroutine(nameof(ReloadMagazine));
            if (sliderTween != null) sliderTween.Kill();
            reloadSlider.gameObject.SetActive(false);
        }
        StartCoroutine(nameof(Reload));
        Fire();

        currentMagazine--;
        bulletsLayoutManager.DecreaseAmount();

        if (currentMagazine == 0) StartCoroutine(nameof(ReloadMagazine));
    }

    protected abstract void Fire();

    public override void StopFiring()
    {
        if (autoReload && firing) StartCoroutine(nameof(ReloadMagazine));
        base.StopFiring();
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
    }
}



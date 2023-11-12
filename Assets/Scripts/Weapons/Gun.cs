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
    LayoutManager bulletsLayoutManager;
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
            bulletsLayoutManager = player.bulletsLayoutManager;
            bulletsLayoutManager.Setup(stats.magazine);
        }
        magazineReloadWindow = Helpers.GetWait(stats.magazineReloadTime);

        autoCooldown = false;
        bulletPrefab.gameObject.layer = LayerMask.NameToLayer(currentLayer);

        pool = new ComponentPool<Bullet>(bulletPrefab);

    }


    protected override void onUse()
    {
        Debug.Log("used");
        if (currentMagazine == 0 || reloading) return;

        StopReloadingMagazine();
        
        Fire();

        currentMagazine--;
        if (playerInteractor) bulletsLayoutManager.DecreaseAmount();

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
        Debug.Log("stopped using");
        if (currentMagazine != stats.magazine && !isReloadingMagazine)
        {
            StartCoroutine(nameof(ReloadMagazine));
        }
    }

    IEnumerator ReloadMagazine()
    {
        isReloadingMagazine = true;
        Debug.Log("reload magazine coroutine started");
        if (playerInteractor)
        {
            reloadSlider.gameObject.SetActive(true);
            reloadSlider.value = 0f;
            sliderTween = reloadSlider.DOValue(1f, stats.magazineReloadTime).SetEase(Ease.Linear);
            Debug.Log(reloadSlider.value);
        }
        yield return magazineReloadWindow;
        isReloadingMagazine = false;
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



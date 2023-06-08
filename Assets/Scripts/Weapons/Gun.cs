using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Gun : Weapon
{
    float bulletLifetime;
    LayoutManager bulletsLayoutManager;
    WaitForSeconds magazineReloadWindow;
    Bullet bulletPrefab;
    bool autoReload = true;
    Tween sliderTween;

    int damage;
    bool critical;

    protected override void Start()
    {
        base.Start();
        bulletLifetime = range / attackSpeed;
        bulletsLayoutManager = player.bulletsLayoutManager;
        bulletsLayoutManager.Setup(magazine);
        magazineReloadWindow = Helpers.GetWait(magazineReloadTime);
        bulletPrefab = player.bulletPrefab;
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


        soundManager.PlaySfx(transform, sfx.shoot);
        Bullet bullet = Instantiate(bulletPrefab, transform.position - transform.forward * 6f, aimTransform.rotation);
        bullet.Fire(attackSpeed, bulletLifetime);
        bullet.pierce = pierce;

        damage = Random.Range(baseDamage.x, baseDamage.y + 1);
        critical = Random.Range(0f, 1f) < criticalChance;
        if (critical) damage = (int)((float)damage * criticalMultiplier);

        bullet.damage = damage;
        bullet.critical = critical;
        bullet.effect = player.effect;


        currentMagazine--;
        bulletsLayoutManager.DecreaseAmount();

        if (currentMagazine == 0) StartCoroutine(nameof(ReloadMagazine));
    }

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



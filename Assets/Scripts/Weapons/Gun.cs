using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    float bulletLifetime;
    LayoutManager bulletsLayoutManager;
    WaitForSeconds magazineReloadWindow;
    Bullet bulletPrefab;

    public override void Setup(Vector2Int baseDamage, int attackSpeed, float range, float bulletReloadTime, float magazineReloadTime, float criticalChance, float criticalMultiplier, int pierce, float speed_aimingDemultiplier, Transform aimTransform, int magazine)
    {
        base.Setup(baseDamage, attackSpeed, range, bulletReloadTime, magazineReloadTime, criticalChance, criticalMultiplier, pierce, speed_aimingDemultiplier, aimTransform, magazine);
        bulletLifetime = range / attackSpeed;
        bulletsLayoutManager = player.bulletsLayoutManager;
        bulletsLayoutManager.Setup(magazine);
        magazineReloadWindow = Helpers.GetWait(magazineReloadTime / 6f);
        bulletPrefab = player.bulletPrefab;
        currentMagazine = magazine;
    }


    public override void Shoot()
    {
        if (currentMagazine == 0) return;
        if (reloadingMagazine) return;
        player.StartCoroutine("Reload");
        soundManager.PlaySfx(transform, sfx.shoot);
        Bullet bullet = Instantiate(bulletPrefab, transform.position - transform.forward * 6f, aimTransform.rotation);
        bullet.Fire(attackSpeed, bulletLifetime);
        bullet.pierce = pierce;
        currentMagazine--;
        bulletsLayoutManager.DecreaseAmount();

        if (currentMagazine == 0) StartCoroutine("ReloadMagazine");
    }

    public override void Reload()
    {
        base.Reload();
        if (!reloadingMagazine) StartCoroutine(ReloadMagazine());
    }

    IEnumerator ReloadMagazine()
    {
        reloadingMagazine = true;
        while (currentMagazine < magazine)
        {
            yield return magazineReloadWindow;
            bulletsLayoutManager.IncreaseAmount();
            currentMagazine++;
        }
        reloadingMagazine = false;
        SoundManager.instance.PlaySfx(transform, sfx.reload);
    }
}



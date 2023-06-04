using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    float bulletLifetime;
    LayoutManager bulletsLayoutManager;
    WaitForSeconds magazineReloadWindow;
    Bullet bulletPrefab;
    bool autoReload = true;

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


    public override void Shoot()
    {
        if (currentMagazine == 0) return;
        if (autoReload) StopCoroutine("ReloadMagazine");
        player.StartCoroutine("Reload");


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

        if (currentMagazine == 0) StartCoroutine("ReloadMagazine");
    }
    public override void StartFiring()
    {
        base.StartFiring();
    }

    public override void StopFiring()
    {
        base.StopFiring();
        if (autoReload) StartCoroutine("ReloadMagazine");
    }

    IEnumerator ReloadMagazine()
    {
        yield return magazineReloadWindow;
        bulletsLayoutManager.SetAmount(magazine);
        currentMagazine = magazine;
        SoundManager.instance.PlaySfx(transform, sfx.reload);
    }
}



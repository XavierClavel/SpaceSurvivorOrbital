using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * <pre>
 * <p>IntA -> Mode (0 : normal, 1 : shoots on two sides, 2 : shoots on 4 sides)</p>
 * </pre>
 */
public class Pistol : Gun
{

    [SerializeField] float distanceOffsetBetweenBullets = 0.1f;
    private static bool main = true;
    private Pistol newPistol;
    private List<Pistol> childPistols = new List<Pistol>();
    private List<Bullet> bulletsStack = new List<Bullet>();

    Animator animator;

    private void OnDestroy()
    {
        main = true;
    }

    private Vector3 getBulletStartPos()
    {
        return PlayerController.instance.transform.position + aimTransform.right * 0.5f;
    }

    private Vector3 getBulletStartRotation()
    {
        return transform.localScale.y > 0 ? firePoint.eulerAngles : 180f*Vector3.forward + firePoint.eulerAngles;
    }

    protected override void Start()
    {
        base.Start();
        bulletPrefab.pierce = stats.pierce;
        if (!main) return;
        
        if (fullStats.generic.intA > 0)
        {
            SetupChildGun(player.pointerBack);
        }

        if (fullStats.generic.intA > 1)
        {
            SetupChildGun(player.pointerRight);
            SetupChildGun(player.pointerLeft);
        }
        
        main = false;

    }

    private void SetupChildGun(Transform rotationAxis)
    {
        Pistol newPistol = (Pistol)Instantiate(PlayerManager.weaponPrefab);
        newPistol.Setup(fullStats);
        newPistol.transform.SetParent(rotationAxis);
        newPistol.transform.localPosition = 0.4f * Vector2.right;
        newPistol.transform.localRotation = Quaternion.identity;
        newPistol.aimTransform = rotationAxis;
        
        childPistols.Add(newPistol);
    }

    protected override void Fire()
    {
        childPistols.ForEach( it => it.Fire());

        if (stats.projectiles == 1)
        {
            Vector3 position = getBulletStartPos();

            Vector3 eulerRotation = getBulletStartRotation();

            FireBullet(position, eulerRotation);
            return;
        }

        int sideProjectiles = stats.projectiles / 2;

        if (stats.projectiles % 2 == 1)
        {
            for (int i = -sideProjectiles; i <= sideProjectiles; i++)
            {
                FireBulletByIndex(i);
            }
            return;
        }

        for (int i = -sideProjectiles; i <= sideProjectiles; i++)
        {
            if (i == 0) continue;
            float j = (float)i - Mathf.Sign(i) * 0.5f;

            FireBulletByIndex(j);
        }
        return;
    }

    void FireBulletByIndex(float i)
    {
        Vector3 position = getBulletStartPos() + i * distanceOffsetBetweenBullets * firePoint.right;
        Vector3 eulerRotation = getBulletStartRotation() + i * stats.spread * Vector3.forward;
        
        FireBullet(position, eulerRotation);
    }

    void FireBullet(Vector3 position, Vector3 eulerRotation)
    {
        SoundManager.PlaySfx(transform, key: "Gun_Shoot");
        Bullet bullet = pool.get(position, eulerRotation);
        bulletsStack.Add(bullet);
        StartCoroutine(nameof(ResetTrailDist), bullet);
        HitInfo hitInfo = new HitInfo(stats, stacker.get());
        hitInfo.effect.Add(status.ice);
        bullet.Fire(stats.attackSpeed, hitInfo);
        Invoke(nameof(recallBullet), bulletLifetime);
    }

    private void recallBullet()
    {
        Bullet bullet = bulletsStack.Pop();
        bullet.trail.time = 0f;
        bullet.gameObject.SetActive(false);
        pool.recall(bullet);
    } 
    
    IEnumerator ResetTrailDist(Bullet bullet){
        yield return null;
        bullet.trail.time = 0.2f;
    }
}

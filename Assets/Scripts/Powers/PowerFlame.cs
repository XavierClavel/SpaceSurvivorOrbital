using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerFlame : Power, IEnnemyListener
{
    private static List<Ennemy> ennemyStacker;
    private HashSet<status> elements = new HashSet<status>();
    public ParticleSystem flamePS;
    public ParticleSystem bigFlamePS;
    public ParticleSystem fireRingPS;
    public BoxCollider2D col2d;
    public CircleCollider2D cir2D;
    private bool flameIsActive;
    public Bullet bulletPrefab;
    public Bullet bigBulletPrefab;
    private bool shootInstead;
    private bool shootAfter;
    private bool bigger;
    private bool fireRing;

    public override void onSetup()
    {
        EventManagers.ennemies.registerListener(this);
        
        shootAfter = fullStats.generic.boolA;
        shootInstead = fullStats.generic.boolB;
        bigger = fullStats.generic.boolC;
        fireRing = fullStats.generic.boolD;
        col2d.size = new Vector2(fullStats.generic.intA, fullStats.generic.intB);
        col2d.offset = new Vector2(0, 2 + fullStats.generic.intA);

        ennemyStacker = new List<Ennemy>();
        
        if (shootInstead) StartCoroutine(nameof(Reload));
        else StartCoroutine(nameof(FlameThrower)); 

        if(fireRing) { cir2D.enabled = true; }
        
    }

    private void OnDestroy()
    {
        EventManagers.ennemies.unregisterListener(this);
    }

    void Update()
    {
        gameObject.transform.position = player.transform.position;
        Vector2 aimDirection = player.aimVector;

        if (aimDirection == Vector2.zero) return;
        
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
    private void FixedUpdate()
    {
        DealDamage();
    }
    private void DealDamage()
    {
        if (!flameIsActive) return;
        ennemyStacker.ToArray().ToList().ForEach(it => it.StackDamage(stats.baseDamage.x, elements));
    }
    void Shoot(Bullet prefab)
    {
        gameObject.transform.position = player.transform.position;
        Vector2 aimDirection = player.aimVector;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        
        Bullet bullet = Instantiate(prefab, transform.position, rotation);
        bullet.gameObject.SetActive(true);
        bullet.Fire(stats.speedWhileAiming, new HitInfo(stats));
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Ennemy ennemy = ObjectManager.dictObjectToEnnemy[other.gameObject];
        ennemyStacker.Add(ennemy);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (ennemyStacker.All(it => it.gameObject != other.gameObject)) return;
        Ennemy ennemy = ObjectManager.dictObjectToEnnemy[other.gameObject];
        ennemyStacker.Remove(ennemy);
    }

    IEnumerator FlameThrower()
    {
        while (true)
        {
            flameIsActive = true;
            if (bigger) bigFlamePS.Play();
            else flamePS.Play();
            if(fireRing) fireRingPS.Play();
            yield return new WaitForSeconds(stats.attackSpeed);
            flameIsActive = false;
            flamePS.Stop();
            bigFlamePS.Stop();
            fireRingPS.Stop();
            if (shootAfter) Shoot(bulletPrefab);
            yield return new WaitForSeconds(stats.cooldown);
        }
    }
    IEnumerator Reload()
    {
        while (true)
        {
            yield return Helpers.getWait(stats.cooldown);
            if (bigger) Shoot(bigBulletPrefab);
            else Shoot(bulletPrefab);
        }
    }

    public void onEnnemyDeath(Ennemy ennemy)
    {
        if (ennemyStacker.Contains(ennemy)) ennemyStacker.Remove(ennemy);
    }
}

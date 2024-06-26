using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerFlame : Power
{
    private static Stacker<Ennemy> ennemyStacker;
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
        shootAfter = fullStats.generic.boolA;
        shootInstead = fullStats.generic.boolB;
        bigger = fullStats.generic.boolC;
        fireRing = fullStats.generic.boolD;
        col2d.size = new Vector2(fullStats.generic.intA, fullStats.generic.intB);
        col2d.offset = new Vector2(0, 2 + fullStats.generic.intA);

        ennemyStacker = new Stacker<Ennemy>();
        
        if(!shootInstead) {StartCoroutine(nameof(FlameThrower));} 
        else if (shootInstead) {StartCoroutine(nameof(Reload));}

        if(fireRing) { cir2D.enabled = true; }
        
    }

    void Update()
    {
        gameObject.transform.position = player.transform.position;
        Vector2 aimDirection = player.aimVector;

        if (aimDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
    private void FixedUpdate()
    {
        DealDamage();
    }
    private void DealDamage()
    {
        if(flameIsActive)
        {
            foreach (Ennemy ennemy in ennemyStacker.get())
            {
                ennemy.StackDamage(stats.baseDamage.x, elements);
            }
        }
    }
    void Shoot()
    {
        gameObject.transform.position = player.transform.position;
        Vector2 aimDirection = player.aimVector;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        
        Bullet bullet = Instantiate(bulletPrefab, transform.position, rotation);
        bullet.gameObject.SetActive(true);
        bullet.Fire(stats.speedWhileAiming, new HitInfo(stats));
    }

    void BiggerShoot()
    {
        gameObject.transform.position = player.transform.position;
        Vector2 aimDirection = player.aimVector;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        
        Bullet bullet = Instantiate(bigBulletPrefab, transform.position, rotation);
        bullet.gameObject.SetActive(true);
        bullet.Fire(stats.speedWhileAiming, new HitInfo(stats));
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Ennemy ennemy = ObjectManager.dictObjectToEnnemy[other.gameObject];
        ennemyStacker.stack(ennemy);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Ennemy ennemy = ObjectManager.dictObjectToEnnemy[other.gameObject];
        ennemyStacker.unstack(ennemy);
    }

    IEnumerator FlameThrower()
    {
        while (true)
        {
            flameIsActive = true;
            if(!bigger) {flamePS.Play();}
            else if (bigger) { bigFlamePS.Play();}
            if(fireRing) { fireRingPS.Play();};
            yield return new WaitForSeconds(stats.attackSpeed);
            flameIsActive = false;
            flamePS.Stop();
            bigFlamePS.Stop();
            fireRingPS.Stop();
            if (shootAfter) { Shoot(); }
            yield return new WaitForSeconds(stats.cooldown);
        }
    }
    IEnumerator Reload()
    {
        while (true)
        {
            yield return Helpers.getWait(stats.cooldown);
            if (!bigger) { Shoot(); }
            else if (bigger) { BiggerShoot(); }
        }
    }
}

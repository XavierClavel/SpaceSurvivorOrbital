using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum status { none, fire, ice, lightning }

public class Bullet : MonoBehaviour
{
    private const int damageScalePlayerToEnnemy = 20;
    [SerializeField] ParticleSystem bulletParticle;
    [SerializeField] Rigidbody2D rb;
    public TrailRenderer trail;
    [HideInInspector] public int pierce = 0;
    int currentPierce = 0;
    private HitInfo hitInfo;


    public void Fire(int speed, HitInfo hitInfo)
    {
        rb.velocity = transform.up * speed;
        this.hitInfo = hitInfo;
    }

    public void Fire(int speed, float lifetime, int damage)
    {
        Destroy(gameObject, lifetime);
        rb.velocity = transform.up * 10f;
        hitInfo = new HitInfo(damage, false, status.none);
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Transparent")) return;
        
        SoundManager.PlaySfx(transform, Vault.sfx.BulletHit);
        Helpers.SpawnPS(transform, bulletParticle);


        if (other.gameObject.layer == LayerMask.NameToLayer(Vault.layer.Obstacles)) gameObject.SetActive(false);


        if (other.gameObject.CompareTag(Vault.tag.Player))
        {
            PlayerController.Hurt(hitInfo.damage);
            if (PlayerController.instance.reflectsProjectiles)
            {
                gameObject.layer = LayerMask.NameToLayer(Vault.layer.ObstaclesAndEnnemiesAndResources);
                rb.velocity *= -1f;
                hitInfo = new HitInfo(hitInfo.damage * damageScalePlayerToEnnemy, hitInfo.critical, hitInfo.effect);
                return;
            }
        }

        if (other.gameObject.CompareTag(Vault.tag.Ennemy) || other.gameObject.CompareTag(Vault.tag.Resource))
        {
            ObjectManager.dictObjectToHitable[other.gameObject].Hit(hitInfo);
        }


        if (currentPierce == pierce)
        {
            gameObject.SetActive(false);
        }


        currentPierce++;
    }

    public void FireFairy(int speed, float lifetime, int pierce, Transform newTarget, HitInfo hitInfo)
    {
        Destroy(gameObject, lifetime);
        Vector3 direction = (newTarget.position - transform.position).normalized;
        rb.velocity = direction * speed;
        this.hitInfo = hitInfo;
        this.pierce = pierce;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum status { none, poison, fire, ice, lightning }

public class Bullet : MonoBehaviour
{
    [SerializeField] ParticleSystem bulletParticle;
    [SerializeField] Rigidbody2D rb;
    [HideInInspector] public int pierce = 0;
    int currentPierce = 0;
    private HitInfo hitInfo;


    public void Fire(int speed, float lifetime, HitInfo hitInfo)
    {
        Destroy(gameObject, lifetime);
        rb.velocity = transform.up * 10f;
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
        SoundManager.instance.PlaySfx(transform, sfx.bulletOnGround);
        Helpers.SpawnPS(transform, bulletParticle);


        if (other.gameObject.layer == LayerMask.NameToLayer(Vault.layer.Obstacles)) Destroy(gameObject);


        if (other.gameObject.CompareTag(Vault.tag.Player)) PlayerController.Hurt(hitInfo.damage);

        if (other.gameObject.CompareTag(Vault.tag.Ennemy) || other.gameObject.CompareTag(Vault.tag.Resource))
        {
            ObjectManager.dictObjectToHitable[other.gameObject].Hit(hitInfo);
        }


        if (currentPierce == pierce)
        {
            Destroy(gameObject);
        }


        currentPierce++;
    }

    public void FireFairy(int speed, float lifetime, Transform newTarget, HitInfo hitInfo)
    {
        Destroy(gameObject, lifetime);
        Vector3 direction = (newTarget.position - transform.position).normalized;
        rb.velocity = direction * speed;
        this.hitInfo = hitInfo;
    }

}

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
    public int damage = 5;
    public status effect = status.none;
    public bool critical = false;


    public void Fire(int speed, float lifetime)
    {
        Destroy(gameObject, lifetime);
        rb.velocity = transform.up * 10f;
    }

    public void Fire(int speed, float lifetime, int damage)
    {
        Destroy(gameObject, lifetime);
        rb.velocity = transform.up * 10f;
        this.damage = damage;
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        SoundManager.instance.PlaySfx(transform, sfx.bulletOnGround);
        Helpers.SpawnPS(transform, bulletParticle);


        if (other.gameObject.layer == LayerMask.NameToLayer(Vault.layer.Obstacles)) Destroy(gameObject);


        if (other.gameObject.CompareTag(Vault.tag.Player)) PlayerController.Hurt(damage);

        if (other.gameObject.CompareTag(Vault.tag.Ennemy) || other.gameObject.CompareTag(Vault.tag.Resource))
        {
            InteractorHandler.playerInteractorHandler.currentInteractor.Hit(other.gameObject);
        }


        if (currentPierce == pierce)
        {
            Destroy(gameObject);
        }


        currentPierce++;
    }

    public void FireFairy(int speed, float lifetime, Transform newTarget, int damage)
    {
        Destroy(gameObject, lifetime);
        Vector3 direction = (newTarget.position - transform.position).normalized;
        rb.velocity = direction * speed;
        this.damage = damage;
    }

}

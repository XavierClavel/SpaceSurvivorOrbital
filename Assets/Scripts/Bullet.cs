using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum status { none, poison, fire, ice, magic }

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
        StartCoroutine(DestroyTimer(lifetime));
        rb.velocity = transform.up * 10f;
    }

    public void Fire(int speed, float lifetime, int damage)
    {
        StartCoroutine(DestroyTimer(lifetime));
        rb.velocity = transform.up * 10f;
        this.damage = damage;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        SoundManager.instance.PlaySfx(transform, sfx.bulletOnGround);
        Helpers.SpawnPS(transform, bulletParticle);


        if (other.gameObject.layer == LayerMask.NameToLayer(Vault.layer.Obstacles)) Destroy(gameObject);


        if (other.gameObject.CompareTag(Vault.tag.Player)) PlayerController.Hurt(damage);

        if (other.gameObject.CompareTag(Vault.tag.Ennemy) || other.gameObject.CompareTag(Vault.tag.Resource)) {
            InteractorHandler.playerInteractorHandler.Hit(other.gameObject);
        }


        if (currentPierce == pierce)
        {
            Destroy(gameObject);
        }


        currentPierce++;
    }

    IEnumerator DestroyTimer(float lifetime)  //Destroys bullet after lifetime
    {
        yield return Helpers.GetWait(lifetime);
        Destroy(gameObject);
    }
}

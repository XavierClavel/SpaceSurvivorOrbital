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
        ParticleSystem parSys = Instantiate(bulletParticle, transform.position, Quaternion.identity);
        parSys.Play();
        Helpers.instance.WaitAndKill(0.5f, parSys.gameObject);

        if (other.gameObject.CompareTag("Player")) PlayerController.Hurt(damage);

        if (other.gameObject.CompareTag("Ennemy")) ObjectManager.dictObjectToBreakable[other.gameObject].Hit(damage, effect, critical);

        if (other.gameObject.CompareTag("Resource")) ObjectManager.dictObjectToBreakable[other.gameObject].Hit(damage, effect, critical);


        if (currentPierce == pierce)
        {
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Obstacle")) Destroy(gameObject);
        currentPierce++;
    }

    IEnumerator DestroyTimer(float lifetime)  //Destroys bullet after lifetime
    {
        yield return Helpers.GetWait(lifetime);
        Destroy(gameObject);
    }
}

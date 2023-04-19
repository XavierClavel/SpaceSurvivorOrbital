using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] ParticleSystem bulletParticle;
    [SerializeField] Rigidbody2D rb;
    [HideInInspector] public int pierce = 0;
    int currentPierce = 0;
    int damage = 5;


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


        if (currentPierce == pierce)
        {
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Resource")) Destroy(gameObject);
        currentPierce++;
    }

    IEnumerator DestroyTimer(float lifetime)  //Destroys bullet after 10 seconds
    {
        yield return Helpers.GetWait(lifetime);
        Destroy(gameObject);
    }
}

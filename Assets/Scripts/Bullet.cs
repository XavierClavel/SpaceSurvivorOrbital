using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public Vector3 planetPos;
    [SerializeField] ParticleSystem bulletParticle;
    [SerializeField] Rigidbody2D rb;
    [HideInInspector] public Vector3 axis;
    [HideInInspector] public float radius;
    [Header("Properties")]
    public int pierce = 0;
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

    void OnCollisionEnter2D(Collision2D other)
    {
        SoundManager.instance.PlaySfx(transform, sfx.bulletOnGround);
        ParticleSystem parSys = Instantiate(bulletParticle, transform.position, Quaternion.LookRotation(other.GetContact(0).normal, transform.up));
        parSys.Play();
        Helpers.instance.WaitAndKill(0.5f, parSys.gameObject);

        if (other.gameObject.CompareTag("Player")) PlayerController.Hurt(damage);

        if (currentPierce == pierce)
        {
            Destroy(gameObject);
        }
        else currentPierce++;
    }

    IEnumerator DestroyTimer(float lifetime)  //Destroys bullet after 10 seconds
    {
        yield return Helpers.GetWait(lifetime);
        Destroy(gameObject);
    }
}

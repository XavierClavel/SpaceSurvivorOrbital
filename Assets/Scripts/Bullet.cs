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
    public float rotationSpeed = 80.0f;
    public float lifetime = 10f;
    public int pierce = 0;
    int currentPierce = 0;

    void Start()
    {
        StartCoroutine(DestroyTimer());
        rb.velocity = transform.up * 40f;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        SoundManager.instance.PlaySfx(transform, sfx.bulletOnGround);
        ParticleSystem parSys = Instantiate(bulletParticle, transform.position, Quaternion.LookRotation(other.GetContact(0).normal, transform.up));
        parSys.Play();
        Helpers.instance.WaitAndKill(0.5f, parSys.gameObject);

        if (currentPierce == pierce)
        {
            Destroy(gameObject);
        }
        else currentPierce++;
    }

    IEnumerator DestroyTimer()  //Destroys bullet after 10 seconds
    {
        yield return Helpers.GetWait(lifetime);
        Destroy(gameObject);
    }
}

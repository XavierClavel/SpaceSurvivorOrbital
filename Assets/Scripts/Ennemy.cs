using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ennemy : MonoBehaviour
{

    [SerializeField] GameObject bulletPrefab;
    PlayerController player;
    Planet planet;
    Vector3 planetPos;
    float radius;
    [SerializeField] float mean = 4f;
    [SerializeField] float standardDeviation = 1f;
    SoundManager soundManager;
    float health = 2f;
    [SerializeField] Rigidbody2D rb;
    const float hurtWindow = 0.5f;
    float speed = 4f;
    Vector3 distance;
    Vector2 projectedDistance;

    [Header("Parameters")]
    float baseDamage = 5f; //5f


    void Start()
    {
        soundManager = SoundManager.instance;
        player = PlayerController.instance;
        StressTest.nbEnnemies++;
    }

    private void FixedUpdate()
    {
        distance = player.transform.position - transform.position;
        projectedDistance = distance.normalized;
        rb.MovePosition(rb.position + projectedDistance * Time.fixedDeltaTime * speed);
    }

    void Shoot()
    {
        //soundManager.PlaySfx(transform, sfx.ennemyShoots);
        Bullet bullet = Instantiate(bulletPrefab, transform.position + transform.forward, transform.rotation).GetComponentInChildren<Bullet>();
        bullet.axis = transform.right;
        bullet.planetPos = planetPos;
        bullet.radius = radius;
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("BlueBullet"))
        {
            health -= PlayerController.HurtEnnemy();
            if (health <= 0) Death();
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            if (PlayerController.instance.hasWon) return;
            PlayerController.instance.Hurt(baseDamage);
            StartCoroutine("Hurt");
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player")) StopCoroutine("Hurt");
    }

    IEnumerator Hurt()
    {
        WaitForSeconds wait = Helpers.GetWait(hurtWindow);
        while (true)
        {
            yield return wait;
            PlayerController.instance.Hurt(baseDamage);
        }
    }

    void Death()
    {
        StressTest.nbEnnemies--;
        soundManager.PlaySfx(transform, sfx.ennemyExplosion);
        Destroy(gameObject);
    }


}

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
    [SerializeField] Slider healthBar;
    SoundManager soundManager;
    int _health = 150;
    public int health
    {
        get { return _health; }
        set
        {
            _health = value;
            healthBar.value = value;
            if (!healthBar.gameObject.activeInHierarchy) healthBar.gameObject.SetActive(true);
            //SoundManager.instance.PlaySfx(transform, sfx.playerHit);
            if (value <= 0) Death();
        }
    }
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
        healthBar.maxValue = _health;
        healthBar.value = _health;
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
            int damageTaken = 0;
            bool critical = false;
            PlayerController.HurtEnnemy(ref damageTaken, ref critical);
            DamageDisplayHandler.DisplayDamage(damageTaken, other.transform.position, critical);
            health -= damageTaken;
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

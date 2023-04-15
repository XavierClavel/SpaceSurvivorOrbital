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
    [SerializeField] Rigidbody rb;
    Transform cameraTransform;
    [SerializeField] Transform spriteTransform;
    [SerializeField] Image sprite;
    const float hurtWindow = 0.5f;
    float speed = 4f;
    Vector3 distance;
    Vector3 up;
    Vector3 projectedDistance;
    Vector3 correctedUp;
    float dotProduct;

    [Header("Parameters")]
    float baseDamage = 0f; //5f


    void Start()
    {
        soundManager = SoundManager.instance;
        player = PlayerController.instance;
        Initialize(Planet.instance);
        cameraTransform = Camera.main.transform;

        StressTest.nbEnnemies++;
    }

    private void FixedUpdate()
    {
        distance = player.transform.position - transform.position;
        up = (transform.position - planetPos).normalized;
        projectedDistance = Vector3.ProjectOnPlane(distance, up).normalized;
        correctedUp = up;
        dotProduct = Vector3.Dot(projectedDistance, player.localTransform.forward);
        if (dotProduct > 0)
        {
            //dotProduct = Mathf.Pow(dotProduct, 2);
            correctedUp = Vector3.Slerp(up, cameraTransform.up, dotProduct);
            //sprite.color = Color.Lerp(Color.white, Color.red, dotProduct);
        }
        //else sprite.color = Color.white;
        transform.rotation = Quaternion.LookRotation(projectedDistance, up);
        spriteTransform.LookAt(cameraTransform, correctedUp);
        rb.MovePosition(rb.position + projectedDistance * Time.fixedDeltaTime * speed);

        rb.AddForce(-9.81f * up);
    }

    void Shoot()
    {
        //soundManager.PlaySfx(transform, sfx.ennemyShoots);
        Bullet bullet = Instantiate(bulletPrefab, transform.position + transform.forward, transform.rotation).GetComponentInChildren<Bullet>();
        bullet.axis = transform.right;
        bullet.planetPos = planetPos;
        bullet.radius = radius;
    }

    public void Initialize(Planet basePlanet)
    {
        planet = basePlanet;
        planetPos = planet.position;
        radius = (planetPos - transform.position).magnitude;
    }

    private void OnCollisionEnter(Collision other)
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

    private void OnCollisionExit(Collision other)
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

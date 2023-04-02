using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

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
    float health = 3f;
    Rigidbody rb;
    Transform cameraTransform;
    [SerializeField] Transform spriteTransform;
    const float hurtWindow = 0.5f;


    void Start()
    {
        soundManager = SoundManager.instance;
        player = PlayerController.instance;
        rb = GetComponent<Rigidbody>();
        FollowPlayer();
        Initialize(Planet.instance);
        cameraTransform = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        Vector3 distance = player.transform.position - transform.position;
        Vector3 up = (transform.position - planetPos).normalized;
        Vector3 projectedDistance = Vector3.ProjectOnPlane(distance, up).normalized;
        transform.rotation = Quaternion.LookRotation(projectedDistance, up);
        spriteTransform.LookAt(cameraTransform, cameraTransform.up);
        rb.MovePosition(rb.position + projectedDistance * Time.fixedDeltaTime * 2f);

        rb.AddForce(-9.81f * up);
    }

    void Shoot()
    {
        soundManager.PlaySfx(transform, sfx.ennemyShoots);
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

    IEnumerator WaitAndShoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(mean + Random.Range(-standardDeviation, standardDeviation));
            //Shoot();
        }
    }

    public void FollowPlayer()
    {
        StartCoroutine("WaitAndShoot");
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("BlueBullet"))
        {
            health -= 1f;
            if (health <= 0) Death();
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            PlayerController.instance.health -= 1;
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
            PlayerController.instance.health -= 1;
        }
    }

    void Death()
    {
        soundManager.PlaySfx(transform, sfx.ennemyExplosion);
        Destroy(gameObject);
    }


}

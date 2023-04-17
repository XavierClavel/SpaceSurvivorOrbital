using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Runner : Ennemy
{
    //[SerializeField] float mean = 4f;
    //[SerializeField] float standardDeviation = 1f;
    const float hurtWindow = 0.5f;

    Vector3 distance;
    Vector2 projectedDistance;

    [SerializeField] float baseDamage = 5f;

    [Header("Additional References")]
    [SerializeField] GameObject bulletPrefab;


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

    }


    override internal void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);
        if (other.gameObject.CompareTag("Player"))
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


}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public static Dictionary<GameObject, Ghost> dictGoToGhost = new Dictionary<GameObject, Ghost>();
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D col;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] ParticleSystem explosion;
    
    private bool explodeOnBullet = false;
    private bool explodeOnEnnemy = false;
    private float speed;

    private bool canBeDestroyedByLaser = false;
    private float laserWaitBeforeHit = 0.3f;
    private float currentLaserCooldown = 0f;
    private static readonly int animReset = Animator.StringToHash("Reset");
    private static readonly int animExplode = Animator.StringToHash("Explode");
    private bool isBig;

    public Ghost Setup(PlayerData _)
    {
        explodeOnBullet = _.generic.boolC;
        explodeOnEnnemy = _.generic.boolB;
        speed = _.character.baseSpeed;

        dictGoToGhost[gameObject] = this;
        StartCoroutine(nameof(DestroyByWait));
        
        currentLaserCooldown = laserWaitBeforeHit;

        if (DataSelector.selectedWeapon == "Laser") StartCoroutine(nameof(WaitLaser));
        
        animator.SetTrigger(animReset);
        col.enabled = true;
        if (speed > 0) StartCoroutine(nameof(Move));

        return this;
    }

    public void setBig()
    {
        isBig = true;
    }

    private IEnumerator WaitLaser()
    {
        currentLaserCooldown = laserWaitBeforeHit;
        while (currentLaserCooldown > 0)
        {
            yield return Helpers.getWaitFixed();
            currentLaserCooldown -= Time.fixedDeltaTime;
        }
        canBeDestroyedByLaser = true;
    }

    private void OnDestroy()
    {
        dictGoToGhost.TryRemove(gameObject);
    }

    private IEnumerator DestroyByWait()
    {
        yield return Helpers.getWait(5f);
        Explode();
    }
    
    private void DestroyByCollision()
    {
        col.enabled = false;
        StopCoroutine(nameof(DestroyByWait));
        Explode();
    }

    private void Explode()
    {
        explosion.Play();
        PlayerController.Shake(1f, 0.2f);
        animator.SetTrigger(animExplode);
        SoundManager.PlaySfx(transform, key: "Ghost_Explode");
        var position = transform.position;
        PowerGhost.SpawnShockwave(position, isBig);
        PowerGhost.SpawnProjectiles(position);
        Invoke(nameof(Recall),0.5f);
    }

    private void Recall()
    {
        PowerGhost.recallGhost(this);
    }
    

    void OnTriggerEnter2D(Collider2D other)
    {
        if (explodeOnBullet && other.CompareTag("Bullet"))
        {
            DestroyByCollision();
            return;
        }

        if (explodeOnEnnemy && other.gameObject.layer == LayerMask.NameToLayer(Vault.layer.Ennemies))
        {
            DestroyByCollision();
            return;
        }
        
    }
    
    public void HitByLaser()
    {
        if (!explodeOnBullet) return;
        currentLaserCooldown = laserWaitBeforeHit;
        if (!canBeDestroyedByLaser) return;
        DestroyByCollision();
    }
    
    private IEnumerator Move()
    {
        while (true)
        {
            Vector2 distanceToPlayer = PlayerController.instance.transform.position - transform.position;
            Vector2 directionToPlayer = distanceToPlayer.normalized;
            rb.AddForce(directionToPlayer * speed);
            yield return Helpers.getWaitFixed();
        }
    }
}

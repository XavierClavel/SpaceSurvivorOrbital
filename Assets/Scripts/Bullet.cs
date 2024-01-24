using System.Collections;
using System.Collections.Generic;
using Microsoft.Win32.SafeHandles;
using UnityEngine;
using UnityEngine.Events;

public enum status { none, fire, ice, lightning }


public class Bullet : MonoBehaviour
{
    private const int damageScalePlayerToEnnemy = 20;
    [SerializeField] ParticleSystem impact;
    [SerializeField] ParticleSystem explosion;
    public Rigidbody2D rb;
    public TrailRenderer trail;
    [HideInInspector] public int pierce = 0;
    int currentPierce = 0;
    private HitInfo hitInfo;
    private UnityAction<Bullet> onImpact = null;
    private ComponentPool<Bullet> pool = null;

    [SerializeField] Animator hit;

    public Bullet setImpactAction(UnityAction<Bullet> action)
    {
        this.onImpact = action;
        return this;
    }

    public Bullet setTimer(float timer)
    {
        StartCoroutine(nameof(RecallTimer), timer);
        return this;
    }

    private IEnumerator RecallTimer(float timer)
    {
        yield return Helpers.getWait(timer);
        recallBullet();
    }

    private void recallBullet()
    {
        StopCoroutine(nameof(RecallTimer));
        pool?.recall(this);
    }


    public Bullet Fire(float speed, HitInfo hitInfo)
    {
        currentPierce = 0;
        rb.velocity = transform.up * speed;
        this.hitInfo = hitInfo;
        return this;
    }

    public void Fire(int speed, float lifetime, int damage)
    {
        Destroy(gameObject, lifetime);
        rb.velocity = transform.up * 10f;
        hitInfo = new HitInfo(damage, false, status.none);
    }
    
    public Bullet setPool(ComponentPool<Bullet> pool)
    {
        this.pool = pool;
        return this;
    }
    

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Transparent")) return;
        
        Helpers.SpawnPS(transform, impact);


        if (other.gameObject.layer == LayerMask.NameToLayer(Vault.layer.Obstacles)) gameObject.SetActive(false);


        if (other.gameObject.CompareTag(Vault.tag.Player))
        {
            PlayerController.Hurt(hitInfo.damage);
            if (PlayerController.instance.reflectsProjectiles)
            {
                gameObject.layer = LayerMask.NameToLayer(Vault.layer.ObstaclesAndEnnemiesAndResources);
                rb.velocity *= -1f;
                hitInfo = new HitInfo(hitInfo.damage * damageScalePlayerToEnnemy, hitInfo.critical, hitInfo.effect);
                return;
            }
        }
        
        if (other.gameObject.CompareTag(Vault.tag.Ennemy))
        {
            SoundManager.PlaySfx(transform, key: "Ennemy_Hit");
            ObjectManager.HitObject(other.gameObject, hitInfo);
            onImpact?.Invoke(this);
        }

        if (other.gameObject.CompareTag(Vault.tag.Resource))
        {
            ObjectManager.HitObject(other.gameObject, hitInfo);
        }
        
        if (other.gameObject.CompareTag("Stele"))
        {
            ObjectManager.HitObject(other.gameObject, hitInfo);
        }


        if (currentPierce == pierce)
        {
            recallBullet();
            gameObject.SetActive(false);
        }


        currentPierce++;
    }

    public void FireFairy(float speed, float lifetime, int pierce, Transform newTarget, HitInfo hitInfo)
    {
        Destroy(gameObject, lifetime);
        Vector3 direction = (newTarget.position - transform.position).normalized;
        rb.velocity = direction * speed;
        this.hitInfo = hitInfo;
        this.pierce = pierce;
    }

}

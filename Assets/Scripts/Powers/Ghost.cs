using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public static Dictionary<GameObject, Ghost> dictGoToGhost = new Dictionary<GameObject, Ghost>();
    [SerializeField] private Animator animator;
    [SerializeField] private Shockwave shockwave;
    [SerializeField] private Collider2D col;

    private bool isShockwaveEnabled;
    private int shockwaveDamage;
    private float shockwaveMaxRange;
    private status shockwaveElement;
    
    private bool explodeOnBullet = false;
    private bool explodeOnEnnemy = false;

    private bool canBeDestroyedByLaser = false;
    private float laserWaitBeforeHit = 0.3f;
    private float currentLaserCooldown = 0f;
    private static readonly int animReset = Animator.StringToHash("Reset");
    private static readonly int animExplode = Animator.StringToHash("Explode");

    public void Setup(PlayerData _)
    {
        isShockwaveEnabled = _.generic.boolA;
        shockwaveMaxRange = _.generic.floatA;
        shockwaveDamage = _.generic.intA;
        shockwaveElement = _.generic.elementA;
        explodeOnBullet = _.generic.boolC;
        explodeOnEnnemy = _.generic.boolB;

        dictGoToGhost[gameObject] = this;
        StartCoroutine(nameof(DestroyByWait));
        
        currentLaserCooldown = laserWaitBeforeHit;

        if (DataSelector.selectedWeapon == "Laser") StartCoroutine(nameof(WaitLaser));
        
        animator.SetTrigger(animReset);

    }

    private IEnumerator WaitLaser()
    {
        currentLaserCooldown = laserWaitBeforeHit;
        while (currentLaserCooldown > 0)
        {
            yield return Helpers.GetWaitFixed;
            currentLaserCooldown -= Time.fixedDeltaTime;
        }
        canBeDestroyedByLaser = true;
    }

    private void OnDestroy()
    {
        dictGoToGhost.Remove(gameObject);
    }

    private IEnumerator DestroyByWait()
    {
        yield return Helpers.GetWait(5.0f);
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
        animator.SetTrigger(animExplode);
        DoShockwave();
        Invoke(nameof(Recall),0.5f);
    }

    private void Recall()
    {
        PowerGhost.recallGhost(this);
    }

    private void DoShockwave()
    {
        Shockwave shockwaveGhost = PowerGhost.SpawnShockwave(transform.position);
        shockwaveGhost.transform.localScale = Vector3.zero;
        shockwaveGhost.Setup(shockwaveMaxRange, shockwaveDamage, shockwaveElement, 0);
        shockwaveGhost.setRecallMethod(delegate
        {
            PowerGhost.recallShockwave(shockwaveGhost);
            
        });
        shockwaveGhost.doShockwave(true);
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Shockwave shockwave;
    [SerializeField] private Collider2D col;

    private bool isShockwaveEnabled;
    private int shockwaveDamage;
    private float shockwaveMaxRange;
    private status shockwaveElement;
    
    private bool explodeOnBullet = false;
    private bool explodeOnEnnemy = false;

    public void Setup(PlayerData _)
    {
        isShockwaveEnabled = _.generic.boolA;
        shockwaveMaxRange = _.generic.floatA;
        shockwaveDamage = _.generic.intA;
        shockwaveElement = _.generic.elementA;
        explodeOnBullet = _.generic.boolC;
        explodeOnEnnemy = _.generic.boolB;
        
        StartCoroutine(nameof(DestroyByWait));
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
        animator.enabled = true;
        DoShockwave();
        Destroy(gameObject,0.5f);
    }

    private void DoShockwave()
    {
        Shockwave shockwaveGhost = Instantiate(shockwave);
        shockwaveGhost.transform.position = this.transform.position;
        shockwaveGhost.transform.localScale = Vector3.zero;
        shockwaveGhost.Setup(shockwaveMaxRange, shockwaveDamage, shockwaveElement);
        shockwaveGhost.doShockwave();
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
}

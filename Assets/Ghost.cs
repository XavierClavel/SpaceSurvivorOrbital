using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    Animator animator;

    [SerializeField] private Shockwave shockwave;

    private bool isShockwaveEnabled;
    private int shockwaveDamage;
    private float shockwaveMaxRange;
    private status shockwaveElement;
    private bool playerBullet = false;
    private bool contactEnnemy = false;

    public void Setup(PlayerData _)
    {
        isShockwaveEnabled = _.generic.boolA;
        shockwaveMaxRange = _.generic.floatA;
        shockwaveDamage = _.generic.intA;
        shockwaveElement = _.generic.elementA;
        playerBullet = _.generic.boolC;
        contactEnnemy = _.generic.boolB;
        

        animator = GetComponent<Animator>();
        StartCoroutine(WaitBeforeDestroy());

        Debug.Log($"shockwave range : {shockwaveMaxRange}");
    }

    private IEnumerator WaitBeforeDestroy()
    {
        yield return new WaitForSeconds(5.0f);

        animator.enabled = true;

        DoShockwave();

        yield return new WaitForSeconds(0.5f);

        Destroy(this.gameObject);
    }

    private void DoShockwave()
    {
        Shockwave shockwaveGhost = Instantiate(shockwave);
        shockwaveGhost.transform.position = this.transform.position;
        shockwaveGhost.transform.localScale = Vector3.zero;
        shockwaveGhost.Setup(shockwaveMaxRange, shockwaveDamage, shockwaveElement);
        shockwaveGhost.doShockwave();
    }
    private IEnumerator DestroyByOther()
    {
        StopCoroutine(WaitBeforeDestroy());

        animator.enabled = true;
        DoShockwave();

        yield return new WaitForSeconds(0.5f);

        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(Vault.layer.Ennemies) && contactEnnemy) StartCoroutine(DestroyByOther());
    }
}

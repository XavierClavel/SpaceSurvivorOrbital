using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    Animator animator;

    [SerializeField] private Shockwave shockwave;

    private bool isShockwaveEnabled;
    private int shockwaveDamage;
    private float shockwaveMaxRange;
    private status shockwaveElement;

    public void Setup(PlayerData _)
    {
        isShockwaveEnabled = _.generic.boolA;
        shockwaveMaxRange = _.generic.floatA;
        shockwaveDamage = _.generic.intA;
        shockwaveElement = _.generic.elementA;

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
    private IEnumerator DestroyByPlayer()
    {
        animator.enabled = true;
        DoShockwave();

        yield return new WaitForSeconds(0.5f);

        Destroy(this.gameObject);
    }
}

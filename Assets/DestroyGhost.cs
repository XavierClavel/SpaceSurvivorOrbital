using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGhost : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(WaitBeforeDestroy());
    }

    private IEnumerator WaitBeforeDestroy()
    {
        yield return new WaitForSeconds(2.0f);

        animator.enabled = true;

        yield return new WaitForSeconds(0.5f);

        Destroy(this.gameObject);
    }
}

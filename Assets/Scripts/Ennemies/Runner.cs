using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Runner : Ennemy
{

    internal override void FixedUpdate()
    {
        base.FixedUpdate();
        Move(directionToPlayer);
    }


    override internal void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);
        if (other.gameObject.CompareTag("Player"))
        {
            if (PlayerController.instance.hasWon) return;
            StartCoroutine("Hurt");
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player")) StopCoroutine("Hurt");
    }

    IEnumerator Hurt()
    {
        while (true)
        {
            PlayerController.Hurt(baseDamage);
            yield return wait;
        }
    }


}

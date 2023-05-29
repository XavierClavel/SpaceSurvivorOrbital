using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Runner : Ennemy
{

    protected override void FixedUpdate()
    {
        if (knockback) return;
        base.FixedUpdate();
        Move(directionToPlayer);
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (PlayerController.instance.hasWon) return;
            PlayerController.Hurt(baseDamage);
            ApplyKnockback();
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : Ennemy
{

    bool healing = false;

    bool needsToRecharge = true;
    bool recharging = false;

    [Header("Additional References")]
    [SerializeField] Transform healRangeDisplay;
    LayerMask mask;

    internal override void Start()
    {
        base.Start();
        healRangeDisplay.localScale = range * Vector3.one;
        mask = LayerMask.GetMask("Ennemies");
    }

    internal override void FixedUpdate()
    //TODO : run on lower frequency
    {
        base.FixedUpdate();
        switch (distanceToPlayer.magnitude)
        {
            case < 3f:
                healing = false;
                Move(-directionToPlayer);
                break;

            case < 6f:
                healing = true;
                if (recharging) break;
                if (needsToRecharge) StartCoroutine("Recharge");
                else Heal();
                break;

            default:
                healing = false;
                Move(directionToPlayer);
                break;
        }
    }

    IEnumerator Recharge()
    {
        recharging = true;
        yield return wait;
        recharging = false;
        needsToRecharge = false;
        if (healing) Heal();
    }

    void Heal()
    {
        Debug.Log("heal");
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, range, Vector2.up, 0.00001f, mask);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject == gameObject) continue; //Does not heal himself
            Debug.Log("one ennemy has been healed");
            Planet.dictObjectToEnnemy[hit.collider.gameObject].HealSelf(baseDamage);
        }
        needsToRecharge = true;
        StartCoroutine("Recharge");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Sprites;
using UnityEngine;
using UnityEngine.InputSystem;
using static Vault;

public class PowerFlame : Power
{
    private static Stacker<Ennemy> ennemyStacker;
    private HashSet<status> elements = new HashSet<status>();
    public ParticleSystem flamePS;
    private bool flameIsActive;

    public override void onSetup()
    {
        ennemyStacker = new Stacker<Ennemy>();
        StartCoroutine(nameof(FlameThrower));
    }

    void Update()
    {
        gameObject.transform.position = player.transform.position;
        Vector2 aimDirection = player.aimVector;

        if (aimDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
    private void FixedUpdate()
    {
        DealDamage();
    }
    private void DealDamage()
    {
        if(flameIsActive)
        {
            foreach (Ennemy ennemy in ennemyStacker.get())
            {
                ennemy.StackDamage(stats.baseDamage.x, new HashSet<status>());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Ennemy ennemy = ObjectManager.dictObjectToEnnemy[other.gameObject];
        ennemyStacker.stack(ennemy);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Ennemy ennemy = ObjectManager.dictObjectToEnnemy[other.gameObject];
        ennemyStacker.unstack(ennemy);
    }

    IEnumerator FlameThrower()
    {
        while (true)
        {
            flameIsActive = true;
            flamePS.Play();
            yield return new WaitForSeconds(stats.attackSpeed);
            flameIsActive = false;
            flamePS.Stop();
            yield return new WaitForSeconds(stats.cooldown);
        }

    }

}

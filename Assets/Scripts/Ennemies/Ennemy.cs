using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Ennemy : MonoBehaviour
{
    internal PlayerController player;
    [SerializeField] Slider healthBar;
    [SerializeField] internal Rigidbody2D rb;
    SoundManager soundManager;
    internal Vector2 distanceToPlayer;
    internal Vector2 directionToPlayer;
    static internal WaitForSeconds wait;
    static internal WaitForSeconds waitStateStep;
    static WaitForSeconds waitPoison;
    static WaitForSeconds waitPoisonDamage;
    static WaitForSeconds waitFire;
    static WaitForSeconds waitFireDamage;
    static WaitForSeconds waitIce;
    float speedMultiplier = 1f;


    [Header("Parameters")]
    [SerializeField] internal int baseHealth = 150;
    [SerializeField] internal float speed = 1f;
    [SerializeField] internal float fleeSpeed = 2f;
    [SerializeField] internal float attackSpeed = 0.5f;
    [SerializeField] internal int baseDamage = 5;
    [SerializeField] internal float range = 5f;
    [SerializeField] internal float stateStep = 0.5f;
    int _health;
    int health
    {
        get { return _health; }
        set
        {
            value = Helpers.CeilInt(value, baseHealth);
            if (value == baseHealth && healthBar.gameObject.activeInHierarchy) healthBar.gameObject.SetActive(false);
            else if (!healthBar.gameObject.activeInHierarchy) healthBar.gameObject.SetActive(true);
            _health = value;
            healthBar.value = value;
            //SoundManager.instance.PlaySfx(transform, sfx.playerHit);
            if (value <= 0) Death();
        }
    }


    internal virtual void Start()
    {
        soundManager = SoundManager.instance;
        player = PlayerController.instance;
        StressTest.nbEnnemies++;
        _health = baseHealth;
        healthBar.maxValue = _health;
        healthBar.value = _health;

        wait = Helpers.GetWait(attackSpeed);
        waitStateStep = Helpers.GetWait(stateStep);
        waitPoison = Helpers.GetWait(player.poisonDuration);
        waitPoisonDamage = Helpers.GetWait(player.poisonPeriod);
        waitIce = Helpers.GetWait(player.iceDuration);

        //TODO : static initalizer

        Planet.dictObjectToEnnemy.Add(gameObject, this);
    }

    internal virtual void FixedUpdate()
    {
        distanceToPlayer = player.transform.position - transform.position;
        directionToPlayer = distanceToPlayer.normalized;
    }

    internal void Move(Vector2 direction, float speed)
    {
        rb.MovePosition(rb.position + direction * Time.fixedDeltaTime * speed * speedMultiplier);
    }

    internal void Move(Vector2 direction)
    {
        rb.MovePosition(rb.position + direction * Time.fixedDeltaTime * speed * speedMultiplier);
    }


    public void Hurt(int damage, status effect, bool critical)
    {
        healthChange value = critical ? healthChange.critical : healthChange.hit;
        if (damage != 0)
        {
            DamageDisplayHandler.DisplayDamage(damage, transform.position, value);
            health -= damage;
        }

        switch (effect)
        {
            case status.none:
                break;

            case status.poison:
                StartCoroutine("PoisonEffect");
                break;

            case status.ice:
                StartCoroutine("IceEffect");
                break;

            case status.fire:
                StartCoroutine("FireEffect");
                break;
        }
    }

    public void HealSelf(int amount)
    {
        health += amount;
        DamageDisplayHandler.DisplayDamage(amount, transform.position, healthChange.heal);
    }

    internal virtual void Death()
    {
        StressTest.nbEnnemies--;
        soundManager.PlaySfx(transform, sfx.ennemyExplosion);
        Planet.dictObjectToEnnemy.Remove(gameObject);
        Destroy(gameObject);
    }

    IEnumerator PoisonEffect()
    {
        StartCoroutine("PoisonDamage");
        yield return waitPoison;
        StopCoroutine("PoisonDamage");
    }

    IEnumerator PoisonDamage()
    {
        while (true)
        {
            yield return waitPoisonDamage;
            DamageDisplayHandler.DisplayDamage(player.poisonDamage, transform.position, healthChange.poison);
            health -= player.poisonDamage;
        }
    }

    IEnumerator FireEffect()
    {
        StartCoroutine("FireDamage");
        yield return waitFire;
        StopCoroutine("FireDamage");
    }

    IEnumerator FireDamage()
    {
        while (true)
        {
            yield return waitFireDamage;
            DamageDisplayHandler.DisplayDamage(player.fireDamage, transform.position, healthChange.fire);
            health -= player.fireDamage;
        }
    }

    IEnumerator IceEffect()
    {
        speedMultiplier = player.iceSpeedMultiplier;
        yield return waitIce;
        speedMultiplier = 1f;
    }


}

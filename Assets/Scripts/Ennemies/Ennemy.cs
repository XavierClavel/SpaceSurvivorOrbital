using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Ennemy : MonoBehaviour
{
    protected PlayerController player;
    [SerializeField] Slider healthBar;
    [SerializeField] protected Rigidbody2D rb;
    SoundManager soundManager;
    protected Vector2 distanceToPlayer;
    protected Vector2 directionToPlayer;
    static protected WaitForSeconds wait;
    static protected WaitForSeconds waitStateStep;
    static WaitForSeconds waitPoison;
    static WaitForSeconds waitPoisonDamage;
    static WaitForSeconds waitFire;
    static WaitForSeconds waitFireDamage;
    static WaitForSeconds waitIce;
    float speedMultiplier = 1f;
    public int cost;


    [Header("Parameters")]
    [SerializeField] protected int baseHealth = 150;
    [SerializeField] protected float speed = 1f;
    [SerializeField] protected float fleeSpeed = 2f;
    [SerializeField] protected float attackSpeed = 0.5f;
    [SerializeField] protected int baseDamage = 5;
    [SerializeField] protected float range = 5f;
    [SerializeField] protected float stateStep = 0.5f;
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
        waitPoison = Helpers.GetWait(PlayerManager.poisonDuration);
        waitPoisonDamage = Helpers.GetWait(PlayerManager.poisonPeriod);
        waitIce = Helpers.GetWait(PlayerManager.iceDuration);

        //TODO : static initalizer

        SpawnManager.dictObjectToEnnemy.Add(gameObject, this);
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
        SpawnManager.dictObjectToEnnemy.Remove(gameObject);
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
            DamageDisplayHandler.DisplayDamage(PlayerManager.poisonDamage, transform.position, healthChange.poison);
            health -= PlayerManager.poisonDamage;
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
            DamageDisplayHandler.DisplayDamage(PlayerManager.fireDamage, transform.position, healthChange.fire);
            health -= PlayerManager.fireDamage;
        }
    }

    IEnumerator IceEffect()
    {
        speedMultiplier = PlayerManager.iceSpeedMultiplier;
        yield return waitIce;
        speedMultiplier = 1f;
    }


}

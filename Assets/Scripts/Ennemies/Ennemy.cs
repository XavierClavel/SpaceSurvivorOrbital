using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class Ennemy : Breakable
{
    public UnityEvent onDeath = new UnityEvent();

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

    [Header("Knockback Parameters")]
    [SerializeField] float knockbackForce = 5f;
    [SerializeField] float knockbackRiseDuration = 0.10f;
    [SerializeField] float knockbackPlateauDuration = 0.10f;
    [SerializeField] float knockbackFallDuration = 0.10f;
    [SerializeField] float knockbackStunDuration = 0.10f;


    [Header("Parameters")]
    [SerializeField] protected float speed = 1f;
    [SerializeField] protected float fleeSpeed = 2f;
    [SerializeField] protected float attackSpeed = 0.5f;
    [SerializeField] protected float range = 5f;
    [SerializeField] protected float stateStep = 0.5f;
    int _health;
    int health
    {
        get { return _health; }
        set
        {
            value = Helpers.CeilInt(value, maxHealth);
            if (value == maxHealth && healthBar.gameObject.activeInHierarchy) healthBar.gameObject.SetActive(false);
            else if (!healthBar.gameObject.activeInHierarchy) healthBar.gameObject.SetActive(true);
            _health = value;
            healthBar.value = value;
            //SoundManager.instance.PlaySfx(transform, sfx.playerHit);
            if (value <= 0) Death();
        }
    }

    protected bool knockback = false;

    WaitForSeconds knockbackWindow;


    protected override void Start()
    {
        base.Start();

        soundManager = SoundManager.instance;
        player = PlayerController.instance;
        StressTest.nbEnnemies++;
        _health = maxHealth;
        healthBar.maxValue = _health;
        healthBar.value = _health;

        wait = Helpers.GetWait(attackSpeed);
        waitStateStep = Helpers.GetWait(stateStep);
        waitPoison = Helpers.GetWait(PlayerManager.poisonDuration);
        waitPoisonDamage = Helpers.GetWait(PlayerManager.poisonPeriod);
        waitIce = Helpers.GetWait(PlayerManager.iceDuration);
        knockbackWindow = Helpers.GetWait(knockbackRiseDuration);

        //TODO : static initalizer

        ObjectManager.dictObjectToEnnemy.Add(gameObject, this);
    }


    protected virtual void FixedUpdate()
    {
        distanceToPlayer = player.transform.position - transform.position;
        directionToPlayer = distanceToPlayer.normalized;
    }

    protected void Move(Vector2 direction, float speed)
    {
        rb.MovePosition(rb.position + direction * Time.fixedDeltaTime * speed * speedMultiplier);
    }

    protected void Move(Vector2 direction)
    {
        rb.MovePosition(rb.position + direction * Time.fixedDeltaTime * speed * speedMultiplier);
    }

    public override void Hit(int damage, status effect, bool critical)
    {
        base.Hit(damage, effect, critical);
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
                StartCoroutine(nameof(PoisonEffect));
                break;

            case status.ice:
                StartCoroutine(nameof(IceEffect));
                break;

            case status.fire:
                StartCoroutine(nameof(FireEffect));
                break;
        }
        ApplyKnockback();
    }

    public void ApplyKnockback()
    {
        OnKnockbackStart();
        Sequence sequence = DOTween.Sequence();

        sequence.Append(DOTween.To(() => rb.velocity, x => rb.velocity = x, (Vector2)(transform.position - player.transform.position).normalized * knockbackForce, knockbackRiseDuration));
        sequence.AppendInterval(knockbackPlateauDuration);
        sequence.Append(DOTween.To(() => rb.velocity, x => rb.velocity = x, Vector2.zero, knockbackFallDuration));
        sequence.AppendInterval(knockbackStunDuration);
        sequence.onComplete += OnKnockbackEnd;
    }

    protected virtual void OnKnockbackStart()
    {
        knockback = true;
    }

    protected virtual void OnKnockbackEnd()
    {
        knockback = false;
        rb.velocity = Vector2.zero;
    }

    public void HealSelf(int amount)
    {
        health += amount;
        DamageDisplayHandler.DisplayDamage(amount, transform.position, healthChange.heal);
    }

    protected virtual void Death()
    {
        StressTest.nbEnnemies--;
        soundManager.PlaySfx(transform, sfx.ennemyExplosion);
        ObjectManager.dictObjectToEnnemy.Remove(gameObject);

        onDeath.Invoke();
        Destroy(gameObject);
    }



    #region elementalEffects

    IEnumerator PoisonEffect()
    {
        StartCoroutine(nameof(PoisonDamage));
        yield return waitPoison;
        StopCoroutine(nameof(PoisonDamage));
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
        StartCoroutine(nameof(FireDamage));
        yield return waitFire;
        StopCoroutine(nameof(FireDamage));
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

    #endregion

}

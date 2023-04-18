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
    internal WaitForSeconds wait;
    internal WaitForSeconds waitStateStep;


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

        Planet.dictObjectToEnnemy.Add(gameObject, this);
    }

    internal virtual void FixedUpdate()
    {
        distanceToPlayer = player.transform.position - transform.position;
        directionToPlayer = distanceToPlayer.normalized;
    }

    internal void Move(Vector2 direction, float speed)
    {
        rb.MovePosition(rb.position + direction * Time.fixedDeltaTime * speed);
    }

    internal void Move(Vector2 direction)
    {
        rb.MovePosition(rb.position + direction * Time.fixedDeltaTime * speed);
    }

    internal virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("BlueBullet"))
        {
            int damageTaken = 0;
            bool critical = false;
            PlayerController.HurtEnnemy(ref damageTaken, ref critical);
            DamageDisplayHandler.DisplayDamage(damageTaken, other.transform.position, healthChange.critical);
            health -= damageTaken;
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


}

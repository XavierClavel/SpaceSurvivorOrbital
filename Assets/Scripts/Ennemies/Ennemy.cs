using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ennemy : MonoBehaviour
{
    internal PlayerController player;
    [SerializeField] Slider healthBar;
    [SerializeField] internal Rigidbody2D rb;
    SoundManager soundManager;


    [Header("Parameters")]
    [SerializeField] internal int _health = 150;
    [SerializeField] internal float speed = 1f;
    public int health
    {
        get { return _health; }
        set
        {
            _health = value;
            healthBar.value = value;
            if (!healthBar.gameObject.activeInHierarchy) healthBar.gameObject.SetActive(true);
            //SoundManager.instance.PlaySfx(transform, sfx.playerHit);
            if (value <= 0) Death();
        }
    }


    internal virtual void Start()
    {
        soundManager = SoundManager.instance;
        player = PlayerController.instance;
        StressTest.nbEnnemies++;
        healthBar.maxValue = _health;
        healthBar.value = _health;
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

    internal virtual void Death()
    {
        StressTest.nbEnnemies--;
        soundManager.PlaySfx(transform, sfx.ennemyExplosion);
        Destroy(gameObject);
    }


}

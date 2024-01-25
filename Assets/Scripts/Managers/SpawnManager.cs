using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using UnityEngine.UI;

public class SpawnManager : Breakable
{
    [SerializeField] private Collider2D collider;
    
    private SpawnData spawnData;


    private int difficulty;
    private bool isDestroyed = false;

    int wallet;
    [SerializeField] Slider healthBar;
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

    protected virtual void Death()
    {
        if (isDestroyed) return;
        isDestroyed = true;
        collider.enabled = false;
        SoundManager.PlaySfx(transform, key: "Spawn_Destroy");
        ObjectManager.registerDenDestroyed();
        ObjectManager.dictObjectToEnnemy.Remove(gameObject);
        ObjectManager.unregisterHitable(gameObject);
        Destroy(gameObject);
    }


    protected override void Start()
    {
        base.Start();
        gameObject.tag = "Stele";
        
        difficulty = PlanetManager.getDifficulty();
        spawnData = DataManager.dictDifficulty[difficulty.ToString()];

        _health = spawnData.denHealth;
        healthBar.maxValue = _health;
        healthBar.value = _health;

        
        //playerTransform = PlayerController.instance.transform;
        
        ObjectManager.registerDenSpawned();
    }

    public override void Hit(HitInfo hitInfo)
    {
        base.Hit(hitInfo);
        SoundManager.PlaySfx(transform, key: "Spawn_Hit");
        healthChange value = hitInfo.critical ? healthChange.critical : healthChange.hit;
        if (hitInfo.damage != 0)
        {
            DamageDisplayHandler.DisplayDamage(hitInfo.damage, transform.position, value);
            health -= hitInfo.damage;
        }
    }
    
    protected override void StackHit(int damage, HashSet<status> elements)
    {
        DamageDisplayHandler.DisplayStackedDamage(gameObject, damage);
        health -= damage;
    }


}
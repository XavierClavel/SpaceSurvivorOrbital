using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using UnityEngine.UI;

public class SpawnManager : Breakable
{
    [SerializeField] TilesBankManager tilesBankManager;
    List<Ennemy> ennemyPrefabs;

    
    private SpawnData spawnData;

    //Transform playerTransform;
    bool doEnnemySpawn = true;
    float waveDuration = 20f;
    List<EntitySpawnInstance<Ennemy>> ennemiesToSpawnList = new List<EntitySpawnInstance<Ennemy>>();

    private int difficulty;

    int wallet;
    EntitySpawnInstanceComparer<Ennemy> comparer = new EntitySpawnInstanceComparer<Ennemy>();
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
        SoundManager.PlaySfx(transform, key: "Spawn_Destroy");
        ObjectManager.registerDenDestroyed();
        ObjectManager.dictObjectToEnnemy.Remove(gameObject);
        Destroy(gameObject);
    }


    protected override void Start()
    {
        base.Start();
        
        difficulty = PlanetManager.getDifficulty();
        spawnData = DataManager.dictDifficulty[difficulty.ToString()];

        _health = spawnData.denHealth;
        healthBar.maxValue = _health;
        healthBar.value = _health;

        
        wallet = spawnData.baseCost;
        ennemyPrefabs = tilesBankManager.GetEnnemies();
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
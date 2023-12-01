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

    public GameObject spawnPosition;
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
        ObjectManager.registerDenDestroyed();
        ObjectManager.dictObjectToEnnemy.Remove(gameObject);
        Destroy(gameObject);
    }


    public void debug_StopEnnemySpawn()
    {
        doEnnemySpawn = false;
        StopCoroutine(nameof(SpawnController));
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

        if (doEnnemySpawn)
        {
            StartCoroutine(nameof(SpawnController));
        }
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
    }
    
    public override void StackHit(int damage, int knockback)
    {
        base.StackHit(damage, knockback);
        DamageDisplayHandler.DisplayStackedDamage(gameObject, damage);
        health -= damage;
    }

    IEnumerator SpawnController()
    {
        float time = 0f;
        while (true)
        {
            if (ennemiesToSpawnList.Count != 0 && time >= ennemiesToSpawnList[0].spawnTime)
            {
                SpawnEnnemy(ennemiesToSpawnList[0].entity);
                ennemiesToSpawnList.RemoveAt(0);
            }

            if (time > waveDuration)
            {
                time = 0f;
                wallet += spawnData.increment;
                PrepareWave(wallet);
            }

            time += Time.fixedDeltaTime;
            yield return Helpers.GetWaitFixed;
        }
    }

    void PrepareWave(int maxCost)
    {
        int currentCost = 0;
        ennemiesToSpawnList = new List<EntitySpawnInstance<Ennemy>>();
        List<Ennemy> ennemies = ennemyPrefabs.Copy();
        ennemies = ennemies.FindAll(it => 
            DataManager.dictObjects[it.name].cost < spawnData.maxSpending &&
            DataManager.dictObjects[it.name].cost > spawnData.minSpending
            ).ToList();

        while (currentCost < maxCost)
        {
            Ennemy ennemy = ennemies.getRandom();
            int newCost = currentCost + DataManager.dictObjects[ennemy.name].cost;
            if (newCost > maxCost)
            {
                ennemies.Remove(ennemy);
                if (ennemies.Count == 0) break;
                else continue;
            }

            float spawnTime = Random.Range(0f, waveDuration);

            ennemiesToSpawnList.Add(new EntitySpawnInstance<Ennemy>(spawnTime, ennemy));
            currentCost = newCost;
        }

        ennemiesToSpawnList.Sort(comparer);
    }

    public void SpawnEnnemy(Ennemy ennemy)
    {
        Vector3 position = spawnPosition.transform.position;
        //Helpers.getRandomPositionInRing(4f, 4f, shape.square) + transform.position;
        Instantiate(ennemy.gameObject, position, Quaternion.identity);
    }

    public void SpawnEnnemy()
    {
        GameObject ennemyPrefab = ennemyPrefabs[Random.Range(0, ennemyPrefabs.Count)].gameObject;
        Vector3 position = spawnPosition.transform.position;
        //Helpers.getRandomPositionInRing(4f, 4f, shape.square) + transform.position;
        Instantiate(ennemyPrefab, position, Quaternion.identity);
    }


}

public class EntitySpawnInstance<T>
{
    public float spawnTime;
    public T entity;

    public EntitySpawnInstance(float spawnTime, T entity)
    {
        this.spawnTime = spawnTime;
        this.entity = entity;
    }
}

public class EntitySpawnInstanceComparer<T> : IComparer<EntitySpawnInstance<T>>
{
    public int Compare(EntitySpawnInstance<T> entityA, EntitySpawnInstance<T> entityB)
    {
        return entityA.spawnTime.CompareTo(entityB.spawnTime);
    }
}

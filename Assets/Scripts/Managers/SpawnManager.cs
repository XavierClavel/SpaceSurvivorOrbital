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

    //Transform playerTransform;
    bool doEnnemySpawn = true;
    [SerializeField] List<int> baseCost = new List<int>();
    [SerializeField] List<int> increaseByWave = new List<int>();
    [SerializeField] List<int> waveDurationList = new List<int>();
    float waveDuration;
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

        _health = maxHealth;
        healthBar.maxValue = _health;
        healthBar.value = _health;

        difficulty = PlanetManager.getDifficulty();
        wallet = baseCost[difficulty];
        ennemyPrefabs = tilesBankManager.GetEnnemies();
        //playerTransform = PlayerController.instance.transform;
        waveDuration = waveDurationList[difficulty];
        
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
                wallet += increaseByWave[difficulty];
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

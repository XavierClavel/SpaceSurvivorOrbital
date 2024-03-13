using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class Spawner : MonoBehaviour
{
    [SerializeField] TilesBankManager tilesBankManager;
    [SerializeField] private Boss boss;
    List<Ennemy> ennemyPrefabs;
    private int increment;

    [Header("Multiplier")]
    [SerializeField] public int baseCostLarge;
    [SerializeField] public int baseCostMedium;
    [SerializeField] public int baseCostSmall;

    [SerializeField] public int incrementLarge;
    [SerializeField] public int incrementMedium;
    [SerializeField] public int incrementSmall;

    [SerializeField] public float waveDurationLarge;
    [SerializeField] public float waveDurationMedium;
    [SerializeField] public float waveDurationSmall;
    

    private SpawnData spawnData;

    float waveDuration;
    List<EntitySpawnInstance<Ennemy>> ennemiesToSpawnList = new List<EntitySpawnInstance<Ennemy>>();

    private int difficulty;

    int wallet;
    EntitySpawnInstanceComparer<Ennemy> comparer = new EntitySpawnInstanceComparer<Ennemy>();
    

    protected void Start()
    {
        if (PlayerManager.isTuto)
        {
            return;
        }
        difficulty = PlanetSelector.getDifficulty();
        spawnData = DataManager.dictDifficulty[difficulty.ToString()];

        wallet = spawnData.baseCost * PlanetManager.getSizeCategory() switch
        {
            planetSize.large => baseCostLarge,
            planetSize.medium => baseCostMedium,
            planetSize.small => baseCostSmall,
            _ => baseCostMedium
        };

        waveDuration = PlanetManager.getSizeCategory() switch
        {
            planetSize.large => waveDurationLarge,
            planetSize.medium => waveDurationMedium,
            planetSize.small => waveDurationSmall,
            _ => waveDurationMedium
        };

        increment = spawnData.increment * PlanetManager.getSizeCategory() switch
        {
            planetSize.large => incrementLarge,
            planetSize.medium => incrementMedium,
            planetSize.small => incrementSmall,
            _ => incrementMedium
        };
        
        Debug.Log($"Difficulty : {difficulty}");
        Debug.Log($"Wallet : {wallet}");
        
        ennemyPrefabs = tilesBankManager.GetEnnemies();
        //Debug.Log(tilesBankManager.GetEnnemies().Count);

        if (!DebugManager.doNoEnnemySpawn())
        {
            StartCoroutine(nameof(SpawnController));
        }

        if (PlanetManager.isBoss() || DebugManager.doSpawnBossOnStart())
        {
            Debug.Log("Boss spawned");
            SpawnEnnemy(boss);   
        }
    }


    IEnumerator SpawnController()
    {
        float time = 0f;
        int waveIndex = 0;
        PrepareWave(wallet, waveIndex);
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
                wallet += increment;
                waveIndex++;
                PrepareWave(wallet, waveIndex);
            }

            time += Time.fixedDeltaTime;
            yield return Helpers.getWaitFixed();
        }
    }

    void PrepareWave(int maxCost, int waveIndex)
    {
        int currentCost = 0;
        ennemiesToSpawnList = new List<EntitySpawnInstance<Ennemy>>();
        List<Ennemy> ennemies = ennemyPrefabs.Copy();
        ennemies = ennemies.FindAll(it =>
            DataManager.dictObjects[it.name].cost <= spawnData.maxSpending &&
            DataManager.dictObjects[it.name].cost >= spawnData.minSpending
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
        
        Debug.Log(waveIndex);

        if (waveIndex == 2 && PlanetSelector.getDifficulty() > 1 && !PlanetManager.isBoss())
        {
            SpawnEnnemy(ScriptableObjectManager.dictKeyToBossData[PlayerManager.currentBoss].miniBosses.getRandom());
        }
    }

    public static void SpawnEnnemy(Ennemy ennemy)
    {
        Bounds bounds = Camera.main.getBounds();
        Vector3 position = Helpers.getRandomPositionInRing(bounds.extents, shape.square) + PlayerController.instance.transform.position;
        Instantiate(ennemy, position, Quaternion.identity);
    }

    public void SpawnEnnemy()
    {
        GameObject ennemyPrefab = ennemyPrefabs[Random.Range(0, ennemyPrefabs.Count)].gameObject;
        Vector3 position = Helpers.getRandomPositionInRing(10f, 10f, shape.square) + transform.position;
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


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

    private int planetDiameter;

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

    //Transform playerTransform;
    bool doEnnemySpawn = true;
    float waveDuration;
    List<EntitySpawnInstance<Ennemy>> ennemiesToSpawnList = new List<EntitySpawnInstance<Ennemy>>();

    private int difficulty;

    int wallet;
    EntitySpawnInstanceComparer<Ennemy> comparer = new EntitySpawnInstanceComparer<Ennemy>();


    public void debug_StopEnnemySpawn()
    {
        doEnnemySpawn = false;
        StopCoroutine(nameof(SpawnController));
    }

    protected void Start()
    {
        difficulty = PlanetSelector.getDifficulty();
        Debug.Log($"Difficulty : {difficulty}");
        spawnData = DataManager.dictDifficulty[difficulty.ToString()];

        planetDiameter = PlanetManager.getSize();
        if (planetDiameter == 7)
        {
            wallet = spawnData.baseCost * baseCostLarge;
        } else if (planetDiameter == 5)
        {
            wallet = spawnData.baseCost * baseCostMedium;
        }
        else if (planetDiameter == 3)
        {
            wallet = spawnData.baseCost * baseCostSmall;
        }
        Debug.Log("wallet is" + wallet);

        if (planetDiameter == 7)
        {
            waveDuration = waveDurationLarge;
        }
        else if (planetDiameter == 5)
        {
            waveDuration = waveDurationMedium;
        }
        else if (planetDiameter == 3)
        {
            waveDuration = waveDurationSmall;
        }


        ennemyPrefabs = tilesBankManager.GetEnnemies();
        //Debug.Log(tilesBankManager.GetEnnemies().Count);

        if (!DebugManager.doNoEnnemySpawn())
        {
            StartCoroutine(nameof(SpawnController));
        }

        if (PlanetManager.isBoss() || DebugManager.doSpawnBossOnStart())
        {
            Debug.Log("boss spawned");
            SpawnEnnemy(boss);   
        }
    }


    IEnumerator SpawnController()
    {
        float time = 0f;
        PrepareWave(wallet);
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
                if (planetDiameter == 7)
                {
                    wallet += spawnData.increment * incrementLarge;
                }
                else if (planetDiameter == 5)
                {
                    wallet += spawnData.increment * incrementMedium;
                }
                else if (planetDiameter == 3)
                {
                    wallet += spawnData.increment * incrementSmall;
                }
                PrepareWave(wallet);
            }

            time += Time.fixedDeltaTime;
            yield return Helpers.getWaitFixed();
        }
    }

    void PrepareWave(int maxCost)
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
    }

    public void SpawnEnnemy(Ennemy ennemy)
    {
        Bounds bounds = Camera.main.getBounds();
        Vector3 position = Helpers.getRandomPositionInRing(bounds.extents, shape.square) + PlayerController.instance.transform.position;
        Instantiate(ennemy.gameObject, position, Quaternion.identity);
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


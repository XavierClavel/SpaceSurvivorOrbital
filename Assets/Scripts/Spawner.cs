using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] TilesBankManager tilesBankManager;
    List<Ennemy> ennemyPrefabs;

    private SpawnData spawnData;

    //Transform playerTransform;
    bool doEnnemySpawn = true;
    float waveDuration = 10f;
    List<EntitySpawnInstance<Ennemy>> ennemiesToSpawnList = new List<EntitySpawnInstance<Ennemy>>();

    private int difficulty;

    int wallet;
    EntitySpawnInstanceComparer<Ennemy> comparer = new EntitySpawnInstanceComparer<Ennemy>();

    protected virtual void Death()
    {
        SoundManager.PlaySfx(transform, key: "Spawn_Destroy");
        ObjectManager.registerDenDestroyed();
        ObjectManager.dictObjectToEnnemy.Remove(gameObject);
        Destroy(gameObject);
    }


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

        wallet = spawnData.baseCost;
        ennemyPrefabs = tilesBankManager.GetEnnemies();
        //Debug.Log(tilesBankManager.GetEnnemies().Count);

        if (!DebugManager.doNoEnnemySpawn())
        {
            StartCoroutine(nameof(SpawnController));
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
        Vector3 position = Helpers.getRandomPositionInRing(10f, 10f, shape.square) + transform.position;
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


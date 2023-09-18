using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;
    [SerializeField] TilesBankManager tilesBankManager;
    List<Ennemy> ennemyPrefabs;

    Transform playerTransform;
    bool doEnnemySpawn = true;
    [SerializeField] List<int> baseCost = new List<int>();
    [SerializeField] List<int> increaseByWave = new List<int>();
    [SerializeField] List<int> waveDurationList = new List<int>();
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

    void Start()
    {
        instance = this;
        difficulty = PlanetManager.getDifficulty();
        wallet = baseCost[difficulty];
        ennemyPrefabs = tilesBankManager.GetEnnemies();
        playerTransform = PlayerController.instance.transform;
        waveDuration = waveDurationList[difficulty];

        if (doEnnemySpawn)
        {
            StartCoroutine(nameof(SpawnController));
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

        while (currentCost < maxCost)
        {
            Ennemy ennemy = ennemyPrefabs.getRandom();
            int newCost = currentCost + ennemy.cost;
            if (newCost > maxCost) break;

            float spawnTime = Random.Range(0f, waveDuration);

            ennemiesToSpawnList.Add(new EntitySpawnInstance<Ennemy>(spawnTime, ennemy));
            currentCost = newCost;
        }
        Debug.Log(ennemiesToSpawnList.Count);

        ennemiesToSpawnList.Sort(comparer);
    }

    public void SpawnEnnemy(Ennemy ennemy)
    {
        Vector3 position = Helpers.getRandomPositionInRing(5f, 10f, shape.square) + playerTransform.position;
        Instantiate(ennemy.gameObject, position, Quaternion.identity);
    }

    public void SpawnEnnemy()
    {
        GameObject ennemyPrefab = ennemyPrefabs[Random.Range(0, ennemyPrefabs.Count)].gameObject;
        Vector3 position = Helpers.getRandomPositionInRing(5f, 10f, shape.square) + playerTransform.position;
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

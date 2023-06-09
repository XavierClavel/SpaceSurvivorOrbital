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
    [SerializeField] float waveLength = 4f;
    Transform playerTransform;
    bool doEnnemySpawn = true;

    int cost = 10;

    public void debug_StopEnnemySpawn()
    {
        doEnnemySpawn = false;
        StopCoroutine(nameof(SpawnController));
    }

    void Start()
    {
        instance = this;
        cost = PlanetManager.getInitialCost();
        ennemyPrefabs = tilesBankManager.GetEnnemies();
        playerTransform = PlayerController.instance.transform;
        if (doEnnemySpawn)
        {
            StartCoroutine(nameof(SpawnController));
        }
    }

    IEnumerator SpawnController()
    {
        while (true)
        {
            yield return Helpers.GetWait(waveLength);
            StartCoroutine(nameof(SpawnWave), cost);
            cost += 5;
        }
    }

    IEnumerator SpawnWave(int maxCost)
    {
        int currentCost = 0;
        while (currentCost < maxCost)
        {
            yield return null;
            Ennemy ennemy = ennemyPrefabs.getRandom();
            int newCost = currentCost + ennemy.cost;
            if (newCost > maxCost) break;

            Instantiate(ennemy.gameObject, randomPos() + playerTransform.position, Quaternion.identity);
            currentCost = newCost;

            if (newCost == maxCost) yield break;
        }
    }

    public void SpawnEnnemy()
    {
        GameObject ennemyPrefab = ennemyPrefabs[Random.Range(0, ennemyPrefabs.Count)].gameObject;
        Instantiate(ennemyPrefab, randomPos() + playerTransform.position, Quaternion.identity);
    }

    Vector3 randomPos()
    {
        float signA = Random.Range(0, 2) * 2 - 1;
        float signB = Random.Range(0, 2) * 2 - 1;
        return signA * Random.Range(6f, 12f) * Vector2.up + signB * Random.Range(4f, 8f) * Vector2.right;
    }


}

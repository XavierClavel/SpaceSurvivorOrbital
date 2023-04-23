using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;
    [SerializeField] List<Ennemy> ennemyPrefabs = new List<Ennemy>();
    [SerializeField] float waveLength = 4f;
    Transform playerTransform;
    [SerializeField] bool doEnnemySpawn = true;
    public static Dictionary<GameObject, Ennemy> dictObjectToEnnemy = new Dictionary<GameObject, Ennemy>();
    public static Dictionary<GameObject, Resource> dictObjectToResource = new Dictionary<GameObject, Resource>();
    public static Dictionary<GameObject, IInteractable> dictObjectToInteractable = new Dictionary<GameObject, IInteractable>();
    int cost = 10;

    private void Awake()
    {
        instance = this;
    }

    IEnumerator SpawnController()
    {
        while (true)
        {
            yield return Helpers.GetWait(waveLength);
            StartCoroutine("SpawnWave", cost);
            cost += 5;
        }
    }

    void Start()
    {
        playerTransform = PlayerController.instance.transform;
        if (doEnnemySpawn)
        {
            StartCoroutine("SpawnController");
        }
    }

    IEnumerator SpawnWave(int maxCost)
    {
        int currentCost = 0;
        while (currentCost < maxCost)
        {
            yield return null;
            int randomIndex = Random.Range(0, ennemyPrefabs.Count);
            Ennemy ennemy = ennemyPrefabs[randomIndex];
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class Planet : MonoBehaviour
{
    [HideInInspector] public Vector3 position;
    [HideInInspector] public float gravityRadius;
    [HideInInspector] public float size;
    [HideInInspector] public float mass = 10;
    public static Planet instance;
    [SerializeField] List<GameObject> ennemyPrefabs;
    float spawnRate = 4f;
    Transform playerTransform;
    [SerializeField] bool doEnnemySpawn = true;

    private void Awake()
    {
        position = transform.position;
        instance = this;
    }

    IEnumerator SpawnController()
    {
        yield return Helpers.GetWait(20f);
        spawnRate = 2.5f;
        yield return Helpers.GetWait(20f);
        spawnRate = 1.5f;
        yield return Helpers.GetWait(20f);
        spawnRate = 1.25f;
        yield return Helpers.GetWait(20f);
        spawnRate = 1f;
    }

    void Start()
    {
        playerTransform = PlayerController.instance.transform;
        if (doEnnemySpawn)
        {
            StartCoroutine("SpawnEnnemies");
            StartCoroutine("SpawnController");
        }
    }

    IEnumerator SpawnEnnemies()
    {
        while (true)
        {
            yield return Helpers.GetWait(spawnRate);
            SpawnEnnemy();
        }
    }

    public void SpawnEnnemy()
    {
        GameObject ennemyPrefab = ennemyPrefabs[Random.Range(0, ennemyPrefabs.Count)];
        Instantiate(ennemyPrefab, randomPos() + transform.position, Quaternion.identity);
    }

    Vector3 randomPos()
    {
        //return (new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f))).normalized;
        float signA = Random.Range(0, 2) * 2 - 1;
        float signB = Random.Range(0, 2) * 2 - 1;
        return signA * Random.Range(10f, 20f) * playerTransform.up + signB * Random.Range(10f, 20f) * playerTransform.right + 10f * playerTransform.up;
    }


}

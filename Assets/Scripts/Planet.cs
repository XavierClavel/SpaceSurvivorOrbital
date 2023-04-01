using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MyBox;
using DG.Tweening;

public class Planet : MonoBehaviour
{
    [HideInInspector] public Vector3 position;
    [HideInInspector] public float gravityRadius;
    [HideInInspector] public float size;
    [HideInInspector] public float mass = 10;
    public static Planet instance;
    [SerializeField] GameObject ennemyPrefab;

    private void Awake()
    {
        position = transform.position;
        gravityRadius = GetComponent<SphereCollider>().radius;
        size = GetComponentInChildren<SphereCollider>().radius;
        instance = this;
    }

    void Start()
    {
        StartCoroutine("SpawnEnnemies");
    }

    IEnumerator SpawnEnnemies()
    {
        while (true)
        {
            yield return Helpers.GetWait(5f);
            SpawnEnnemy();
        }
    }

    void SpawnEnnemy()
    {

        Instantiate(ennemyPrefab, randomPos() * (size + 10f) + transform.position, Quaternion.identity);
    }

    Vector3 randomPos()
    {
        return (new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f))).normalized;
    }


}

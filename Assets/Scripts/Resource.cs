using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

enum type { violet, orange, green }

public class Resource : Breakable
{
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Slider healthBar;

    [Header("Parameters")]
    [SerializeField] Vector2Int dropInterval = new Vector2Int(2, 5);
    int _health;

    public int health
    {
        get { return _health; }
        set
        {
            _health = value;
            healthBar.value = value;
            if (!healthBar.gameObject.activeInHierarchy) healthBar.gameObject.SetActive(true);
            //SoundManager.instance.PlaySfx(transform, sfx.playerHit);
            if (value <= 0) Break();
        }
    }

    protected override void Start()
    {
        base.Start();
        _health = maxHealth;
        healthBar.maxValue = _health;
        healthBar.value = _health;
    }

    public override void Hit(int damage, status effect, bool critical)
    {
        base.Hit(damage, effect, critical);
        health -= damage;
    }

    void Break()
    {
        SoundManager.instance.PlaySfx(transform, sfx.breakResource);
        int nbItemsToSpawn = dropInterval.getRandom();
        for (int i = 0; i < nbItemsToSpawn; i++)
        {
            Instantiate(itemPrefab, randomPos() + transform.position, Quaternion.identity);
        }
        Destroy(gameObject);

    }



    Vector3 randomPos()
    {
        Transform playerTransform = PlayerController.instance.transform;
        float signA = Helpers.getRandomSign();
        float signB = Helpers.getRandomSign();
        return signA * Helpers.getRandomFloat(1.5f) * Vector2.up + signB * Helpers.getRandomFloat(1.5f) * Vector2.right;
    }

    public void StartMining() { }
    public void StopMining() { }
}

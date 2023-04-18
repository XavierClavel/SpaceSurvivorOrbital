using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum type { violet, orange, green }

public class Resource : MonoBehaviour
{
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Slider healthBar;
    [Header("Parameters")]
    [SerializeField] int _health = 150;
    [SerializeField] Vector2Int dropInterval = new Vector2Int(2, 5);

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

    private void Start()
    {
        healthBar.maxValue = _health;
        healthBar.value = _health;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "RedBullet")
        {
            int damageTaken = PlayerController.DamageResource();
            health -= damageTaken;
            DamageDisplayHandler.DisplayDamage(damageTaken, transform.position);
        }
    }


    void Break()
    {
        SoundManager.instance.PlaySfx(transform, sfx.breakResource);
        int nbItemsToSpawn = Random.Range(dropInterval.x, dropInterval.y + 1);
        for (int i = 0; i < nbItemsToSpawn; i++)
        {
            Instantiate(itemPrefab, randomPos() + transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }



    Vector3 randomPos()
    {
        Transform playerTransform = PlayerController.instance.transform;
        float signA = Random.Range(0, 2) * 2 - 1;
        float signB = Random.Range(0, 2) * 2 - 1;
        return signA * Random.Range(0f, 1.5f) * Vector2.up + signB * Random.Range(0f, 1.5f) * Vector2.right;
    }
}

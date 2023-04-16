using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum type { violet, orange, green }

public class Resource : MonoBehaviour
{
    [SerializeField] GameObject itemPrefab;
    float health = 3;
    float maxRange = 4f;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "RedBullet")
        {
            health -= PlayerController.DamageResource();
            if (health <= 0) Break();
        }
    }


    void Break()
    {
        SoundManager.instance.PlaySfx(transform, sfx.breakResource);
        int nbItemsToSpawn = Random.Range(2, 5);
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
        return signA * Random.Range(0f, 4f) * Vector2.up + signB * Random.Range(0f, 4f) * Vector2.right;
    }
}

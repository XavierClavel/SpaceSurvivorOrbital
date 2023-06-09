using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

enum type { violet, orange, green }

public class Resource : MonoBehaviour
{
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Slider healthBar;
    [SerializeField] SpriteRenderer spriteOverlay;

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

    private void Awake()
    {
        ObjectManager.dictObjectToResource.Add(gameObject, this);
    }

    private void Start()
    {
        healthBar.maxValue = _health;
        healthBar.value = _health;
    }

    public void Hit(int damage)
    {
        health -= damage;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(spriteOverlay.DOColor(Color.white, 0.1f));
        sequence.Append(spriteOverlay.DOColor(Helpers.color_whiteTransparent, 0.1f));
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

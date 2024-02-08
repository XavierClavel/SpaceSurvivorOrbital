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

    [Header("Animation")]
    public Sprite damagedSprite;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isDestroy = false;
    private new GameObject spriteOverlay;
    public Collider2D myCollider;


    [Header("Parameters")]
    Vector2Int dropInterval;
    int _health;

    public int health
    {
        get { return _health; }
        set
        {
            _health = value;
            healthBar.value = value;
            if (!healthBar.gameObject.activeInHierarchy && !isDestroy) healthBar.gameObject.SetActive(true);
            if (value >= 0 && value < maxHealth) spriteRenderer.sprite = damagedSprite;
            if (value <= 0 && !isDestroy) Break();
        }
    }
    
    public static List<IResourceListener> listeners = new List<IResourceListener>();

    public static void registerListener(IResourceListener listener)
    {
        listeners.TryAdd(listener);
    }

    public static void unregisterListener(IResourceListener listener)
    {
        listeners.TryRemove(listener);
    }

    protected override void Start()
    {
        base.Start();
        _health = maxHealth;
        healthBar.maxValue = _health;
        healthBar.value = _health;

        dropInterval = baseDamage;

        ObjectManager.registerEggSpawned();

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        Transform childTransform = transform.Find("Sprite Overlay");
        spriteOverlay = childTransform.gameObject;
        myCollider = GetComponent<Collider2D>();
        gameObject.tag = Vault.tag.Resource;

    }

    public override void Hit(HitInfo hitInfo)
    {
        base.Hit(hitInfo);
        SoundManager.PlaySfx(transform, key: "Eggs");
        health -= hitInfo.damage;
    }
    
    protected override void StackHit(int damage, HashSet<status> elements)
    {
        SoundManager.PlaySfx(transform, key: "Eggs");
        health -= damage;
    }

    void Break()
    {
        SoundManager.PlaySfx(transform, key: "Eggs_Destroy");
        int nbItemsToSpawn = dropInterval.getRandom();
        for (int i = 0; i < nbItemsToSpawn; i++)
        {
            var go = Instantiate(itemPrefab, randomPos() + transform.position, Quaternion.identity);
        }
        ResourcesAttractor.ForceUpdate();
        ObjectManager.registerEggDestroyed();
        animator.enabled = true;
        isDestroy = true;
        healthBar.gameObject.SetActive(false);
        myCollider.enabled = false;
        listeners.ForEach(it => it.onResourceDestroyed(this));
        Destroy(spriteOverlay);


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

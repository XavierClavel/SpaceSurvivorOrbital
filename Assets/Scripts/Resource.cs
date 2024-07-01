using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public enum type {orange, green, blue }

public class Resource : Breakable
{
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Slider healthBar;
    public type resourceType;

    [Header("Animation")]
    public Sprite damagedSprite;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isDestroy = false;
    private new GameObject spriteOverlay;
    public Collider2D myCollider;
    public ParticleSystem hitPS;
    
    [HideInInspector] public Vector2Int dropInterval;
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
    
    protected override void Start()
    {
        base.Start();
        _health = maxHealth;
        healthBar.maxValue = _health;
        healthBar.value = _health;

        dropInterval = baseDamage;

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        Transform childTransform = transform.Find("Sprite Overlay");
        spriteOverlay = childTransform.gameObject;
        myCollider = GetComponent<Collider2D>();
        gameObject.tag = Vault.tag.Resource;
        
        EventManagers.eggs.dispatchEvent(v => v.onEggSpawned(this));
    }

    public override void Hit(HitInfo hitInfo)
    {
        base.Hit(hitInfo);
        SoundManager.PlaySfx(transform, key: "Eggs");
        hitPS.Play();
        health -= hitInfo.getDamage();
    }
    
    protected override void StackHit(int damage, HashSet<status> elements)
    {
        //SoundManager.PlaySfx(transform, key: "Eggs");
        health -= damage;
    }

    void Break()
    {
        SoundManager.PlaySfx(transform, key: "Eggs_Destroy");
        animator.enabled = true;
        isDestroy = true;
        healthBar.gameObject.SetActive(false);
        myCollider.enabled = false;
        EventManagers.eggs.dispatchEvent(v => v.onEggDestroyed(this));
        Debug.Log("event dispatched");
        Destroy(spriteOverlay);
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using System.Net;
using UnityEditor;
using static Vault.key;
using UnityEngine.Timeline;

public class Ennemy : Breakable
{

    const float shakeDuration = 0.2f;
    const float shakeIntensity = 0.5f;

    protected PlayerController player;
    [SerializeField] protected Slider healthBar;
    [SerializeField] protected Rigidbody2D rb;
    protected Vector2 distanceToPlayer;
    protected Vector2 directionToPlayer;
    static protected WaitForSeconds wait;
    static protected WaitForSeconds waitStateStep;
    private static WaitForSeconds waitFire;
    float speedMultiplier = 1f;
    private float damageMultiplier = 1f;
    protected bool isImmuneToEffects = false;
    protected bool isImmuneToKnockback = false;

    private int fireDamageRemaining = 0;

    public GameObject marked;
    public bool isMarked = false;
    public float markedMultiplier = 1f;

    [Header("Knockback Parameters")]
    [SerializeField] Vector3 startDeformationScale = new Vector3(-0.5f, 0.1f, 1f);
    [SerializeField] float deformationDuration = 0.1f;

    private Vector3 originalScale;
    private Vector3 deformationScale;

    private ParticleSystem firePs;
    private ParticleSystem lightningPs;
    private ParticleSystem icePs;

    [Header("Parameters")]
    [SerializeField] protected float speed = 1f;
    [SerializeField] protected float fleeSpeed = 2f;
    [SerializeField] protected float cooldown = 1f;
    [SerializeField] protected float range = 5f;
    [SerializeField] protected float stateStep = 0.5f;

    int _health;
    protected int health
    {
        get { return _health; }
        set
        {
            value = Helpers.CeilInt(value, maxHealth);
            //if (value == maxHealth && healthBar.gameObject.activeInHierarchy) healthBar.gameObject.SetActive(false);
            //else if (!healthBar.gameObject.activeInHierarchy) healthBar.gameObject.SetActive(true);
            _health = value;
            healthBar.value = value;
            //SoundManager.instance.PlaySfx(transform, sfx.playerHit);
            if (value <= 0) Death();
        }
    }

    protected bool knockback = false;
    private float remainingLightning = 0f;
    private float remainingIce = 0f;

    WaitForSeconds knockbackWindow;
    [HideInInspector] public bool doUseKillPs = true;
    

    protected override void Start()
    {
        base.Start();

        rb.gravityScale = 0;
        rb.drag = 10f;
        speed = baseSpeed.getRandom();

        player = PlayerController.instance;
        StressTest.nbEnnemies++;
        _health = maxHealth;
        healthBar.maxValue = _health;
        healthBar.value = _health;

        wait = Helpers.getWait(cooldown);
        waitStateStep = Helpers.getWait(stateStep);
        waitFire = Helpers.getWait(ConstantsData.fireStep);

        //TODO : static initalizer

        originalScale = transform.localScale;
        deformationScale = startDeformationScale;

        ObjectManager.dictObjectToEnnemy.Add(gameObject, this);

        animator = GetComponent<Animator>();
        playerDir = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();

        Transform childTransform = transform.Find("Sprite Overlay");
        gameObject.tag = Vault.tag.Ennemy;

        if (childTransform != null)
        {
            SpriteRenderer childSpriteRenderer = childTransform.GetComponent<SpriteRenderer>();
            
        }

        firePs =  Instantiate(ObjectManager.instance.firePS);
        firePs.transform.SetParent(transform);
        firePs.transform.localPosition = Vector3.zero;
        
        lightningPs =  Instantiate(ObjectManager.instance.lightningPS);
        lightningPs.transform.SetParent(transform);
        lightningPs.transform.localPosition = Vector3.zero;

        icePs = Instantiate(ObjectManager.instance.icePS);
        icePs.transform.SetParent(transform);
        icePs.transform.localPosition = Vector3.zero;
    }
    
    


    protected virtual void FixedUpdate()
    {
        distanceToPlayer = player.transform.position - transform.position;
        directionToPlayer = distanceToPlayer.normalized;
    }

    protected void Move(Vector2 direction, float speed)
    {
        rb.AddForce(direction * speed);
    }

    protected void Move(Vector2 direction)
    {
        Move(direction, speed * speedMultiplier);
    }

    public override void Hit(HitInfo hitInfo)
    {
        if (isMarked) { hitInfo.applyDamageMultiplier(damageMultiplier * markedMultiplier); }
        else { hitInfo.applyDamageMultiplier(damageMultiplier); }

        base.Hit(hitInfo);
        
        healthChange value = hitInfo.critical ? healthChange.critical : healthChange.hit;
        if (hitInfo.damage != 0)
        {
            DamageDisplayHandler.DisplayDamage(hitInfo.damage, transform.position, value);
            StopCoroutine(nameof(ApplyDeformationOverTime));
            StartCoroutine(nameof(ApplyDeformationOverTime));
            health -= hitInfo.damage;
            if (health <= 0) return;
        }

        ApplyEffects(hitInfo);
        if (hitInfo.effect.Contains(status.lightning)) return; //no knockback if ennemy is stun
        ApplyKnockback(hitInfo.knockback);
    }
    
    
    
    protected override void StackHit(int damage, HashSet<status> elements)
    {
        ApplyEffects(elements);
        health -= damage;
        DamageDisplayHandler.DisplayStackedDamage(gameObject, damage);
        StopCoroutine(nameof(ApplyDeformationOverTime));
        StartCoroutine(nameof(ApplyDeformationOverTime));
    }
    
    

    public void ApplyKnockback(int knockbackForce, bool overrideImmunity = false)
    {
        if (isImmuneToKnockback && !overrideImmunity) return;
        if (knockbackForce == 0) return;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.AddForce((transform.position - player.transform.position).normalized * knockbackForce);
    }

    public void ApplyForce(Vector2 force)
    {
        rb.AddForce(force);
    }


    public void HealSelf(int amount)
    {
        health += amount;
        DamageDisplayHandler.DisplayDamage(amount, transform.position, healthChange.heal);
    }


    [SerializeField] ParticleSystem onDestroyPS;

    protected virtual void Death()
    {
        //SoundManager.PlaySfx(transform, key: "Ennemy_Destroy");
        player.AddEnnemyScore(cost);
        StressTest.nbEnnemies--;
        ObjectManager.dictObjectToEnnemy.TryRemove(gameObject);
        ObjectManager.unregisterHitable(gameObject);
        PowerHuntersMark.markedEnemies.TryRemove(this);

        if (isMarked)
        {
            PowerHuntersMark.isMarked = true;
        }

        onDeath();
        ShakeManager.Shake(shakeIntensity, shakeDuration);
        EventManagers.ennemies.dispatchEvent(v => v.onEnnemyDeath(this));
        Destroy(gameObject);
    }

    protected virtual void onDeath() {}




    #region elementalEffects

    void ApplyEffects(HitInfo hitInfo)
    {
        ApplyEffects(hitInfo.effect);
    }
    
    void ApplyEffects(HashSet<status> effects)
    {
        foreach (var effect in effects)
        {
            ApplyEffect(effect);
        }
    }

    public void ApplyEffect(status element)
    {
        if (isImmuneToEffects) return;
        switch (element)
        {
            case status.none:
                break;

            case status.lightning:
                ApplyLightning();
                break;

            case status.ice:
                ApplyIce();
                break;

            case status.fire:
                ApplyFire();
                break;
        }
    }

    void ApplyFire()
    {
        if (fireDamageRemaining <= 0f) StartCoroutine(nameof(FireDamage));
        else fireDamageRemaining = ConstantsData.fireDuration;
    }

    void ApplyIce()
    {
        if (remainingIce <= 0f) StartCoroutine(nameof(IceEffect));
        else remainingIce = ConstantsData.iceDuration;
    }
    
    void ApplyLightning()
    {
        if (remainingLightning <= 0f) StartCoroutine(nameof(LightningEffect));
        else remainingLightning = ConstantsData.lightningDuration;
    }

    IEnumerator FireDamage()
    {
        fireDamageRemaining = ConstantsData.fireDuration;
        firePs.Play();
        while (fireDamageRemaining > 0)
        {
            yield return waitFire;
            DamageDisplayHandler.DisplayDamage(ConstantsData.fireDamage, transform.position, healthChange.fire);
            health -= (int)(ConstantsData.fireDamage * BonusManager.current.getFireDamageMultiplier());
            fireDamageRemaining--;
        }
        firePs.Stop();
    }

    IEnumerator IceEffect()
    {
        icePs.Play();
        spriteRenderer.color = new Color32(126,171,242,255);
        speedMultiplier = ConstantsData.iceSlowdown;
        damageMultiplier = ConstantsData.iceDamageMultiplier;
        remainingIce = ConstantsData.iceDuration;

        while (remainingIce > 0f)
        {
            yield return Helpers.getWaitFixed();
            remainingIce -= Time.fixedDeltaTime;
        }
        
        speedMultiplier = 1f;
        damageMultiplier = 1f;
        spriteRenderer.color = Color.white;
        icePs.Stop();
    }
    
    IEnumerator LightningEffect()
    {
        lightningPs.Play();
        speedMultiplier = 0f;
        remainingLightning = ConstantsData.lightningDuration;

        while (remainingLightning > 0f)
        {
            yield return Helpers.getWaitFixed();
            remainingLightning -= Time.fixedDeltaTime;
            rb.velocity = Vector2.zero;
        }
        
        speedMultiplier = 1f;
        lightningPs.Stop();
    }
    
    

    #endregion

    private GameObject playerDir;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isMovingRight = true;
    private static readonly int IsMovingRight = Animator.StringToHash("IsMovingRight");
    private float order;


    private void Update()
    {

        if (player == null)
        {
            return; 
        }

        order = -transform.position.y + 1500;
        spriteRenderer.sortingOrder = (int)order;
        spriteOverlay.sortingOrder = (int)order + 1;

        Vector3 directionToPlayer = (player.transform.position - transform.position);
        Vector3 facingDirection = transform.right;

        float dotProduct = Vector3.Dot(directionToPlayer, facingDirection);
        var position = this.transform.position;

        if (dotProduct > 0 && isMovingRight)
        {
            animator.SetBool(IsMovingRight, false);
            FlipSprite();
        }
        else if (dotProduct < 0 && !isMovingRight)
        {
            animator.SetBool(IsMovingRight, true);
            FlipSprite();
        }
    }
    private void FlipSprite()
    {
        isMovingRight = !isMovingRight;
        spriteRenderer.flipX = !isMovingRight;
        spriteOverlay.flipX = !isMovingRight;
        
    }

    private IEnumerator ApplyDeformationOverTime()
    {
        float elapsedTime = 0f;
        float duration = deformationDuration;
        Vector3 deformationScale = this.deformationScale;
        Vector3 deformation = deformationScale;
        Vector3 startScale = deformationScale;
        Vector3 endScale = deformationScale + deformation;

        transform.localScale = originalScale;

        while (elapsedTime < duration)
        {
            deformationScale = Vector3.Lerp(startScale, endScale, elapsedTime / duration);
            transform.localScale = originalScale + deformationScale;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = originalScale;
        deformationScale = startDeformationScale;
    }

    public void ReceiveMark(float markMultiplier)
    {
        marked.SetActive(true);
        isMarked = true;
        markedMultiplier = markMultiplier;
    }
}

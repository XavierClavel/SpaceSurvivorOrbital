using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using System.Net;
using UnityEditor;

public class Ennemy : Breakable
{
    
    public UnityEvent onDeath = new UnityEvent();

    protected PlayerController player;
    [SerializeField] Slider healthBar;
    [SerializeField] protected Rigidbody2D rb;
    protected Vector2 distanceToPlayer;
    protected Vector2 directionToPlayer;
    static protected WaitForSeconds wait;
    static protected WaitForSeconds waitStateStep;
    static WaitForSeconds waitIce;
    private static WaitForSeconds waitLightning;
    private static WaitForSeconds waitFire;
    float speedMultiplier = 1f;

    private int fireDamageRemaining = 0;

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
    [SerializeField] protected float attackSpeed = 0.5f;
    [SerializeField] protected float range = 5f;
    [SerializeField] protected float stateStep = 0.5f;
    int _health;
    int health
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

    
#region API

    public static List<IEnnemyListener> listeners = new List<IEnnemyListener>();

    public static void registerListener(IEnnemyListener listener)
    {
        listeners.Add(listener);
    }

    public static void unregisterListener(IEnnemyListener listener)
    {
        listeners.Remove(listener);
    }

#endregion

    protected override void Start()
    {
        base.Start();

        rb.gravityScale = 0;
        rb.drag = 1f;
        speed = baseSpeed.getRandom();

        player = PlayerController.instance;
        StressTest.nbEnnemies++;
        _health = maxHealth;
        healthBar.maxValue = _health;
        healthBar.value = _health;

        wait = Helpers.getWait(attackSpeed);
        waitStateStep = Helpers.getWait(stateStep);
        waitLightning = Helpers.getWait(ConstantsData.lightningDuration);
        waitIce = Helpers.getWait(ConstantsData.iceDuration);
        waitFire = Helpers.getWait(ConstantsData.fireStep);

        //TODO : static initalizer

        originalScale = transform.localScale;
        deformationScale = startDeformationScale;

        ObjectManager.dictObjectToEnnemy.Add(gameObject, this);

        animator = GetComponent<Animator>();
        playerDir = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();

        Transform childTransform = transform.Find("Sprite Overlay");
        cameraTransform = Camera.main.transform;
        gameObject.tag = Vault.tag.Ennemy;

        if (childTransform != null)
        {
            SpriteRenderer childSpriteRenderer = childTransform.GetComponent<SpriteRenderer>();
            overlaydSpriteRenderer = childSpriteRenderer.GetComponent<SpriteRenderer>();
            
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
        base.Hit(hitInfo);
        
        healthChange value = hitInfo.critical ? healthChange.critical : healthChange.hit;
        if (hitInfo.damage != 0)
        {
            DamageDisplayHandler.DisplayDamage(hitInfo.damage, transform.position, value);
            StopCoroutine(nameof(ApplyDeformationOverTime));
            StartCoroutine(nameof(ApplyDeformationOverTime));
            health -= hitInfo.damage;
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
    
    

    public void ApplyKnockback(int knockbackForce)
    {
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




    protected virtual void Death()
    {
        //SoundManager.PlaySfx(transform, key: "Ennemy_Destroy");
        player.AddEnnemyScore(cost);
        StressTest.nbEnnemies--;
        ObjectManager.dictObjectToEnnemy.Remove(gameObject);
        ObjectManager.unregisterHitable(gameObject);
        
        onDeath.Invoke();
        StartCoroutine(nameof(ShakeCoroutine));
        
        listeners.ForEach(it => it.onEnnemyDeath(transform.position));
        
    }




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

    void ApplyEffect(status element)
    {
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
        bool isOnFire = fireDamageRemaining > 0;
        fireDamageRemaining = ConstantsData.fireDuration;
        if (!isOnFire)
        {
            StartCoroutine(nameof(FireDamage));
        }
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
        firePs.Play();
        while (fireDamageRemaining > 0)
        {
            yield return waitFire;
            DamageDisplayHandler.DisplayDamage(ConstantsData.fireDamage, transform.position, healthChange.fire);
            health -= ConstantsData.fireDamage;
            fireDamageRemaining--;
        }
        firePs.Stop();
    }

    IEnumerator IceEffect()
    {
        Debug.Log("Ice Go");
        icePs.Play();
        spriteRenderer.color = new Color32(126,171,242,255);
        speedMultiplier = ConstantsData.iceSlowdown;

        while (remainingIce > 0f)
        {
            yield return Helpers.getWaitFixed();
            remainingIce -= Time.fixedDeltaTime;
        }
        
        speedMultiplier = 1f;
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
    private SpriteRenderer overlaydSpriteRenderer;
    private bool isMovingRight = true;

    private void Update()
    {
        originalCameraPosition = cameraTransform.localPosition;

        if (player == null)
        {
            return; 
        }

        Vector3 directionToPlayer = (player.transform.position - transform.position);
        Vector3 facingDirection = transform.right;

        float dotProduct = Vector3.Dot(directionToPlayer, facingDirection);

        if (dotProduct > 0 && isMovingRight)
        {
            animator.SetBool("IsMovingRight", false);
            FlipSprite();
        }
        else if (dotProduct < 0 && !isMovingRight)
        {
            animator.SetBool("IsMovingRight", true);
            FlipSprite();
        }
        
    }
    private void FlipSprite()
    {
        isMovingRight = !isMovingRight;
        spriteRenderer.flipX = !isMovingRight;
        overlaydSpriteRenderer.flipX = !isMovingRight;
        
    }

    Transform cameraTransform;
    private Vector3 originalCameraPosition;

    [Header("CameraShake")]
    public float shakeDuration = 0.1f;
    public float shakeIntensity = 1f;
    public float negativeRange = -0.1f;
    public float positiveRange = 0.1f;

    private IEnumerator ShakeCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            float randomX = Random.Range(negativeRange, positiveRange) * shakeIntensity;
            float randomY = Random.Range(negativeRange, positiveRange) * shakeIntensity;
            Vector3 randomPoint = originalCameraPosition + new Vector3(randomX, randomY, 0f);
            cameraTransform.localPosition = randomPoint;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        cameraTransform.localPosition = originalCameraPosition;
        Destroy(gameObject);

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

}

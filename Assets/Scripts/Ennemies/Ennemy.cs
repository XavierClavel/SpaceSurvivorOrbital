using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using System.Net;

public class Ennemy : Breakable
{
    public UnityEvent onDeath = new UnityEvent();

    protected PlayerController player;
    [SerializeField] Slider healthBar;
    [SerializeField] protected Rigidbody2D rb;
    SoundManager soundManager;
    protected Vector2 distanceToPlayer;
    protected Vector2 directionToPlayer;
    static protected WaitForSeconds wait;
    static protected WaitForSeconds waitStateStep;
    static WaitForSeconds waitPoison;
    static WaitForSeconds waitPoisonDamage;
    static WaitForSeconds waitFire;
    static WaitForSeconds waitFireDamage;
    static WaitForSeconds waitIce;
    float speedMultiplier = 1f;

    [Header("Knockback Parameters")]
    [SerializeField] float knockbackForce = 5f;
    [SerializeField] float knockbackRiseDuration = 0.10f;
    [SerializeField] float knockbackPlateauDuration = 0.10f;
    [SerializeField] float knockbackFallDuration = 0.10f;
    [SerializeField] float knockbackStunDuration = 0.10f;
    [SerializeField] Vector3 startDeformationScale = new Vector3(-0.5f, 0.1f, 1f);
    [SerializeField] float deformationDuration = 0.1f;

    private Vector3 originalScale;
    private Vector3 deformationScale;

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

    WaitForSeconds knockbackWindow;


    protected override void Start()
    {
        base.Start();

        soundManager = SoundManager.instance;
        player = PlayerController.instance;
        StressTest.nbEnnemies++;
        _health = maxHealth;
        healthBar.maxValue = _health;
        healthBar.value = _health;

        wait = Helpers.GetWait(attackSpeed);
        waitStateStep = Helpers.GetWait(stateStep);
        waitPoison = Helpers.GetWait(PlayerManager.poisonDuration);
        waitPoisonDamage = Helpers.GetWait(PlayerManager.poisonPeriod);
        waitIce = Helpers.GetWait(PlayerManager.iceDuration);
        knockbackWindow = Helpers.GetWait(knockbackRiseDuration);

        //TODO : static initalizer

        originalScale = transform.localScale;
        deformationScale = startDeformationScale;

        ObjectManager.dictObjectToEnnemy.Add(gameObject, this);

        animator = GetComponent<Animator>();
        playerDir = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();

        Transform childTransform = transform.Find("Sprite Overlay");
        cameraTransform = Camera.main.transform;

        if (childTransform != null)
        {
            SpriteRenderer childSpriteRenderer = childTransform.GetComponent<SpriteRenderer>();
            overlaydSpriteRenderer = childSpriteRenderer.GetComponent<SpriteRenderer>();
            
        }
    }


    protected virtual void FixedUpdate()
    {
        distanceToPlayer = player.transform.position - transform.position;
        directionToPlayer = distanceToPlayer.normalized;
    }

    protected void Move(Vector2 direction, float speed)
    {
        rb.MovePosition(rb.position + (direction +0.05f*direction.perpendicular()) * (Time.fixedDeltaTime * speed * speedMultiplier));
    }

    protected void Move(Vector2 direction)
    {
        Move(direction, speed);
    }

    public override void Hit(int damage, status effect, bool critical)
    {
        base.Hit(damage, effect, critical);
        healthChange value = critical ? healthChange.critical : healthChange.hit;
        if (damage != 0)
        {
            DamageDisplayHandler.DisplayDamage(damage, transform.position, value);
            StartCoroutine(ApplyDeformationOverTime(deformationScale,deformationDuration));
            health -= damage;
        }

        switch (effect)
        {
            case status.none:
                break;

            case status.poison:
                StartCoroutine(nameof(PoisonEffect));
                break;

            case status.ice:
                StartCoroutine(nameof(IceEffect));
                break;

            case status.fire:
                StartCoroutine(nameof(FireEffect));
                break;
        }
        ApplyKnockback();
    }

    public void ApplyKnockback()
    {
        OnKnockbackStart();
        Sequence sequence = DOTween.Sequence();

        sequence.Append(DOTween.To(() => rb.velocity, x => rb.velocity = x, (Vector2)(transform.position - player.transform.position).normalized * knockbackForce, knockbackRiseDuration));
        sequence.AppendInterval(knockbackPlateauDuration);
        sequence.Append(DOTween.To(() => rb.velocity, x => rb.velocity = x, Vector2.zero, knockbackFallDuration));
        sequence.AppendInterval(knockbackStunDuration);
        sequence.onComplete += OnKnockbackEnd;
    }

    protected virtual void OnKnockbackStart()
    {
        knockback = true;
    }

    protected virtual void OnKnockbackEnd()
    {
        knockback = false;
        rb.velocity = Vector2.zero;
    }

    public void HealSelf(int amount)
    {
        health += amount;
        DamageDisplayHandler.DisplayDamage(amount, transform.position, healthChange.heal);
    }




    protected virtual void Death()
    {
        if (ghostPower == true) {Instantiate(ghost, transform.position, Quaternion.identity);}
        
        PlayerManager.AddEnnemyScore(cost);
        StressTest.nbEnnemies--;
        soundManager.PlaySfx(transform, sfx.ennemyExplosion);
        ObjectManager.dictObjectToEnnemy.Remove(gameObject);
        onDeath.Invoke();
        StartCoroutine("ShakeCoroutine");
        
    }
    [Header("Ghost")] 
    public float spawnChance;
    public bool ghostPower = false;
    public GameObject ghost;

    public void GhostAppear(float ennemySpawnChance)
    {
        if (Random.Range(0f, 1f) <= ennemySpawnChance)
        {
            Debug.Log(ennemySpawnChance);
            Debug.Log(Random.Range(0f, 1f));
            ghostPower = true;
        }
               
    }



    #region elementalEffects

    IEnumerator PoisonEffect()
    {
        StartCoroutine(nameof(PoisonDamage));
        yield return waitPoison;
        StopCoroutine(nameof(PoisonDamage));
    }

    IEnumerator PoisonDamage()
    {
        while (true)
        {
            yield return waitPoisonDamage;
            DamageDisplayHandler.DisplayDamage(PlayerManager.poisonDamage, transform.position, healthChange.poison);
            health -= PlayerManager.poisonDamage;
        }
    }

    IEnumerator FireEffect()
    {
        StartCoroutine(nameof(FireDamage));
        yield return waitFire;
        StopCoroutine(nameof(FireDamage));
    }

    IEnumerator FireDamage()
    {
        while (true)
        {
            yield return waitFireDamage;
            DamageDisplayHandler.DisplayDamage(PlayerManager.fireDamage, transform.position, healthChange.fire);
            health -= PlayerManager.fireDamage;
        }
    }

    IEnumerator IceEffect()
    {
        speedMultiplier = PlayerManager.iceSpeedMultiplier;
        yield return waitIce;
        speedMultiplier = 1f;
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
    public IEnumerator ApplyDeformationOverTime(Vector3 deformation, float duration)
    {
        float elapsedTime = 0f;
        Vector3 startScale = deformationScale;
        Vector3 endScale = deformationScale + deformation;

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

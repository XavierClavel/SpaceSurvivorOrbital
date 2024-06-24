using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public enum playerState { idle, walking, shooting, mining, dashing };
public enum playerDirection { front, left, back, right };


public class PlayerController : MonoBehaviour, IResourcesListener
{
    [Header("References")]
    [SerializeField] FloatingJoystick joystickMove;
    [SerializeField] FloatingJoystick joystickAim;
    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] Animator playerAnimator;

    InteractorHandler interactorHandler;
    [SerializeField] GameObject minerBot;
    [HideInInspector] public Transform attractorTransform;
    [HideInInspector] public static PlayerController instance;
    [HideInInspector] public InputMaster controls;
    [HideInInspector] public playerState _playerState_value = playerState.idle;
    [HideInInspector] public bool reflectsProjectiles = false;

    public ObjectManager objectManager;

    [HideInInspector] public int maxDashes;
    [HideInInspector] public float dashCooldown;
    [HideInInspector] public bool dashAvailable;
    public float dashSpeed;
    public float dashDuration;
    [HideInInspector] public bool isDashing = false;
    private int currentDashes;
    private float dashTime;
    private float dashCooldownTime;
    private bool isDashingInput;
    private Vector2 dashDirection;


    [SerializeField] private CinemachineVirtualCamera cinemachineCamera;
    
    [Header("Particle Systems")]
    [SerializeField] ParticleSystem boostAttack;
    [SerializeField] ParticleSystem boostSpeed;
    [SerializeField] public ParticleSystem healPlayer;
    [SerializeField] public ParticleSystem upGreen;
    [SerializeField] public ParticleSystem upYellow;
    [SerializeField] public ParticleSystem upBlue;
    [SerializeField] private ParticleSystem shieldUp;
    [SerializeField] private ParticleSystem shieldDown;
    [SerializeField] public ParticleSystem dashPS;

    private const float boostedSpeed = 6f;
    [HideInInspector]
    public playerState state
    {
        get
        {
            return _playerState_value;
        }
        set
        {
            if (_playerState_value == value) return;
            _playerState_value = value;
            animator.SetInteger(Vault.animatorParameter.State, (int)value);
        }
    }
    playerDirection _aimDirection_value = playerDirection.front;
    [HideInInspector]
    public playerDirection _aimDirection
    {
        get
        {
            return _aimDirection_value;
        }
        set
        {
            if (_aimDirection_value == value) return;
            _aimDirection_value = value;
            animator.SetInteger(Vault.animatorParameter.AimDirection, (int)value);
        }
    }

    playerDirection _walkDirection_value = playerDirection.front;
    playerDirection _walkDirection
    {
        set
        {
            if (_walkDirection_value == value) return;
            playerSprite.flipX = value == playerDirection.left;
            _walkDirection_value = value;
            
            animator.SetInteger(Vault.animatorParameter.WalkDirection, (int)value);
        }
    }

    Rigidbody2D rb;
    float speed;
    Vector2 moveAmount;
    Vector2 smoothMoveVelocity;
    Transform cameraTransform;
    Vector3 targetMoveAmount;
    SoundManager soundManager;
    Vector2 prevMoveDir = Vector2.zero;
    LayoutManager healthBar;
    int _health;
    int _resurrection;
    [Header("Transforms")]
    [SerializeField] Animator animator;
    public Transform pointerFront;
    public Transform pointerBack;
    public Transform pointerRight;
    public Transform pointerLeft;
    public Vector2 moveDir;
    private Camera cam;
    private bool playerDead;

    [Header("UI")] 
    [SerializeField] private TextMeshProUGUI soulsDisplay;
    [SerializeField] private TextMeshProUGUI souls2Display;
    [SerializeField] private TextMeshProUGUI resurrectionDisplay;
    EventSystem eventSystem;
    [SerializeField] private Transform camTarget;

    public float smallSize = 3.0f;
    public float largeSize = 5.0f;

    [Header("Bonus")]
    [SerializeField] GameObject spaceshipIndicator;

    public static bool isPlayingWithGamepad = false;

    private bool isWeaponOverridingRotation = false;


    //Player parameters
    public int maxHealth;
    private float baseSpeed;
    private bool invulnerable = false;
    private float damageMultiplier;

    [HideInInspector] public float speedMultiplier = 1f;

    //Wait
    WaitForSeconds invulnerabilityFrameDuration;
    public Freezer freezer;
    [SerializeField] ParticleSystem hit;

    [HideInInspector] public Vector2 aimVector;

    [SerializeField] protected SpriteRenderer spriteOverlay;
    private WaitForSeconds footstepsWait;
    private int shieldsAmount = 0;

    private BonusManager bonusManager;
    
    public int shields
    {
        get { return shieldsAmount; }
        private set
        {
            healthBar.SetShieldsAmount(value);
            shieldsAmount = value;
            SoundManager.PlaySfx(transform, key: "Shield_Hit");
            if (value == 0 && health == 0) Death();
        }
    }

    public int health
    {
        get { return _health; }
        private set
        {
            healthBar.SetAmount(value);
            _health = value;
            SoundManager.PlaySfx(transform, key: "Player_Hit");
            if (value <= 0) Death();
        }
    }

    public int resurrection
    {
        get { return _resurrection; }
        private set
        {
            resurrectionDisplay.SetText("x" + value.ToString());
            _resurrection = value;
        }
    }

    private void takeDamage(int damage)
    {
        for (int i = 0; i < damage; i++)
        {
            var listeners = EventManagers.player.getListeners();
            bool avoidDamage = false;
            foreach (var eventListener in listeners)
            {
                avoidDamage = avoidDamage || eventListener.onPlayerHit(shieldsAmount > 0);
            }
            if (avoidDamage) continue;
            takeDamage();
        }
    }
    
    /**
     * Removes one shield, if there are none left removes one heart.
     */
    private void takeDamage()
    {
        if (!playerDead)
        {
            hit.Play();
            freezer.FreezeScreen(0.5f);
        }


        if (shieldsAmount > 0)
        {
            shields--;
        }
        else
        {
            health--;

        }
    }

    private int _souls;

    public int souls
    {
        get { return _souls; }
        private set
        {
            soulsDisplay.SetText(value.ToString());
            souls2Display.SetText(value.ToString());

            _souls = value;
        }
    }

    #region interface
    
    public static void Hurt(Vector2Int amount)
    {
        Hurt((float)amount.getRandom());
    }

    /**
     * Called when the player is dealt damage.
     */
    public static void Hurt(float amount)
    {
        if (instance.invulnerable) return;
        if (instance.interactorHandler.currentInteractor.isDamageAbsorbed()) return;
        instance.takeDamage((int)(amount));
        instance.OnHitOverlay();
        ShakeManager.Shake(playerShakeIntensity, playerShakeDuration);
        instance.StartCoroutine(nameof(InvulnerabilityFrame));
    }

    public static void Heal(int amount = 1)
    {
        SoundManager.PlaySfx(instance.transform, key: "Heal_Player");
        instance.healPlayer.Play();
        instance.health += amount;
    }
    
    public void OnHitOverlay()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(spriteOverlay.DOColor(Color.white, 0.1f));
        sequence.Append(spriteOverlay.DOColor(Helpers.color_whiteTransparent, 0.1f));
    }

    public IEnumerator InvulnerabilityFrame()
    {
        invulnerable = true;
        yield return invulnerabilityFrameDuration;
        invulnerable = false;
    }

    public void onResourcePickup(resourceType type)
    {
        Color color = type == resourceType.green ? Color.green : Color.yellow;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(spriteOverlay.DOColor(color, 0.05f));
        sequence.Append(spriteOverlay.DOColor(Helpers.color_whiteTransparent, 0.1f));
    }

    public void AddEnnemyScore(int value)
    {
        souls += value;
    }

    #endregion

    void OnEnable()
    {
        //controls.Enable();
    }

    void OnDisable()
    {
        if (!Helpers.isPlatformAndroid()) controls.Disable();
    }

    private void Awake()
    {
        instance = this;
        bonusManager = new BonusManager();
        interactorHandler = GetComponent<InteractorHandler>();

        if (Helpers.isPlatformAndroid())
        {
            joystickMove.gameObject.SetActive(true);
            joystickAim.gameObject.SetActive(true);
        }
    }


    void Start()
    {
        cam = Camera.main;
        if (Helpers.isPlatformAndroid()) Application.targetFrameRate = 60;
        
        playerAnimator.runtimeAnimatorController = DataSelector.getSelectedCharacter().getAnimatorController();
        playerSprite.sprite = DataSelector.getSelectedCharacter().getIcon();
        
        rb = GetComponent<Rigidbody2D>();
        cameraTransform = cinemachineCamera.transform;
        soundManager = SoundManager.instance;
        healthBar = ObjectManager.instance.healthBar;
        interactorHandler.Initialize(PlayerManager.weaponPrefab, pointerFront, true);
        
        bonusManager.applyCharacterEffect();

        foreach (EquipmentHandler equipmentHandler in PlayerManager.equipments)
        {
            equipmentHandler.Activate(bonusManager);
        }

        foreach (ArtefactHandler artefactHandler in PlayerManager.artefacts)
        {
            artefactHandler.Activate();
        }
        
        foreach(PowerHandler powerHandler in PlayerManager.powers) {
            powerHandler.Activate();
        }

        maxHealth = PlayerManager.playerData.character.maxHealth + bonusManager.getBonusMaxHealth();
        shieldsAmount = bonusManager.getBonusShield();
        int currentHealth = maxHealth - PlayerManager.damageTaken;
        baseSpeed = PlayerManager.playerData.character.baseSpeed * bonusManager.getBonusSpeed();
        speedMultiplier = 1f;
        damageMultiplier = PlayerManager.playerData.character.damageMultiplier;
        

        souls = PlayerManager.getSouls();
        resurrection = PlayerManager.getResurrection();

        invulnerabilityFrameDuration = Helpers.getWait(ConstantsData.invulenerabilityFrame);

        _health = maxHealth;
        healthBar.Setup(maxHealth, currentHealth);
        healthBar.SetupShields(shieldsAmount);

        if (!Helpers.isPlatformAndroid()) InitializeControls();
        
        DebugManager.DisplayValue("MaxHealth", maxHealth.ToString());

        footstepsWait = new WaitForSeconds(ConstantsData.audioFootstepInterval);
        StartCoroutine(nameof(PlayFootsteps));

        playerDead = false;
        
        EventManagers.resources.registerListener(this);

        currentDashes = maxDashes;

    }


    private IEnumerator PlayFootsteps()
    {
        while (true)
        {
            if (state == playerState.walking) SoundManager.PlaySfx(transform, "Footstep");
            yield return footstepsWait;
        }
    }


    public void SpawnMinerBot()
    {
        Instantiate(minerBot);
    }

    public static void SwitchInput(bool newValue)
    {
        if (instance == null) return;
        isPlayingWithGamepad = newValue;
        if (isPlayingWithGamepad)
        {
            UnityEngine.Cursor.visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        }
    }


    void InitializeControls()
    {
        controls = new InputMaster();

        controls.Player.Reload.performed += _ =>
        {
            Camera.main.orthographicSize = (Camera.main.orthographicSize == smallSize) ? largeSize : smallSize;
        };

        controls.Player.MouseAimActive.started += ctx =>
        {
            TutoManager.instance.Click();
            Aim();
            interactorHandler.StartAction();
        };
        controls.Player.MouseAimActive.canceled += ctx => interactorHandler.StopAction();

        controls.Player.Dash.performed += ctx => isDashingInput = true;
        controls.Player.Dash.canceled += ctx => isDashingInput = false;

        controls.Enable();
    }


    public void OverrideWeaponRotation()
    {
        isWeaponOverridingRotation = true;
    }

    public void ReleaseWeaponRotation()
    {
        isWeaponOverridingRotation = false;
    }

    void Update()
    {
        Move();
        Aim();
        attractorTransform.position = transform.position;
        originalCameraPosition = cameraTransform.localPosition;
    }
    void Move()
    {
        if (isDashing) return; // Ignore movement input during dash

        moveDir = Helpers.isPlatformAndroid() ? joystickMove.Direction : controls.Player.Move.ReadValue<Vector2>();

        if (moveDir.sqrMagnitude > 1) moveDir = moveDir.normalized;
        if (moveDir != Vector2.zero)
        {
            prevMoveDir = moveDir;
            state = playerState.walking;
        }
        else state = playerState.idle;

        if (dashAvailable && isDashingInput && Time.time >= dashCooldownTime)
        {
            StartDash();
        }

        targetMoveAmount = moveDir * (baseSpeed * speedMultiplier);
        moveAmount = Vector2.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, 0.10f);

        pointerFront.position = transform.position;
    }

    void StartDash()
    {
        if (currentDashes > 0)
        {
            isDashing = true;
            dashTime = Time.time + dashDuration;
            dashCooldownTime = Time.time + 0.4f;
            dashDirection = prevMoveDir;
            state = playerState.dashing;
            currentDashes--;
        } 
    }

    void Dash()
    {
        if (Time.time < dashTime)
        {
            dashPS.Play();
            moveAmount = dashDirection * dashSpeed;
        }
        else
        {
            isDashing = false;
            state = playerState.idle;

            if (currentDashes == 0)
            {
                StartCoroutine(nameof(DashReload));
            }
        }
    }
    public IEnumerator DashReload()
    {
        objectManager.onDash();
        yield return new WaitForSeconds(dashCooldown);
        currentDashes = maxDashes;
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            Dash();
        }

        Vector2 localMove = moveAmount * Time.fixedDeltaTime;
        _walkDirection = angleToDirection(Vector2.SignedAngle(localMove, Vector2.down) + 180f);
        rb.MovePosition(rb.position + localMove);
    }

    Vector2 getGamepadAimInput()
    {
        var value = controls.Player.Aim.ReadValue<Vector2>();
        camTarget.position = (Vector2)pointerFront.position + Mathf.Clamp(value.sqrMagnitude * 16f, 0f, 16f) * 0.1f * value.normalized;
        return value;
    }

    Vector2 getMouseAimInput()
    {
        Vector2 mousePos = controls.Player.MousePosition.ReadValue<Vector2>();
        Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);
        var position = pointerFront.position;
        Vector2 direction = (worldPos - position);
        Vector3 normalizedDirection = direction.normalized;
        float magnitude = direction.sqrMagnitude;
        camTarget.position = position + Mathf.Clamp(magnitude, 0f, 16f) * 0.1f * normalizedDirection;
        return normalizedDirection;
    }

    void Aim()
    {
        cinemachineCamera.transform.rotation = Quaternion.identity;
        
        if (Helpers.isPlatformAndroid()) aimVector = joystickAim.Direction;
        else aimVector = isPlayingWithGamepad ? getGamepadAimInput() : getMouseAimInput();

        if (aimVector != Vector2.zero && !isWeaponOverridingRotation)
        {
            float angle = Vector2.SignedAngle(Vector2.up, aimVector) + 90f;
            pointerFront.rotation = Quaternion.Euler(0f, 0f, angle);
            pointerRight.rotation = Quaternion.Euler(0f, 0f, angle + 90f);
            pointerBack.rotation = Quaternion.Euler(0f, 0f, angle + 180f);
            pointerLeft.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }

        _aimDirection = angleToDirection(Vector2.SignedAngle(aimVector, Vector2.down) + 180f);

        if (!isPlayingWithGamepad && !Helpers.isPlatformAndroid()) return;

        if (aimVector == Vector2.zero && interactorHandler.action) interactorHandler.StopAction();
        else if (aimVector != Vector2.zero && !interactorHandler.action) interactorHandler.StartAction();
    }



    playerDirection angleToDirection(float angle)
    {
        return angle switch
        {
            > 315f => playerDirection.back,
            > 225f => playerDirection.left,
            > 135f => playerDirection.front,
            > 45f => playerDirection.right,
            _ => playerDirection.back
        };
    }

    void Death()
    {
        bool avoidDeath = false;
        var listeners = EventManagers.player.getListeners();
        foreach (var eventListener in listeners)
        {
            avoidDeath = avoidDeath || eventListener.onPlayerDeath();
        }
        if (avoidDeath) return;
        cinemachineCamera.enabled = false;
        playerDead = true;
        ObjectManager.DisplayLoseScreen();
    }
    
    public void OnClick()
    {
        PauseMenu.instance.ResumeGame();
        SceneManager.LoadScene(Vault.scene.TitleScreen);
    }

    private Vector3 originalCameraPosition;

    [Header("CameraShake")]
    private const float playerShakeDuration = 0.5f;
    private const float playerShakeIntensity = 3f;
    

    private void OnDestroy()
    {
        PlayerManager.setSouls(souls);
        EventManagers.player.resetListeners();
        EventManagers.resources.unregisterListener(this);
    }

    public static void ApplySpeedBoost()
    {
        instance.boostSpeed.Play();
        SoundManager.PlaySfx(instance.transform, key: "Boost_Player");
        instance.baseSpeed = boostedSpeed;
    }
    public static void RemoveSpeedBoost()
    {
        instance.boostSpeed.Stop();
        instance.baseSpeed = PlayerManager.playerData.character.baseSpeed;
    }
    public static void ApplyStrengthBoost()
    {
        instance.boostAttack.Play();
        SoundManager.PlaySfx(instance.transform, key: "Boost_Player");
        instance.damageMultiplier *= 1.2f;
    }
    public static void RemoveStrengthBoost()
    {
        instance.boostAttack.Stop();
        instance.damageMultiplier /= 1.2f;
    }

    public static float getDamageMultiplier() => instance.damageMultiplier;

    public static void onShieldUp()
    {
        instance.shieldUp.Play();
    }

    public static void onShieldDown()
    {
        instance.shieldDown.Play();
        instance.shieldUp.Stop();
        instance.shieldUp.gameObject.SetActive(false);
        instance.shieldUp.gameObject.SetActive(true);
    }

}

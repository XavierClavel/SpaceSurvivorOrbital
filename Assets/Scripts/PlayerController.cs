using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public enum playerState { idle, walking, shooting, mining };
public enum playerDirection { front, left, back, right };


public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] FloatingJoystick joystickMove;
    [SerializeField] FloatingJoystick joystickAim;
    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] Animator playerAnimator;

    CharacterHandler characterHandler;
    InteractorHandler interactorHandler;
    [SerializeField] GameObject minerBot;
    [HideInInspector] public Transform attractorTransform;
    [HideInInspector] public static PlayerController instance;
    [HideInInspector] public InputMaster controls;
    [HideInInspector] public playerState _playerState_value = playerState.idle;
    [HideInInspector] public bool reflectsProjectiles = false;
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
    [SerializeField] Animator animator;
    public Transform pointerFront;
    public Transform pointerBack;
    public Transform pointerRight;
    public Transform pointerLeft;
    Vector2 moveDir;

    [Header("UI")] 
    [SerializeField] private TextMeshProUGUI soulsDisplay;
    [SerializeField] ResourceLayoutManager layoutManagerOrange;
    [SerializeField] ResourceLayoutManager layoutManagerGreen;
    public LayoutManager bulletsLayoutManager;
    EventSystem eventSystem;
    [SerializeField] GameObject button;

    private bool playerControlled = true;

    public float smallSize = 3.0f;
    public float largeSize = 5.0f;

    [Header("Bonus")]
    [SerializeField] public GameObject radar;
    [SerializeField] GameObject spaceshipIndicator;

    public static bool isPlayingWithGamepad = false;

    private bool isWeaponOverridingRotation = false;


    //Player parameters
    private int maxHealth;
    private float baseSpeed;
    private float damageResistanceMultiplier;
    private bool invulnerable = false;

    //Wait
    WaitForSeconds invulnerabilityFrameDuration;

    [HideInInspector] public Vector2 aimVector;

    [SerializeField] protected SpriteRenderer spriteOverlay;
    private WaitForSeconds footstepsWait;
    private int shieldsAmount = 0;
    private int bonusStockAmount = 0;
    
    public int shields
    {
        get { return shieldsAmount; }
        private set
        {
            healthBar.SetShieldsAmount(value);
            shieldsAmount = value;
            SoundManager.PlaySfx(transform, key: "Shield_Hit");
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

    private void takeDamage(int damage)
    {
        for (int i = 0; i < damage; i++)
        {
            if (shieldsAmount > 0)
            {
                shields--;
            }
            else
            {
                health--;
            }
        }
    }

    private int _souls;

    public int souls
    {
        get { return _souls; }
        private set
        {
            soulsDisplay.SetText(value.ToString());
            _souls = value;
        }
    }

    #region interface

    public static void Hurt(float amount)
    {
        if (instance.invulnerable) return;
        if (instance.interactorHandler.currentInteractor.isDamageAbsorbed()) return;
        instance.takeDamage((int)(amount));
        instance.OnHitOverlay();
        instance.StartCoroutine(nameof(ShakeCoroutine));
        instance.StartCoroutine(nameof(InvulnerabilityFrame));
    }

    public static void Heal(int amount)
    {
        instance.health += amount;
    }
    
    public void OnHitOverlay()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(spriteOverlay.DOColor(Color.white, 0.1f));
        sequence.Append(spriteOverlay.DOColor(Helpers.color_whiteTransparent, 0.1f));
    }

    public static void Hurt(Vector2Int amount)
    {
        Hurt((float)amount.getRandom());
    }

    IEnumerator InvulnerabilityFrame()
    {
        invulnerable = true;
        yield return invulnerabilityFrameDuration;
        invulnerable = false;
    }

    public void IncreaseOrange()
    {
        layoutManagerOrange.AddResource();
        Sequence sequence = DOTween.Sequence();
        sequence.Append(spriteOverlay.DOColor(Color.yellow, 0.05f));
        sequence.Append(spriteOverlay.DOColor(Helpers.color_whiteTransparent, 0.1f));
    }

    public void IncreaseGreen()
    {
        layoutManagerGreen.AddResource();
        Sequence sequence = DOTween.Sequence();
        sequence.Append(spriteOverlay.DOColor(Color.green, 0.05f));
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
        interactorHandler = GetComponent<InteractorHandler>();

        //playerSprite = characterHandler.spriteRenderer;
        //playerAnimator = characterHandler.animator;

        if (Helpers.isPlatformAndroid())
        {
            joystickMove.gameObject.SetActive(true);
            joystickAim.gameObject.SetActive(true);
        }
    }

    public void AddBonusStock(int amount)
    {
        bonusStockAmount = amount;
    }


    void Start()
    {
        if (Helpers.isPlatformAndroid()) Application.targetFrameRate = 60;

        radar.SetActive(PlayerManager.activateRadar);
        spaceshipIndicator.SetActive(PlayerManager.activateShipArrow);

        maxHealth = PlayerManager.playerData.character.maxHealth;
        int currentHealth = PlayerManager.currentHealth ?? maxHealth;
        baseSpeed = PlayerManager.playerData.character.baseSpeed;
        setSpeed(1f);
        damageResistanceMultiplier = PlayerManager.playerData.character.damageMultiplier;

        rb = GetComponent<Rigidbody2D>();
        cameraTransform = Camera.main.transform;
        soundManager = SoundManager.instance;
        healthBar = ObjectManager.instance.healthBar;

        souls = PlayerManager.getSouls();

        invulnerabilityFrameDuration = Helpers.GetWait(ConstantsData.invulenerabilityFrame);

        _health = maxHealth;

        healthBar.Setup(maxHealth, currentHealth);

        interactorHandler.Initialize(PlayerManager.weaponPrefab, pointerFront, true);
        
        foreach(PowerHandler powerHandler in PlayerManager.powers) {
            powerHandler.Activate();
        }

        foreach (EquipmentHandler equipmentHandler in PlayerManager.equipments)
        {
            equipmentHandler.Activate();
        }
        
        layoutManagerOrange.Setup(PlayerManager.playerData.resources.maxOrange + bonusStockAmount, ConstantsData.resourcesFillAmount, resourceType.orange);
        layoutManagerGreen.Setup(PlayerManager.playerData.resources.maxGreen + bonusStockAmount, ConstantsData.resourcesFillAmount, resourceType.green);

        layoutManagerOrange.FillNSliders(PlayerManager.amountOrange);
        layoutManagerGreen.FillNSliders(PlayerManager.amountGreen);
        
        if (!Helpers.isPlatformAndroid()) InitializeControls();
        
        DebugManager.DisplayValue("MaxHealth", maxHealth.ToString());

        footstepsWait = new WaitForSeconds(ConstantsData.audioFootstepInterval);
        StartCoroutine(nameof(PlayFootsteps));
        
    }


    private IEnumerator PlayFootsteps()
    {
        while (true)
        {
            //if (state == playerState.walking) SoundManager.PlaySfx(transform, "Footstep");
            yield return footstepsWait;
        }
    }

    public void SetupShields(int amount)
    {
        shieldsAmount = amount;
        healthBar.SetupShields(amount);
    }


    public void setSpeed(float speedMultiplier)
    {
        speed = baseSpeed * speedMultiplier;
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
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    public void debug_GiveResources(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            layoutManagerGreen.AddResource();
        }
        for (int i = 0; i < amount; i++)
        {
            layoutManagerOrange.AddResource();
        }

    }


    void InitializeControls()
    {
        controls = new InputMaster();

        controls.Player.Reload.performed += _ =>
        {
            Camera.main.orthographicSize = (Camera.main.orthographicSize == smallSize) ? largeSize : smallSize;
        };

        controls.Player.Pause.performed += _ => PauseMenu.instance.PauseGame();

        controls.Player.MouseAimActive.started += ctx =>
        {
            Aim();
            interactorHandler.StartAction();
        };
        controls.Player.MouseAimActive.canceled += ctx => interactorHandler.StopAction(); ;

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
        //SoundManager.PlaySfx(transform, key: "Footstep_grass");
        if (Helpers.isPlatformAndroid()) moveDir = joystickMove.Direction;
        else moveDir = controls.Player.Move.ReadValue<Vector2>();

        if (moveDir.sqrMagnitude > 1) moveDir = moveDir.normalized;
        if (moveDir != Vector2.zero)
        {
            prevMoveDir = moveDir;
            state = playerState.walking;
        }
        else state = playerState.idle;
        targetMoveAmount = moveDir * speed;

        moveAmount = Vector2.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, 0.10f);
    }

    Vector2 getGamepadAimInput()
    {
        return controls.Player.Aim.ReadValue<Vector2>();
    }

    Vector2 getMouseAimInput()
    {
        Vector2 mousePos = controls.Player.MousePosition.ReadValue<Vector2>();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 direction = (worldPos - transform.position);
        Debug.DrawRay(transform.position, direction, Color.red);
        return direction.normalized;
    }

    void Aim()
    {
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


    void FixedUpdate()
    {
        if (!playerControlled) return;

        Vector2 localMove = moveAmount * Time.fixedDeltaTime;
        _walkDirection = angleToDirection(Vector2.SignedAngle(localMove, Vector2.down) + 180f);

        rb.MovePosition(rb.position + localMove);
        pointerFront.position = transform.position;
        cameraTransform.position = new Vector3(transform.position.x, transform.position.y, cameraTransform.position.z);

    }

    void Death()
    {
        if (!PlayerEventsManager.onPlayerDeath()) return;
        ObjectManager.DisplayLoseScreen();
    }
    
    public void OnClick()
    {
        PauseMenu.instance.ResumeGame();
        SceneManager.LoadScene(Vault.scene.TitleScreen);
    }

    private Vector3 originalCameraPosition;

    [Header("CameraShake")]
    public float shakeDuration = 0.1f;
    public float shakeIntensity = 0.05f;
    public float negativeRange = -0.1f;
    public float positiveRange = 0.1f;

    private IEnumerator ShakeCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            // Générez des valeurs aléatoires pour le déplacement en 2D
            float randomX = Random.Range(negativeRange, positiveRange) * shakeIntensity;
            float randomY = Random.Range(negativeRange, positiveRange) * shakeIntensity;

            // Appliquez le déplacement à la caméra en 2D
            Vector3 randomPoint = originalCameraPosition + new Vector3(randomX, randomY, 0f);
            cameraTransform.localPosition = randomPoint;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Remettez la caméra à sa position d'origine après le screenshake
        cameraTransform.localPosition = originalCameraPosition;
    }

    private void OnDestroy()
    {
        PlayerEventsManager.resetListeners();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum playerState { idle, walking, shooting, mining };
public enum playerDirection { front, left, back, right };


public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] FloatingJoystick joystickMove;
    [SerializeField] FloatingJoystick joystickAim;
    [SerializeField] SpriteRenderer playerSprite;
    InteractorHandler interactorHandler;
    [SerializeField] GameObject minerBot;
    [HideInInspector] public Transform attractorTransform;
    [HideInInspector] public static PlayerController instance;
    [HideInInspector] public InputMaster controls;
    [HideInInspector] public playerState _playerState_value = playerState.idle;
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
    public Transform arrowTransform;
    Vector2 moveDir;

    [Header("UI")]
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


    //Player parameters
    private int maxHealth;
    private float baseSpeed;
    private float damageResistanceMultiplier;
    private static bool invulnerable = false;

    //Wait
    WaitForSeconds invulnerabilityFrameDuration;

    [HideInInspector] public status effect;
    [HideInInspector] public Vector2 aimVector;


    int health
    {
        get { return _health; }
        set
        {
            healthBar.SetAmount(value);
            _health = value;
            SoundManager.instance.PlaySfx(transform, sfx.playerHit);
            if (value <= 0) Death();
        }
    }

    #region interface
    public static void Hurt(float amount)
    {
        if (invulnerable) return;
        instance.health -= (int)(amount * (1 - instance.damageResistanceMultiplier));
        instance.StartCoroutine(nameof(InvulnerabilityFrame));
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
    }

    public void IncreaseGreen()
    {
        layoutManagerGreen.AddResource();
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

        if (Helpers.isPlatformAndroid())
        {
            joystickMove.gameObject.SetActive(true);
            joystickAim.gameObject.SetActive(true);
        }
    }


    void Start()
    {
        if (Helpers.isPlatformAndroid()) Application.targetFrameRate = 60;

        radar.SetActive(PlayerManager.activateRadar);
        spaceshipIndicator.SetActive(PlayerManager.activateShipArrow);

        maxHealth = PlayerManager.playerData.character.maxHealth;
        baseSpeed = PlayerManager.playerData.character.baseSpeed;
        setSpeed(1f);
        damageResistanceMultiplier = PlayerManager.playerData.character.damageResistanceMultiplier;

        effect = PlayerManager.statusEffect;

        rb = GetComponent<Rigidbody2D>();
        cameraTransform = Camera.main.transform;
        soundManager = SoundManager.instance;
        healthBar = ObjectManager.instance.healthBar;

        layoutManagerOrange.Setup(PlayerManager.playerData.resources.maxOrange, ObjectManager.instance.gameData.fillAmountOrange, resourceType.orange);
        layoutManagerGreen.Setup(PlayerManager.playerData.resources.maxGreen, ObjectManager.instance.gameData.fillAmountGreen, resourceType.green);

        layoutManagerOrange.FillNSliders(PlayerManager.amountOrange);
        layoutManagerGreen.FillNSliders(PlayerManager.amountGreen);

        invulnerabilityFrameDuration = Helpers.GetWait(PlayerManager.invulnerabilityFrameDuration);

        _health = maxHealth;

        healthBar.Setup(maxHealth);

        interactorHandler.Initialize(PlayerManager.weaponPrefab, PlayerManager.toolPrefab, ObjectManager.instance.armTransform, true);

        if (!Helpers.isPlatformAndroid()) InitializeControls();

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

    public void debug_ActivateRadar()
    {
        radar.SetActive(true);
    }


    void InitializeControls()
    {
        controls = new InputMaster();

        controls.Player.Reload.performed += context =>
        {
            Camera.main.orthographicSize = (Camera.main.orthographicSize == smallSize) ? largeSize : smallSize;
        };

        controls.Player.Pause.performed += context => PauseMenu.instance.PauseGame();

        controls.Player.MouseAimActive.started += ctx =>
        {
            Aim();
            interactorHandler.StartAction();
        };
        controls.Player.MouseAimActive.canceled += ctx => interactorHandler.StopAction(); ;

        controls.Enable();
    }

    void Update()
    {
        Move();
        Aim();
        attractorTransform.position = transform.position;
    }

    void Move()
    {
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
        return direction.normalized;
    }

    void Aim()
    {
        if (Helpers.isPlatformAndroid()) aimVector = joystickAim.Direction;
        else aimVector = isPlayingWithGamepad ? getGamepadAimInput() : getMouseAimInput();

        if (aimVector != Vector2.zero)
        {
            float angle = Vector2.SignedAngle(Vector2.up, aimVector) + 90f;
            arrowTransform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        _aimDirection = angleToDirection(Vector2.SignedAngle(aimVector, Vector2.down) + 180f);

        if (!isPlayingWithGamepad && !Helpers.isPlatformAndroid()) return;

        if (aimVector == Vector2.zero && interactorHandler.action) interactorHandler.StopAction();
        else if (aimVector != Vector2.zero && !interactorHandler.action) interactorHandler.StartAction();
    }



    playerDirection angleToDirection(float angle)
    {
        switch (angle)
        {
            case > 315f:
                return playerDirection.back;
            case > 225f:
                return playerDirection.left;
            case > 135f:
                return playerDirection.front;
            case > 45f:
                return playerDirection.right;
            default:
                return playerDirection.back;
        }
    }


    void FixedUpdate()
    {
        if (!playerControlled) return;

        Vector2 localMove = moveAmount * Time.fixedDeltaTime;
        _walkDirection = angleToDirection(Vector2.SignedAngle(localMove, Vector2.down) + 180f);

        rb.MovePosition(rb.position + localMove);
        arrowTransform.position = transform.position;
        cameraTransform.position = new Vector3(transform.position.x, transform.position.y, cameraTransform.position.z);

    }

    void Death()
    {
        ObjectManager.DisplayLoseScreen();
    }
    public void OnClick()
    {
        PauseMenu.instance.ResumeGame();
        SceneManager.LoadScene(Vault.scene.TitleScreen);
    }
}

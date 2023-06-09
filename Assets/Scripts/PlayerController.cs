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
    [SerializeField] GameObject minerBot;
    public Bullet bulletPrefab;
    [SerializeField] Slider reloadSlider;
    [HideInInspector] public Transform attractorTransform;
    [HideInInspector] public static PlayerController instance;
    [HideInInspector] public InputMaster controls;
    [HideInInspector] public playerState _playerState_value = playerState.idle;
    PlayerInput playerInput;
    [HideInInspector]
    public playerState _playerState
    {
        get
        {
            return _playerState_value;
        }
        set
        {
            if (_playerState_value == value) return;
            _playerState_value = value;
            animator.SetInteger("state", (int)value);
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
            animator.SetInteger("aimDirection", (int)value);
        }
    }

    playerDirection _walkDirection_value = playerDirection.front;
    playerDirection _walkDirection
    {
        set
        {
            if (_walkDirection_value == value) return;
            _walkDirection_value = value;
            animator.SetInteger("walkDirection", (int)value);
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
    bool aiming = false;
    [SerializeField] Slider healthBar;
    float _health;
    [HideInInspector] public GameObject spaceship;
    [SerializeField] Animator animator;
    [HideInInspector] public bool hasWon = false;
    public SpriteRenderer arrowMouse;
    public Transform arrowTransform;
    Vector2 moveDir;
    private Weapon weapon;
    private Tool tool;

    [Header("UI")]
    [SerializeField] ResourceLayoutManager layoutManagerViolet;
    [SerializeField] ResourceLayoutManager layoutManagerOrange;
    [SerializeField] ResourceLayoutManager layoutManagerGreen;
    public LayoutManager bulletsLayoutManager;
    [SerializeField] GameObject youLooseScreen;
    EventSystem eventSystem;
    [SerializeField] GameObject button;

    [Header("Debug")]
    [SerializeField] bool giveResources = false;

    private bool mouseAiming = false;
    private bool playerControlled = true;
    private bool shootWhileAiming;

    public float smallSize = 3.0f;
    public float largeSize = 5.0f;

    [Header("Bonus")]
    [SerializeField] public GameObject radar;
    [SerializeField] public bool isRadarActive;
    [SerializeField] GameObject spaceshipIndicator;
    [SerializeField] public bool isArrowShipActive;

    //ResourceParameters
    private int maxViolet;
    private int maxOrange;
    private int maxGreen;

    public static int fillAmountViolet;
    public static int fillAmountOrange;
    public static int fillAmountGreen;

    public static bool isPlayingWithGamepad = false;


    //Player parameters
    private int maxHealth;
    private float baseSpeed;
    private float damageResistanceMultiplier;
    private static bool invulnerable = false;

    //Wait
    WaitForSeconds bulletReloadWindow;
    WaitForSeconds invulnerabilityFrameDuration;



    float bulletReloadTime;
    float speed_aimingDemultiplier;
    [HideInInspector] public status effect;



    float health
    {
        get { return _health; }
        set
        {
            _health = value;
            healthBar.value = value;
            SoundManager.instance.PlaySfx(transform, sfx.playerHit);
            if (value <= 0) Death();
        }
    }

    #region interface
    public static void Hurt(float amount)
    {
        if (invulnerable) return;
        instance.health -= amount * (1 - instance.damageResistanceMultiplier);
        instance.StartCoroutine("InvulnerabilityFrame");
    }

    IEnumerator InvulnerabilityFrame()
    {
        invulnerable = true;
        yield return invulnerabilityFrameDuration;
        invulnerable = false;
    }

    public static void ActivateSpaceship()
    {
        instance.spaceship.GetComponent<CircleCollider2D>().enabled = true;
        instance.spaceshipIndicator.SetActive(true);
        instance.spaceshipIndicator.GetComponent<ObjectIndicator>().target = instance.spaceship.transform;
    }

    public void IncreaseViolet()
    {
        layoutManagerViolet.AddResource();
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
        controls.Disable();
    }

    private void Awake()
    {

        instance = this;
        playerInput = GetComponent<PlayerInput>();
        SwitchInput();

    }

    void Start()
    {
        ActivateSpaceship();

        if (PlayerManager.activateRadar) { radar.SetActive(true); } else { radar.SetActive(false); }
        if (PlayerManager.activateShipArrow) { spaceshipIndicator.SetActive(true); } else { spaceshipIndicator.SetActive(false); }

        maxViolet = PlayerManager.maxViolet;
        maxOrange = PlayerManager.maxOrange;
        maxGreen = PlayerManager.maxGreen;

        fillAmountViolet = PlayerManager.fillAmountViolet;
        fillAmountOrange = PlayerManager.fillAmountOrange;
        fillAmountGreen = PlayerManager.fillAmountGreen;

        maxHealth = PlayerManager.maxHealth;
        baseSpeed = PlayerManager.baseSpeed;
        damageResistanceMultiplier = PlayerManager.damageResistanceMultiplier;


        bulletReloadTime = PlayerManager.bulletReloadTime;
        speed_aimingDemultiplier = PlayerManager.speed_aimingDemultiplier;
        effect = PlayerManager.statusEffect;

        weapon = Instantiate(PlayerManager.weapon, transform.position, Quaternion.identity);
        weapon.transform.SetParent(transform);
        weapon.reloadSlider = reloadSlider;

        tool = Instantiate(PlayerManager.tool, transform.position, Quaternion.identity);
        tool.transform.SetParent(transform);
        tool.Initialize(new Vector2(PlayerManager.toolRange, PlayerManager.toolRange), PlayerManager.toolPower, PlayerManager.toolReloadTime);
        //TODO? Vector2 for toolRange in PlayerManager

        rb = GetComponent<Rigidbody2D>();
        cameraTransform = Camera.main.transform;
        soundManager = SoundManager.instance;

        layoutManagerViolet.Setup(maxViolet, fillAmountViolet, resourceType.violet);
        layoutManagerOrange.Setup(maxOrange, fillAmountOrange, resourceType.orange);
        layoutManagerGreen.Setup(maxGreen, fillAmountGreen, resourceType.green);

        bulletReloadWindow = Helpers.GetWait(bulletReloadTime);
        invulnerabilityFrameDuration = Helpers.GetWait(PlayerManager.invulnerabilityFrameDuration);

        _health = maxHealth;

        healthBar.maxValue = maxHealth;
        healthBar.value = health;

        arrowMouse.enabled = false;




        InitializeControls();
        if (giveResources) debugGiveResources(50);

    }

    public void SpawnMinerBot()
    {
        Instantiate(minerBot);
    }

    public static void SwitchInput()
    {
        if (instance == null) return;
        isPlayingWithGamepad = instance.playerInput.currentControlScheme == "Gamepad";
        if (isPlayingWithGamepad)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            //Cursor.visible = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    void debugGiveResources(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            layoutManagerViolet.AddResource();
        }
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
        controls.Player.Shoot.started += ctx =>
        {
            if (aiming)
            {
                tool.StopMining();
                weapon.StartFiring();
            }

        };
        controls.Player.Shoot.canceled += ctx =>
        {
            weapon.StopFiring();
        };

        controls.Player.Mine.started += ctx =>
        {
            weapon.StopFiring();
            tool.StartMining();
        };

        controls.Player.Mine.canceled += ctx =>
        {
            tool.StopMining();
        };

        controls.Player.Reload.performed += context =>
        {
            //weapon.Reload();
            Camera.main.orthographicSize = (Camera.main.orthographicSize == smallSize) ? largeSize : smallSize;
        };

        controls.Player.Pause.performed += context => PauseMenu.instance.PauseGame();

        controls.Player.MouseAimActive.started += ctx =>
        {
            mouseAiming = true;
        };
        controls.Player.MouseAimActive.canceled += ctx => { mouseAiming = false; };

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
        moveDir = controls.Player.Move.ReadValue<Vector2>();
        if (moveDir.sqrMagnitude > 1) moveDir = moveDir.normalized;
        if (moveDir != Vector2.zero) prevMoveDir = moveDir;
        targetMoveAmount = moveDir * speed;
        moveAmount = Vector2.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, 0.10f);
    }

    Vector2 getGamepadAimInput()
    {
        Vector2 input = controls.Player.Aim.ReadValue<Vector2>();
        if (input == Vector2.zero)
        {
            arrowMouse.enabled = false;
        }
        if (input != Vector2.zero)
        {
            arrowMouse.enabled = true;
        }
        return input;
    }

    Vector2 getMouseAimInput()
    {
        Vector2 input = Vector2.zero;
        if (mouseAiming)
        {

            arrowMouse.enabled = true;
            Vector2 mousePos = controls.Player.MousePosition.ReadValue<Vector2>();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector2 direction = (worldPos - transform.position);
            input = direction.normalized;
        }
        else
        {
            arrowMouse.enabled = false;
        }
        return input;
    }

    void Aim()
    {
        Vector2 input = isPlayingWithGamepad ? getGamepadAimInput() : getMouseAimInput();



        if (input == Vector2.zero)
        {
            input = moveDir == Vector2.zero ? prevMoveDir : moveDir;
            speed = baseSpeed;
            aiming = false;

        }
        else
        {
            aiming = true;
            speed = baseSpeed * speed_aimingDemultiplier;
        }

        float angle = Vector2.SignedAngle(Vector2.up, input);
        arrowTransform.rotation = Quaternion.Euler(0f, 0f, angle);
        _aimDirection = angleToDirection(Vector2.SignedAngle(input, Vector2.down) + 180f);

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

        _playerState = localMove.sqrMagnitude < 1e-4f ? playerState.idle : playerState.walking;

    }

    void Death()
    {
        youLooseScreen.SetActive(true);
        PauseMenu.instance.PauseGame(false);
        if (PlayerController.isPlayingWithGamepad) eventSystem.SetSelectedGameObject(button);
    }
    public void OnClick()
    {
        PauseMenu.instance.ResumeGame();
        SceneManager.LoadScene("SampleScene");
        youLooseScreen.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public enum playerState { idle, walking, shooting, mining };
public enum playerDirection { front, left, back, right };


public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Bullet bulletPrefab;
    [HideInInspector] public static PlayerController instance;
    [HideInInspector] public InputMaster controls;
    [HideInInspector] public playerState _playerState_value = playerState.idle;
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
    WaitForSeconds bulletReloadWindow;
    bool reloading = false;
    bool reloadingMining = false;
    bool shooting = false;
    bool mining = false;
    int violetAmount = 0;
    [HideInInspector] public int orangeAmount = 0;
    [HideInInspector] public int greenAmount = 0;
    [SerializeField] Slider healthBar;
    float _health;
    [SerializeField] GameObject spaceship;
    [SerializeField] GameObject canvas;
    [SerializeField] Animator animator;
    [HideInInspector] public bool hasWon = false;
    public Transform arrowTransform;
    Vector2 moveDir;
    [SerializeField] Weapon weapon;
    [SerializeField] Tool tool;
    [SerializeField] ResourcesAttractor attractor;

    [Header("UI")]
    [SerializeField] ResourceLayoutManager layoutManagerViolet;
    [SerializeField] ResourceLayoutManager layoutManagerOrange;
    [SerializeField] ResourceLayoutManager layoutManagerGreen;
    public LayoutManager bulletsLayoutManager;
    [SerializeField] GameObject spaceshipIndicator;

    bool mouseAiming = false;
    bool playerControlled = true;
    bool shootWhileAiming;

    //ResourceParameters
    int maxViolet;
    int maxOrange;
    int maxGreen;

    public static int fillAmountViolet;
    public static int fillAmountOrange;
    public static int fillAmountGreen;


    //Player parameters
    int maxHealth;
    float baseSpeed;
    float damageResistanceMultiplier;


    float bulletReloadTime;
    float speed_aimingDemultiplier;
    public status effect;


    //Game parameters



    internal float health
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
        instance.health -= amount * (1 - instance.damageResistanceMultiplier);
    }

    public void IncreaseViolet()
    {
        layoutManagerViolet.AddResource();
        violetAmount++;
        if (violetAmount >= fillAmountViolet)
        {
            spaceship.GetComponent<CircleCollider2D>().enabled = true;
            spaceship.GetComponent<SpriteRenderer>().color = Color.white;
            spaceshipIndicator.SetActive(true);
        }
    }

    public void IncreaseOrange()
    {
        layoutManagerOrange.AddResource();
        orangeAmount++;
    }

    public void IncreaseGreen()
    {
        layoutManagerGreen.AddResource();
        greenAmount++;
    }

    #endregion

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    private void Awake()
    {

        instance = this;
        controls = new InputMaster();
        controls.Player.Shoot.started += ctx =>
        {
            shooting = true;
            mining = false;
            if (reloading) return;
            weapon.Shoot();
        };
        controls.Player.Shoot.canceled += ctx =>
        {
            shooting = false;
        };

        controls.Player.Mine.started += ctx =>
        {
            mining = true;
            shooting = false;
            if (reloadingMining) return;
            Mine();
        };
        controls.Player.Mine.canceled += ctx =>
        {
            mining = false;
        };
        controls.Player.Reload.performed += context => { weapon.Reload(); };

        controls.Player.Pause.performed += context => PauseMenu.instance.PauseGame();

        controls.Player.MouseAimActive.started += ctx => { mouseAiming = true; };
        controls.Player.MouseAimActive.canceled += ctx => { mouseAiming = false; };
    }

    IEnumerator Reload()
    {
        reloading = true;
        yield return bulletReloadWindow;
        reloading = false;
        if (shooting) weapon.Shoot();
    }



    IEnumerator ReloadMining()
    {
        reloadingMining = true;
        yield return bulletReloadWindow;
        reloadingMining = false;
        if (mining) Mine();
    }

    void Start()
    {
        maxViolet = PlayerManager.maxViolet;
        maxOrange = PlayerManager.maxOrange;
        maxGreen = PlayerManager.maxGreen;

        fillAmountViolet = PlayerManager.fillAmountViolet;
        fillAmountOrange = PlayerManager.fillAmountOrange;
        fillAmountGreen = PlayerManager.fillAmountGreen;

        //Player parameters
        maxHealth = PlayerManager.maxHealth;
        baseSpeed = PlayerManager.baseSpeed;
        damageResistanceMultiplier = PlayerManager.damageResistanceMultiplier;


        bulletReloadTime = PlayerManager.bulletReloadTime;
        speed_aimingDemultiplier = PlayerManager.speed_aimingDemultiplier;
        effect = PlayerManager.statusEffect;


        Cursor.lockState = CursorLockMode.Confined;
        rb = GetComponent<Rigidbody2D>();
        cameraTransform = Camera.main.transform;
        soundManager = SoundManager.instance;

        layoutManagerViolet.Setup(maxViolet, fillAmountViolet);
        layoutManagerOrange.Setup(maxOrange, fillAmountOrange);
        layoutManagerGreen.Setup(maxGreen, fillAmountGreen);

        bulletReloadWindow = Helpers.GetWait(bulletReloadTime);
        _health = maxHealth;

        healthBar.maxValue = maxHealth;
        healthBar.value = health;
    }

    void Update()
    {
        Move();
        Aim();
    }

    void Move()
    {
        moveDir = controls.Player.Move.ReadValue<Vector2>();
        if (moveDir.sqrMagnitude > 1) moveDir = moveDir.normalized; //TODO : delete
        if (moveDir != Vector2.zero) prevMoveDir = moveDir;
        targetMoveAmount = moveDir * speed;
        moveAmount = Vector2.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, 0.15f);
    }

    void Aim()
    {
        Vector2 input = controls.Player.Aim.ReadValue<Vector2>();
        if (input != Vector2.zero)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (mouseAiming)
        {
            //Cursor.visible = true;
            Vector2 mousePos = controls.Player.MousePosition.ReadValue<Vector2>();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector2 direction = (worldPos - transform.position);
            input = direction.normalized;
        }

        if (input == Vector2.zero)
        {
            input = moveDir == Vector2.zero ? prevMoveDir : moveDir;
            speed = baseSpeed;

        }
        else
        {
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
        if (mining)
        {
            _playerState = playerState.mining;
            rb.MovePosition(rb.position);
            arrowTransform.position = transform.position;
            return;
        }
        _walkDirection = angleToDirection(Vector2.SignedAngle(localMove, Vector2.down) + 180f);



        rb.MovePosition(rb.position + localMove);
        arrowTransform.position = transform.position;
        cameraTransform.position = new Vector3(transform.position.x, transform.position.y, cameraTransform.position.z);

        if (shooting)
        {
            _playerState = playerState.shooting;
            return;
        }
        _playerState = localMove.sqrMagnitude < 1e-4f ? playerState.idle : playerState.walking;


    }



    void Mine()
    {
        StartCoroutine("ReloadMining");
        tool.Hit();
    }

    void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    void Death()
    {
        controls.Disable();
        Restart();
    }


}

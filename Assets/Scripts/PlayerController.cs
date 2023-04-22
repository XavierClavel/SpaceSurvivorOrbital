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
    bool needToReload = false;
    bool needToReloadMining = false;
    bool reloading = false;
    bool reloadingMining = false;
    bool shooting = false;
    bool mining = false;

    [SerializeField] TextMeshProUGUI violetAmountDisplay;
    [SerializeField] TextMeshProUGUI orangeAmountDisplay;
    [SerializeField] TextMeshProUGUI greenAmountDisplay;
    int violetAmount = 0;
    int orangeAmount = 0;
    int greenAmount = 0;
    [SerializeField] Slider healthBar;
    float _health;
    [SerializeField] GameObject spaceship;
    [SerializeField] GameObject canvas;
    [SerializeField] Animator animator;
    [HideInInspector] public bool hasWon = false;
    [SerializeField] Transform arrowTransform;
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


    [Header("Resources parameters")]
    [SerializeField] int maxViolet = 2;
    [SerializeField] int maxOrange = 2;
    [SerializeField] int maxGreen = 2;

    [SerializeField] int fillAmountViolet = 20;
    [SerializeField] int fillAmountOrange = 20;
    [SerializeField] int fillAmountGreen = 20;

    [Header("Player parameters")]
    [SerializeField] int maxHealth = 100;

    [SerializeField] float baseSpeed = 30f;

    [SerializeField] float damageResistanceMultiplier = 0f;


    [Header("Weapon parameters")]
    [SerializeField] Vector2Int baseDamage = new Vector2Int(50, 75);
    [SerializeField] int attackSpeed = 10;
    [SerializeField] float range = 30;

    [SerializeField] float bulletReloadTime = 0.5f;
    [SerializeField] float magazineReloadTime = 2f;

    [SerializeField] float criticalChance = 0.2f;    //between 0 and 1
    [SerializeField] float criticalMultiplier = 2f;  //superior to 1

    [SerializeField] int pierce = 0;
    [SerializeField] float speed_aimingDemultiplier = 0.7f;
    [SerializeField] int magazine = 6;


    [Header("Tool parameters")]
    [SerializeField] int toolPower = 50;
    [SerializeField] float toolRange;
    [Header("Attractor parameters")]
    [SerializeField] float attractorRange = 2.5f;
    [SerializeField] float attractorForce = 2.5f;

    bool mouseAiming = false;
    bool playerControlled = true;

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

    public static void HurtEnnemy(ref int damage, ref bool critical)
    {
        damage = Random.Range(instance.baseDamage.x, instance.baseDamage.y + 1);
        critical = Random.Range(0f, 1f) < instance.criticalChance;
        if (critical) damage = (int)((float)damage * instance.criticalMultiplier);
    }

    public static int DamageResource()
    {
        return instance.toolPower;
    }

    public void IncreaseViolet()
    {
        layoutManagerViolet.AddResource();
        violetAmount++;
        violetAmountDisplay.text = violetAmount.ToString();
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
        orangeAmountDisplay.text = orangeAmount.ToString();
    }

    public void IncreaseGreen()
    {
        layoutManagerGreen.AddResource();
        greenAmount++;
        greenAmountDisplay.text = greenAmount.ToString();
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
        bulletReloadWindow = Helpers.GetWait(bulletReloadTime);
        _health = maxHealth;
        instance = this;
        controls = new InputMaster();
        controls.Player.Shoot.started += ctx =>
        {
            shooting = true;
            mining = false;
            if (reloading) return;
            //if (needToReload) StartCoroutine("Reload");
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
            if (needToReloadMining) StartCoroutine("ReloadMining");
            else Mine();
        };
        controls.Player.Mine.canceled += ctx =>
        {
            mining = false;
        };
        controls.Player.Reload.performed += context => { weapon.Reload(); };

        controls.Player.Pause.performed += context => PauseMenu.instance.PauseGame();

        controls.Player.MouseAimActive.started += ctx => { mouseAiming = true; };
        controls.Player.MouseAimActive.canceled += ctx => { mouseAiming = false; };

        healthBar.maxValue = maxHealth;
        healthBar.value = health;
    }

    IEnumerator Reload()
    {
        reloading = true;
        yield return bulletReloadWindow;
        reloading = false;
        needToReload = false;
        if (shooting) weapon.Shoot();
    }



    IEnumerator ReloadMining()
    {
        reloadingMining = true;
        yield return bulletReloadWindow;
        reloadingMining = false;
        needToReloadMining = false;
        if (mining) Mine();
    }

    void Start()
    {
        weapon.Setup(baseDamage, attackSpeed, range, bulletReloadTime, magazineReloadTime, criticalChance, criticalMultiplier, pierce, speed_aimingDemultiplier, arrowTransform, magazine);
        tool.Resize(toolRange);
        Cursor.lockState = CursorLockMode.Confined;
        rb = GetComponent<Rigidbody2D>();
        cameraTransform = Camera.main.transform;
        soundManager = SoundManager.instance;

        attractor.Setup(attractorRange, attractorForce);

        layoutManagerViolet.Setup(maxViolet, fillAmountViolet);
        layoutManagerOrange.Setup(maxOrange, fillAmountOrange);
        layoutManagerGreen.Setup(maxGreen, fillAmountGreen);
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
        tool.Hit(toolPower);
    }

    void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    IEnumerator LeavePlanet()
    {
        hasWon = true;
        PlayerManager.SaveResources(greenAmount, orangeAmount);
        canvas.SetActive(false);
        playerControlled = false;
        rb.velocity = Vector3.zero;
        WaitForFixedUpdate wait = Helpers.GetWaitFixed;
        while (true)
        {
            rb.AddForce(5 * spaceship.transform.forward);
            yield return wait;
        }
    }

    void Death()
    {
        controls.Disable();
        Restart();
    }


}

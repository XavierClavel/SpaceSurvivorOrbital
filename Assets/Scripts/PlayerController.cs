using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using DG.Tweening;
using MyBox;
using TMPro;
using UnityEngine.UI;

public enum playerState { idle, walking, shooting, mining };
public enum playerDirection { front, left, back, right };


public class PlayerController : MonoBehaviour
{
    [Header("Parameters")]
    public float mouseSensitivityX = 1;
    public float mouseSensitivityY = 1;
    public float jumpForce = 500;
    public controlMode controlScheme = controlMode.Keyboard;
    [Separator("test", true)]

    //[Header("References")]
    [SerializeField] LayerMask groundedMask;
    public GameObject bulletPrefab;
    public static PlayerController instance;
    [HideInInspector] public InputMaster controls;
    //Vector2 direction;
    public enum controlMode { Keyboard, Gamepad };

    public playerState _playerState_value = playerState.idle;
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


    //Local variables
    Vector3 planetPos;
    Rigidbody2D rb;
    //Vector3 gravityVector;
    float planetSize;
    float planetMass;
    CharacterController characterController;
    float speed = runSpeed;
    const float runSpeed = 30f;
    const float shootSpeed = 20f;
    Vector2 moveAmount;
    Vector2 smoothMoveVelocity;
    float verticalLookRotation;
    Transform cameraTransform;
    Vector3 groundNormal = Vector3.up;
    Vector3 distance;
    float inputX;
    float inputY;
    Vector3 targetMoveAmount;
    Rigidbody2D cameraRB;
    Vector2 targetMouseDelta;
    float moveX;
    float moveY;
    SoundManager soundManager;
    [SerializeField] Planet planet;
    public Transform localTransform;
    float cameraOffset_fwd;
    float cameraOffset_up;
    Vector2 prevInput = Vector2.up;
    const float shootWindow_value = 0.5f;
    WaitForSeconds shootWindow = new WaitForSeconds(shootWindow_value);
    WaitForSeconds reloadWindow = new WaitForSeconds(0.4f);
    bool canShoot = true;
    bool canMine = true;
    bool mining = false;
    bool shooting = false;

    [SerializeField] TextMeshProUGUI violetAmountDisplay;
    [SerializeField] TextMeshProUGUI orangeAmountDisplay;
    [SerializeField] TextMeshProUGUI greenAmountDisplay;
    int violetAmount = 0;
    int orangeAmount = 0;
    int greenAmount = 0;
    const int requiredAmount = 10;
    [SerializeField] GameObject minerPrefab;
    bool rotateLeft = false;
    bool rotateRight = false;
    [SerializeField] Slider healthBar;
    float _health = maxHealth;
    [SerializeField] GameObject leaveBeam;
    [SerializeField] GameObject canvas;
    [SerializeField] LineRenderer line;
    [SerializeField] GameObject arrow;
    [SerializeField] Animator animator;
    [HideInInspector] public bool hasWon = false;
    [SerializeField] Transform arrowTransform;

    [Header("Parameters")]
    static float baseDamage = 1f;
    const float maxHealth = 100;
    float damageResistanceMultiplier = 0f;
    float criticalChance = 0.2f;    //between 0 and 1
    float criticalMultiplier = 2f;  //superior to 1
    int pierce = 0;
    float toolPower = 1f;
    float toolRange;
    public float health
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
    const float rotateSpeed = 1.5f;

    bool playerControlled = true;

    #region interface
    public void Hurt(float amount)
    {
        health -= amount * (1 - damageResistanceMultiplier);
    }

    public static float HurtEnnemy()
    {
        return baseDamage * (Random.Range(0f, 1f) < instance.criticalChance ? instance.criticalMultiplier : 1f);
    }

    public static float DamageResource()
    {
        return instance.toolPower;
    }

    public void IncreaseViolet()
    {
        violetAmount++;
        violetAmountDisplay.text = violetAmount.ToString();
        if (violetAmount >= requiredAmount) leaveBeam.SetActive(true);
    }

    public void IncreaseOrange()
    {
        orangeAmount++;
        orangeAmountDisplay.text = orangeAmount.ToString();
    }

    public void IncreaseGreen()
    {
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
        localTransform = transform;
        instance = this;
        controls = new InputMaster();
        controls.Player.Shoot.started += ctx =>
        {
            shooting = true;
            speed = shootSpeed;
            StartCoroutine("Shooting");
            StopCoroutine("Mining");
            StartCoroutine("ReloadMining");
        };
        controls.Player.Shoot.canceled += ctx =>
        {
            shooting = false;
            speed = runSpeed;
            StopCoroutine("Shooting");
            StartCoroutine("Reload");
        };

        controls.Player.Mine.started += ctx =>
        {
            mining = true;
            StartCoroutine("Mining");
            StopCoroutine("Shooting");
            StartCoroutine("Reload");
        };
        controls.Player.Mine.canceled += ctx =>
        {
            mining = false;
            StopCoroutine("Mining");
            StartCoroutine("Reload");
        };

        controls.Player.Shoot.performed += context => Shoot();
        controls.Player.Mine.performed += ctx => Mine();
        controls.Player.Reload.performed += context => Restart();
        controls.Player.Pause.performed += context => PauseMenu.instance.PauseGame();

        controls.Player.RotateLeft.started += ctx =>
        {
            rotateLeft = true;
            rotateRight = false;
        };
        controls.Player.RotateRight.started += ctx =>
        {
            rotateRight = true;
            rotateLeft = false;
        };
        controls.Player.RotateLeft.canceled += ctx => rotateLeft = false;
        controls.Player.RotateRight.canceled += ctx => rotateRight = false;

        if (Gamepad.all.Count > 0) controlScheme = controlMode.Gamepad;
        //rotArrayX = new Vector3[0];
        //controlScheme = controlMode.Keyboard;

        healthBar.maxValue = maxHealth;
        healthBar.value = health;
    }

    IEnumerator Shooting()
    {
        while (true)
        {
            yield return shootWindow;
            Shoot();
        }
    }

    IEnumerator Mining()
    {
        while (true)
        {
            yield return shootWindow;
            Mine();
        }
    }

    IEnumerator Reload()
    {
        canShoot = false;
        yield return reloadWindow;
        canShoot = true;
    }

    IEnumerator ReloadMining()
    {
        canMine = false;
        yield return reloadWindow;
        canMine = true;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody2D>();
        cameraTransform = Camera.main.transform;
        soundManager = SoundManager.instance;
    }

    void Update()
    {

        RotatePlayer();
        Move();
    }

    void Move()
    {

        Vector2 moveDir = controls.Player.Move.ReadValue<Vector2>();
        if (moveDir.sqrMagnitude > 1) moveDir = moveDir.normalized;
        targetMoveAmount = moveDir * speed;
        moveAmount = Vector2.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, 0.15f);


        //transform.Translate(localMove);
    }

    void RotatePlayer()
    {

        if (controlScheme == controlMode.Keyboard)
        {
            inputX = Mouse.current.delta.x.ReadValue();
            inputY = Mouse.current.delta.y.ReadValue();
            targetMouseDelta = Mouse.current.delta.ReadValue();

            verticalLookRotation += inputY * mouseSensitivityY * Time.deltaTime * 10f;
            verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90, 90);

            transform.Rotate(Vector3.up * inputX * mouseSensitivityX * Time.deltaTime * 10f);
        }
        else
        {
            Vector2 input = controls.Player.Rotate.ReadValue<Vector2>();

            if (input == Vector2.zero)
            {
                input = prevInput;
                arrow.SetActive(false);
            }
            else
            {
                prevInput = input;
                arrow.SetActive(true);
            }

            Vector2 localLook = localTransform.TransformVector(new Vector2(input.x, input.y));
            float angle = Vector2.SignedAngle(Vector2.up, input);
            arrowTransform.rotation = Quaternion.Euler(0f, 0f, angle);
            _aimDirection = angleToDirection(Vector2.SignedAngle(input, Vector2.down) + 180f);
        }

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
        //cameraRB.MovePosition(Vector3.Normalize(cameraRB.position + localMove - planetPos) * 50f + planetPos);
        //Debug.Log(transform.position + (cameraOffset_fwd * localTransform.forward) + (cameraOffset_up * localTransform.up));
        cameraTransform.position = new Vector3(transform.position.x, transform.position.y, cameraTransform.position.z);

        if (shooting)
        {
            _playerState = playerState.shooting;
            return;
        }
        _playerState = localMove.sqrMagnitude < 1e-4f ? playerState.idle : playerState.walking;


    }

    void Shoot()
    {
        if (!canShoot) return;
        StartCoroutine("Reload");
        soundManager.PlaySfx(transform, sfx.shoot);
        Bullet bullet = Instantiate(bulletPrefab, transform.position - transform.forward * 6f, arrowTransform.rotation).GetComponentInChildren<Bullet>();
        bullet.pierce = pierce;
    }

    void Mine()
    {
        if (!canMine) return;
        StartCoroutine("ReloadMining");
        soundManager.PlaySfx(transform, sfx.shoot);
        Bullet bullet = Instantiate(minerPrefab, transform.position - transform.forward * 6f, arrowTransform.rotation).GetComponentInChildren<Bullet>();
        bullet.lifetime = 0.3f;
    }

    void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
    void GenerateLine()
    {
        float x;
        float y;
        int segments = 200;
        line.positionCount = 2 + segments;
        float radius = (planetPos - transform.position).magnitude;
        float angle = angle = 4f;

        for (int j = 0; j <= 1 + segments * 0.20; j++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;



            line.SetPosition(j, planetPos + x * transform.forward + y * transform.up);

            angle += (360f / segments);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
        switch (other.tag)
        {
            case "Beam":
                controls.Disable();
                StartCoroutine("LeavePlanet");
                break;
            case "Exit":
                SceneManager.LoadScene("Ship");
                break;

            case "VioletCollectible":
                Destroy(other.gameObject);
                IncreaseViolet();
                break;

            case "GreenCollectible":
                Destroy(other.gameObject);
                IncreaseGreen();
                break;

            case "OrangeCollectible":
                Destroy(other.gameObject);
                IncreaseOrange();
                break;
        }
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
            rb.AddForce(5 * leaveBeam.transform.forward);
            yield return wait;
        }
    }

    void Death()
    {
        controls.Disable();
        Restart();
    }


}

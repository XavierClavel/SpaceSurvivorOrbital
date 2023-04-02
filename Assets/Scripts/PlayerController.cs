using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using DG.Tweening;
using MyBox;
using TMPro;
using UnityEngine.UI;

//public class HeaderDrawer : DecoratorDrawer
//{

//}

public class PlayerController : MonoBehaviour
{
    [Header("Parameters")]
    public float mouseSensitivityX = 1;
    public float mouseSensitivityY = 1;
    public float walkSpeed = 6;
    public float jumpForce = 500;
    public controlMode controlScheme = controlMode.Keyboard;
    [Separator("test", true)]

    //[Header("References")]
    [SerializeField] LayerMask groundedMask;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject shield;
    //public variables
    [HideInInspector] public bool hasBullet = true;
    public static PlayerController instance;
    [HideInInspector] public bool shieldUp = false;
    [HideInInspector] public InputMaster controls;
    //Vector2 direction;
    public enum controlMode { Keyboard, Gamepad };


    //Local variables
    Vector3 planetPos;
    Rigidbody rb;
    //Vector3 gravityVector;
    float planetSize;
    float planetMass;
    CharacterController characterController;
    float runSpeed = 10;
    float speed;
    Vector3 moveAmount;
    Vector3 smoothMoveVelocity;
    float verticalLookRotation;
    Transform cameraTransform;
    bool groundContact = true;
    Vector3 groundNormal = Vector3.up;
    Vector3 distance;
    float inputX;
    float inputY; Vector3 targetMoveAmount;
    Rigidbody cameraRB;
    Vector2 targetMouseDelta;
    float moveX;
    float moveY;
    SoundManager soundManager;
    [SerializeField] Planet planet;
    [SerializeField] Transform localTransform;
    float cameraOffset_fwd;
    float cameraOffset_up;
    Vector2 prevInput = Vector2.up;
    const float shootWindow_value = 0.5f;
    WaitForSeconds shootWindow = new WaitForSeconds(shootWindow_value);
    WaitForSeconds reloadWindow = new WaitForSeconds(0.4f);
    bool canShoot = true;
    bool canMine = true;

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
    const float maxHealth = 5;
    float _health = maxHealth;
    [SerializeField] GameObject leaveBeam;
    [SerializeField] GameObject canvas;
    [SerializeField] LineRenderer line;
    public float health
    {
        get { return _health; }
        set
        {
            _health = value;
            healthBar.value = value;
            if (value <= 0) Death();
        }
    }

    bool playerControlled = true;

    public void IncreaseViolet()
    {
        violetAmount++;
        violetAmountDisplay.text = violetAmount.ToString();
        if (violetAmount == requiredAmount) leaveBeam.SetActive(true);
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
            StartCoroutine("Shooting");
            StopCoroutine("Mining");
            StartCoroutine("ReloadMining");
        };
        controls.Player.Shoot.canceled += ctx =>
        {
            StopCoroutine("Shooting");
            StartCoroutine("Reload");
        };

        controls.Player.Mine.started += ctx =>
        {
            StartCoroutine("Mining");
            StopCoroutine("Shooting");
            StartCoroutine("Reload");
        };
        controls.Player.Mine.canceled += ctx =>
        {
            StopCoroutine("Mining");
            StartCoroutine("Reload");
        };

        controls.Player.Shoot.performed += context => Shoot();
        controls.Player.Mine.performed += ctx => Mine();
        controls.Player.Jump.performed += context => Jump();
        controls.Player.Reload.performed += context => Restart();
        controls.Player.Run.canceled += context => Run();
        controls.Player.Run.canceled += context => Walk();
        //controls.Player.Pause.performed += context => PauseMenu.instance.PauseGame();

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
        cameraRB = Camera.main.GetComponent<Rigidbody>();

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
        rb = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        speed = walkSpeed;
        soundManager = SoundManager.instance;
        //Application.targetFrameRate = -1;
        //QualitySettings.vSyncCount = 0;

        planetPos = planet.position;
        planetSize = planet.size;
        planetMass = planet.mass;

        Vector3 cameraOffset = cameraRB.position - transform.position;
        cameraOffset_fwd = cameraOffset.x;
        cameraOffset_up = cameraOffset.y;
    }

    void Update()
    {
        distance = rb.position - planetPos;
        groundNormal = distance.normalized;
        transform.rotation = Quaternion.FromToRotation(transform.up, groundNormal) * transform.rotation;

        RotatePlayer();
        Move();
        //RotatePlayer();



        rb.angularVelocity = Vector3.zero;
        cameraRB.transform.LookAt(transform, transform.up);

        GenerateLine();
    }

    void Move()
    {

        moveX = controls.Player.Move.ReadValue<Vector2>().x;
        moveY = controls.Player.Move.ReadValue<Vector2>().y;
        Vector3 moveDir = new Vector3(moveX, 0f, moveY);
        //Vector3 moveDir = moveX * transform.right + moveY * transform.forward;
        //direction = controls.Player.Move.ReadValue<Vector2>();
        if (moveDir.sqrMagnitude > 1) moveDir = moveDir.normalized;
        targetMoveAmount = moveDir * speed;
        moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, 0.15f);


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
            inputX = Gamepad.current.rightStick.x.ReadValue() * 10f;
            inputY = Gamepad.current.rightStick.y.ReadValue() * 10f;
            Vector2 input = controls.Player.Rotate.ReadValue<Vector2>();

            if (input == Vector2.zero)
            {
                input = prevInput;
                line.gameObject.SetActive(false);
            }
            else
            {
                prevInput = input;
                line.gameObject.SetActive(true);
            }

            Vector3 localLook = localTransform.TransformVector(new Vector3(input.x, 0f, input.y));
            transform.rotation = Quaternion.LookRotation(localLook, -(planetPos - transform.position).normalized);
        }
        //targetMouseDelta = Mouse.current.delta.ReadValue();

        //input = Vector2.SmoothDamp(input, targetMouseDelta, ref smoothRotateVelocity, mouseSmoothTime);


        //cameraTransform.localEulerAngles = Vector3.left * verticalLookRotation;

        //transform.Rotate(Vector3.up * input.x * mouseSensitivityX * Time.deltaTime * 10f);
        //verticalLookRotation += inputY * mouseSensitivityY * Time.deltaTime * 10f;
        //verticalLookRotation = Mathf.Clamp(verticalLookRotation, -60,60);
        //cameraTransform.localEulerAngles = Vector3.left * verticalLookRotation;

        //angle += 1f;

        //cameraRB.MoveRotation(Quaternion.Euler(Vector3.left * angle));


        /*
        //transform.rotation =  Quaternion.Euler(transform.rotation.eulerAngles + Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivityX);
        verticalLookRotation += inputY * mouseSensitivityY * Time.deltaTime * 10f;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -60,60);
        float a = inputY * mouseSensitivityY * Time.deltaTime * 10f;
        transform.Rotate(Vector3.up * inputX * mouseSensitivityX * Time.deltaTime * 10f);
        cameraTransform.Rotate(transform.up * inputX * mouseSensitivityX * Time.deltaTime * 10f  +  inputY * mouseSensitivityY * Time.deltaTime * 10f * - transform.right);
        //cameraTransform.localEulerAngles = Vector3.left * verticalLookRotation;*/
    }


    void FixedUpdate()
    {

        if (!playerControlled) return;

        rb.AddForce(-9.81f * groundNormal);

        // Apply movement to rigidbody
        //Vector3 localMove = transform.TransformDirection(moveAmount * Time.fixedDeltaTime);
        localTransform.position = transform.position;
        Vector3 up = (transform.position - planetPos).normalized;
        Vector3 fwd = Vector3.ProjectOnPlane(localTransform.forward, up).normalized;
        localTransform.rotation = Quaternion.LookRotation(fwd, (localTransform.position - planetPos).normalized);
        //if (rotateRight) localTransform.Rotate(localTransform.up, Time.fixedDeltaTime * 50f, Space.Self);
        //else if (rotateLeft) localTransform.Rotate(localTransform.up, -Time.fixedDeltaTime * 50f, Space.Self);
        if (rotateRight) localTransform.RotateAround(localTransform.position, localTransform.up, 1f);
        else if (rotateLeft) localTransform.RotateAround(localTransform.position, localTransform.up, -1f);

        Vector3 localMove = localTransform.TransformDirection(moveAmount * Time.fixedDeltaTime);
        rb.MovePosition(rb.position + localMove);
        //cameraRB.MovePosition(Vector3.Normalize(cameraRB.position + localMove - planetPos) * 50f + planetPos);
        //Debug.Log(transform.position + (cameraOffset_fwd * localTransform.forward) + (cameraOffset_up * localTransform.up));
        cameraRB.transform.position = transform.position - fwd * 7f + up * 6f;


    }



    void Jump()
    {
        if (Grounded())
        {
            rb.AddForce(transform.up * jumpForce);
            soundManager.PlaySfx(transform, sfx.jump);
        }
    }



    void Run() { speed = runSpeed; }

    void Walk() { speed = walkSpeed; }

    void Shoot()
    {
        if (!canShoot) return;
        StartCoroutine("Reload");
        soundManager.PlaySfx(transform, sfx.shoot);
        Bullet bullet = Instantiate(bulletPrefab, transform.position + transform.forward * 0.3f, transform.rotation).GetComponentInChildren<Bullet>();
        bullet.axis = transform.right;
        bullet.planetPos = planetPos;
        bullet.radius = distance.magnitude - 0.5f;
    }

    void Mine()
    {
        if (!canMine) return;
        StartCoroutine("ReloadMining");
        soundManager.PlaySfx(transform, sfx.shoot);
        Bullet bullet = Instantiate(minerPrefab, transform.position + transform.forward, transform.rotation).GetComponentInChildren<Bullet>();
        bullet.axis = transform.right;
        bullet.planetPos = planetPos;
        bullet.radius = distance.magnitude;
    }

    void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }


    bool Grounded()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;
        return Physics.Raycast(ray, out hit, 2f, groundedMask);
    }

    void GenerateLine()
    {
        float x;
        float y;
        int segments = 200;
        line.positionCount = 2 + segments;
        float radius = (planetPos - transform.position).magnitude;
        float angle = angle = 4f;

        for (int j = 0; j <= 1 + segments * 0.25; j++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;



            line.SetPosition(j, planetPos + x * transform.forward + y * transform.up);

            angle += (360f / segments);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Beam":
                controls.Disable();
                StartCoroutine("LeavePlanet");
                break;
            case "Exit":
                SceneManager.LoadScene("Level 2");
                break;
        }
    }

    IEnumerator LeavePlanet()
    {
        canvas.SetActive(false);
        playerControlled = false;
        rb.velocity = Vector3.zero;
        while (true)
        {
            rb.AddForce(5 * leaveBeam.transform.forward);
            yield return Helpers.GetWait(Time.fixedDeltaTime);
        }
    }

    void Death()
    {
        Restart();
    }


}

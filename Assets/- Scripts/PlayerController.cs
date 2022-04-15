using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector3 planetPos;
    Rigidbody rb;
    Vector3 gravityVector;
    float planetSize;
    Planet planet;
    float planetMass;
    CharacterController characterController;
    bool inGravityField = false;
    // public vars
	public float mouseSensitivityX = 1;
	public float mouseSensitivityY = 1;
	public float walkSpeed = 6;
    float runSpeed = 10;
    float speed;
	public float jumpForce = 500;
	public LayerMask groundedMask;
	
	// System vars
	Vector3 moveAmount;
	Vector3 smoothMoveVelocity;
	float verticalLookRotation;
	Transform cameraTransform;
    bool transitionned;
    [HideInInspector] public bool hasBullet;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject shield;
    bool groundContact = true;
    bool recentGroundContact = true;
    Vector3 groundNormal = Vector3.up;
    bool inPath = true;
    public static PlayerController instance;
    [HideInInspector] public bool shieldUp = false;
    Vector3 distance;
    bool inFightZone = false;
    [SerializeField] GameObject talkPanel;
    [HideInInspector] public bool inTalkZone = false;
    [SerializeField] GameObject dialogueManager;
    Transform talkTarget;
    Quaternion rotationBeforeConversation;
    [HideInInspector] public InputMaster controls;
    Vector2 direction;
    public enum controlMode  {Keyboard, Gamepad};
    public controlMode controlScheme = controlMode.Keyboard;
    float inputX;
    float inputY;
    Vector3 rotateAmount;
    Vector3 targetRotateAmount;
    Vector2 smoothRotateVelocity;
    Vector3 targetMoveAmount;
    Rigidbody cameraRB;
    float angle = 0f;
    //List rotArrayX;
    Vector2 input;
    Vector2 targetMouseDelta;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;


    void OnEnable() {
        controls.Enable();
    }

    void OnDisable() {
        controls.Disable();
    }
    
    private void Awake() {
        instance = this;
        controls = new InputMaster();
        controls.Player.Shoot.performed += context => Shoot();
        controls.Player.Jump.performed += context => Jump();
        controls.Player.Shield.started += context => ShieldUp();
        controls.Player.Shield.canceled += context => ShieldDown();
        controls.Player.Reload.performed += context => Reload();
        controls.Player.Talk.performed += context => Talk();
        controls.Player.Run.started += context => Run();
        controls.Player.Run.canceled += context => Walk();
        controls.Player.Pause.performed += context => PauseMenu.instance.Pause();

        if (Gamepad.all.Count > 0) controlScheme = controlMode.Gamepad;
        //rotArrayX = new Vector3[0];
        controlScheme = controlMode.Keyboard;
        cameraRB = Camera.main.GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        speed = walkSpeed;
        Application.targetFrameRate = -1;
        QualitySettings.vSyncCount = 0;
    }

    void Update()
    {
        RotatePlayer();
        Move();
        //RotatePlayer();

        //if (Input.GetKeyDown(KeyCode.Tab)) hasBullet = true; // Debug, to delete later*/

        rb.angularVelocity = Vector3.zero; 

    }

    void Move()
    {
        float moveX;
        float moveY;
        moveX = controls.Player.Move.ReadValue<Vector2>().x;
        moveY = controls.Player.Move.ReadValue<Vector2>().y;
        Vector3 moveDir = new Vector3(moveX, 0f, moveY);
        //direction = controls.Player.Move.ReadValue<Vector2>();
        if (moveDir.sqrMagnitude > 1) moveDir = moveDir.normalized;
        targetMoveAmount = moveDir * speed;
        moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, 0.15f);

        // Apply movement to rigidbody
		Vector3 localMove = transform.TransformDirection(moveAmount) * Time.deltaTime;
		rb.MovePosition(rb.position + localMove);
        //transform.Translate(localMove);
    }

    void RotatePlayer()
    {

        if (controlScheme == controlMode.Keyboard) {
            //inputX = Mouse.current.delta.x.ReadValue();
            //inputY = Mouse.current.delta.y.ReadValue();
            targetMouseDelta = Mouse.current.delta.ReadValue();
        }
        else {
            //inputX = Gamepad.current.rightStick.x.ReadValue() * 10f;
            //inputY = Gamepad.current.rightStick.y.ReadValue() * 10f;
        }    
        targetMouseDelta = Mouse.current.delta.ReadValue();

        input = Vector2.SmoothDamp(input, targetMouseDelta, ref smoothRotateVelocity, mouseSmoothTime);
        Debug.Log(input);
        
        verticalLookRotation += input.y * mouseSensitivityY * Time.deltaTime * 10f;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -60,60);

        rb.MoveRotation(Quaternion.Euler(rb.rotation.eulerAngles + Vector3.up * input.x * mouseSensitivityX * Time.deltaTime * 10f));
        //transform.Rotate(Vector3.up * input.x * mouseSensitivityX * Time.deltaTime * 10f);
        cameraTransform.localEulerAngles = Vector3.left * verticalLookRotation;

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


    void FixedUpdate() {
 
        //gravity for paths
        rb.AddForce(-9.81f*groundNormal);

		


        
	}



    void Jump()
    {
        if (Grounded()) {
            rb.AddForce(transform.up * jumpForce);
        }
    }

    

    void Run() {speed = runSpeed;}

    void Walk() {speed = walkSpeed;}

    void Shoot()
    {
        if (hasBullet && !shieldUp) {
            if (inGravityField) {
                Bullet bullet = Instantiate(bulletPrefab, transform.position + transform.forward, transform.rotation).GetComponentInChildren<Bullet>();
                bullet.axis = transform.right;
                bullet.planetPos = planetPos;
                bullet.radius = distance.magnitude;
                bullet.inGravityField = inGravityField;
            }
            else {
                Bullet bullet = Instantiate(bulletPrefab, cameraTransform.position + cameraTransform.forward, cameraTransform.rotation).GetComponentInChildren<Bullet>();
                bullet.inGravityField = inGravityField;
            }
        }
        
        
        hasBullet = false;
        GameManagement.instance.NormalCrosshair();
    }

    void ShieldUp()
    {
        shieldUp = true;
        shield.SetActive(true);
        GameManagement.instance.HideCrosshair();
    }

    void ShieldDown()
    {
        shieldUp = false;
        shield.SetActive(false);
        if (inGravityField || inFightZone) GameManagement.instance.ShowCrosshair();
    }

    void Reload()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    void Talk()
    {
        if (inTalkZone) {
            controls.Disable();
            inTalkZone = false;
            rotationBeforeConversation = cameraTransform.rotation;
            StartCoroutine(LerpRotationBodyAndCam(Quaternion.LookRotation(talkTarget.position - cameraTransform.position, transform.up)));
            talkPanel.SetActive(false);
            dialogueManager.SetActive(true);
            DialogueManager.instance.StartDialogue();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            moveAmount = Vector3.zero;
            PauseMenu.canGameBePaused = false;
        }
    }


    bool Grounded()
    {
        Ray ray = new Ray(transform.position, -transform.up);
		RaycastHit hit;
        return Physics.Raycast(ray, out hit, 2f, groundedMask);
    }

    private void OnTriggerEnter(Collider other) {
        switch (other.gameObject.tag) {

            case "PathRadius" :
                if (!recentGroundContact) FlipToPath(other.transform);
                break;

            case "Planet" :
                planet = other.GetComponent<Planet>();
                planetPos = planet.position;
                planetSize = planet.size;
                planetMass = planet.mass;
                inGravityField = true;
                recentGroundContact = false;
                if (!groundContact) StartCoroutine(LerpRotationToPlanet());
                break;
        
            case "FightZone" : 
                inFightZone = true;
                break;

            case "TalkZone" :
                talkPanel.SetActive(true);
                inTalkZone = true;
                talkTarget = other.gameObject.GetComponent<TalkZone>().targetTransform;
                break;
        
        }

    }
    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Planet") && !groundContact) {
            distance = rb.position - planetPos;
            groundNormal = distance.normalized;
            if (transitionned) transform.rotation = Quaternion.FromToRotation(transform.up, groundNormal) * rb.rotation;
        }
    }

    private void OnTriggerExit(Collider other) {
        switch(other.tag) {

            case "PathRadius" :
                inPath = false;
                break;

            case "Planet" :
                inGravityField = false;
                break;
            
            case "FightZone" : 
                inFightZone = false;
                break;

            case "TalkZone" :
                talkPanel.SetActive(false);
                inTalkZone = false;
                break;

            case "WorldEdge" :
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
                break;
        }
    }

    IEnumerator LerpRotationToPlanet()
    {   
        transitionned = false;
        Quaternion initPos = transform.rotation;
        float duration = 0.5f;
        float invDuration = 1f/duration;
        float startTime = Time.time;
        float ratio = 0f;
        WaitForEndOfFrame waitFrame = Helpers.GetWaitFrame;
        
        while (ratio < 1f) {
            ratio = (Time.time - startTime)*invDuration;
            transform.rotation = Quaternion.Slerp(initPos, Quaternion.FromToRotation(transform.up, groundNormal) * rb.rotation, ratio);
            yield return waitFrame;
        }
        transitionned = true;
    }

    IEnumerator LerpRotationBodyAndCam(Quaternion finalPos)
    {
        float duration = 0.5f;
        float invDuration = 1f/duration;
        float startTime = Time.time;
        float ratio = 0f;
        WaitForEndOfFrame waitFrame = Helpers.GetWaitFrame;
        Quaternion initPos = cameraTransform.rotation;

        while (ratio < 1f) {
            ratio = (Time.time - startTime)*invDuration;
            cameraTransform.rotation = Quaternion.Slerp(initPos, finalPos, ratio);
            yield return waitFrame;
        }
    }

    IEnumerator LerpRotation(Transform objectTransform, Quaternion finalPos)
    {   
        Quaternion initPos = objectTransform.rotation;

        float duration = 0.5f;
        float invDuration = 1f/duration;
        float startTime = Time.time;
        float ratio = 0f;
        WaitForEndOfFrame waitFrame = Helpers.GetWaitFrame;

        while (ratio < 1f) {
            ratio = (Time.time - startTime)*invDuration;
            transform.rotation = Quaternion.Slerp(initPos, finalPos, ratio);
            yield return waitFrame;
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Ground")) {
            if (!groundContact && !recentGroundContact) {
                FlipToPath(other.gameObject.transform);
            }
        }
    }

    void FlipToPath(Transform pathObject)
    {
        Vector3 normal = pathObject.up;
        Vector3 tangent = pathObject.right;
        Vector3 objectPos = pathObject.position;

        groundNormal = Vector3.Project(transform.position - objectPos, normal).normalized;
        //the normal is used projected here only to get the right direction even if the player is walking upside-down

        tangent = Vector3.Project(objectPos - transform.position, tangent);
        StartCoroutine(LerpRotation(transform, Quaternion.LookRotation(tangent, groundNormal)));
        
        groundContact = true;
        recentGroundContact = true;
    }

    private void OnCollisionExit(Collision other) {
        if (other.gameObject.CompareTag("Ground")) {
            groundContact = false;
            if (inGravityField) {
                StartCoroutine(LerpRotationToPlanet());
                recentGroundContact = false;
            }
        }
    }

    public void ConversationEnded()
    {
        StartCoroutine(LerpTowardInitPos(rotationBeforeConversation));
    }

    IEnumerator LerpTowardInitPos(Quaternion finalPos)
    {
        float duration = 0.5f;
        float invDuration = 1f/duration;
        float startTime = Time.time;
        float ratio = 0f;
        WaitForEndOfFrame waitFrame = Helpers.GetWaitFrame;
        Quaternion initPos = cameraTransform.rotation;

        while (ratio < 1f) {
            ratio = (Time.time - startTime)*invDuration;
            cameraTransform.rotation = Quaternion.Slerp(initPos, finalPos, ratio);
            yield return waitFrame;
        }

        PauseMenu.canGameBePaused = true;
        controls.Enable();
    }

}

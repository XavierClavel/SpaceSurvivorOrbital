using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MinerBot : MonoBehaviour
{
    enum state { following, mining, miningTransition, attacking, attackingTransition, transitioning, none }
    enum botFunction { mining, attacking, both };
    enum TargetType { player, resource, ennemy }

    state botState = state.none;
    Transform playerTransform;
    [SerializeField] float visionRadius = 4f;
    [SerializeField] Tool tool;
    [SerializeField] Vector2 toolRange;
    int toolPower;
    float toolReloadTime;
    List<GameObject> ennemies = new List<GameObject>();
    List<GameObject> resources = new List<GameObject>();
    TargetType targetType = TargetType.player;
    [SerializeField] float maxDistanceToPlayer = 9f;
    float sqrMaxDistanceToPlayer;
    Tween tween;
    [SerializeField] float speed = 0.2f;
    Rigidbody2D rb;
    bool isStatic = false;
    Transform target;
    float radius;

    delegate void onComplete();
    onComplete action;
    float stopRadius = 2;
    float fullSpeedRadius = 4;
    float sqrtStopRadius;
    float sqrtFullSpeedRadius;
    float cumulativeSpeed = 0f;
    InteractorHandler interactorHandler;
    [SerializeField] interactor weaponInteractorType = interactor.Laser;
    [SerializeField] interactor toolInteractorType = interactor.None;
    [SerializeField] botFunction function = botFunction.mining;
    Interactor weaponInteractor;
    Interactor toolInteractor;
    [SerializeField] Transform rotationPoint;
    float detectionRange = 6f;

    public const string ennemyLayerName = "EnnemiesOnly";
    public const string resourceLayerName = "ResourcesOnly";
    public const string everythingLayerName = "ResourcesAndEnnemies";

    const float ennemyAttackRange = 2f;
    const float resourceAttackRange = 2f;
    const float playerFollowRange = 2f;


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<CircleCollider2D>().radius = detectionRange;

        interactorHandler = GetComponent<InteractorHandler>();
        Interactor weapon = CsvParser.instance.objectReferencer.getInteractor(weaponInteractorType);
        Interactor tool = CsvParser.instance.objectReferencer.getInteractor(toolInteractorType);
        interactorHandler.Initialize(weapon, tool, rotationPoint);

        playerTransform = PlayerController.instance.transform;

        sqrMaxDistanceToPlayer = Mathf.Pow(maxDistanceToPlayer, 2);

        GetComponent<CircleCollider2D>().radius = visionRadius;
        rb = GetComponent<Rigidbody2D>();

        speed = PlayerManager.baseSpeed;

        SetNewTarget(playerTransform, fullSpeedRadius, Follow, state.transitioning);

        sqrtStopRadius = Mathf.Pow(stopRadius, 2);
        sqrtFullSpeedRadius = Mathf.Pow(fullSpeedRadius, 2);

        gameObject.layer = LayerMask.NameToLayer(getLayerName());
    }

    string getLayerName()
    {
        switch (function)
        {
            case botFunction.mining:
                return resourceLayerName;

            case botFunction.attacking:
                return ennemyLayerName;

            case botFunction.both:
                return everythingLayerName;

            default:
                throw new System.ArgumentException($"{function} not defined");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = target == playerTransform ? rb.velocity : target.position - rotationPoint.position;
        rotationPoint.position = transform.position;
        rotationPoint.rotation = Helpers.LookRotation2D(direction);

        if (interactorHandler.action)
        {
            if ((playerTransform.position - transform.position).sqrMagnitude > sqrMaxDistanceToPlayer)
            {
                interactorHandler.StopAction();
                SetNewTarget(playerTransform, fullSpeedRadius, Follow, state.transitioning);
            }
        }

    }

    private void FixedUpdate()
    {
        if (isStatic) return;

        Vector2 direction = target.position - transform.position;

        if (botState == state.following || botState == state.attacking)
        {
            if (direction.sqrMagnitude < sqrtStopRadius)
            {
                rb.velocity = direction.normalized * (1 - (sqrtStopRadius / direction.sqrMagnitude));
                //rb.velocity = Vector2.zero;
            }
            else if (direction.sqrMagnitude > sqrtFullSpeedRadius)
            {
                rb.velocity = direction.normalized * speed;
            }
            else
            {
                rb.velocity = direction.normalized * speed * (direction.sqrMagnitude - sqrtStopRadius) / (sqrtFullSpeedRadius - sqrtStopRadius);
            }
            return;

        }

        if (direction.sqrMagnitude > Mathf.Pow(radius, 2))
        {
            rb.velocity = direction.normalized * speed;
            return;
        }

        action();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Priority to ennemies, then to resources, then to player
        if (other.gameObject.layer == LayerMask.NameToLayer(ennemyLayerName))
        {
            ennemies.Add(other.gameObject);
            if (botState == state.attacking || botState == state.attackingTransition) return;
            SetNewTarget(other.transform, ennemyAttackRange, AttackEnnemy, state.attackingTransition);
            return;
        }
        else
        {
            resources.Add(other.gameObject);
            if (botState == state.following || botState == state.transitioning)
            {
                SetNewTarget(other.transform, resourceAttackRange, StartMining, state.miningTransition);
            }
        }

    }

    void StartMining()
    {
        //TODO : switch interactor;
        rb.velocity = Vector2.zero;
        isStatic = true;
        botState = state.mining;
        interactorHandler.StartAction();
    }

    void SetNewTarget(Transform target, float radius, onComplete action, state newState)
    {
        isStatic = false;
        botState = newState;
        this.target = target;
        this.radius = radius;
        this.action = action;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(resourceLayerName)) resources.TryRemove(other.gameObject);
        else ennemies.TryRemove(other.gameObject);
        if (other.transform == target) EndAction();
    }

    void EndAction()
    {
        interactorHandler.StopAction();
        if (ennemies.Count > 0)
        {
            SetNewTarget(ennemies[0].transform, 1f, AttackEnnemy, state.attackingTransition);
            return;
        }
        else if (resources.Count > 0)    //current resource is counted
        {
            SetNewTarget(resources[0].transform, 1f, StartMining, state.miningTransition);
            return;
        }
        SetNewTarget(playerTransform, fullSpeedRadius, Follow, state.transitioning);

    }

    void Follow()
    {
        botState = state.following;
    }

    void AttackEnnemy()
    {
        botState = state.attacking;
        interactorHandler.StartAction();
    }


}

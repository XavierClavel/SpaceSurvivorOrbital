using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MinerBot : MonoBehaviour
{
    enum state { following, mining, miningTransition, transitioning, none }

    state botState = state.none;
    Transform playerTransform;
    [SerializeField] float visionRadius = 4f;
    [SerializeField] Tool tool;
    [SerializeField] Vector2 toolRange;
    int toolPower;
    float toolReloadTime;
    List<GameObject> resources = new List<GameObject>();
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

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = PlayerController.instance.transform;
        toolPower = PlayerManager.minerBotPower;
        toolReloadTime = 1f / PlayerManager.mineerBotSpeed;


        tool = Instantiate(PlayerManager.tool, transform.position, Quaternion.identity);
        tool.transform.SetParent(transform);
        tool.Initialize(toolRange, toolPower, toolReloadTime);
        tool.onNoRessourcesLeft.AddListener(EndMining);
        tool.onResourceExit.AddListener(gameObject => resources.TryRemove(gameObject));

        sqrMaxDistanceToPlayer = Mathf.Pow(maxDistanceToPlayer, 2);

        GetComponent<CircleCollider2D>().radius = visionRadius;
        rb = GetComponent<Rigidbody2D>();

        speed = PlayerManager.baseSpeed;

        SetNewTarget(playerTransform, fullSpeedRadius, Follow, state.transitioning);

        sqrtStopRadius = Mathf.Pow(stopRadius, 2);
        sqrtFullSpeedRadius = Mathf.Pow(fullSpeedRadius, 2);
    }

    // Update is called once per frame
    void Update()
    {


        if (botState == state.mining)
        {
            if ((playerTransform.position - transform.position).sqrMagnitude > sqrMaxDistanceToPlayer)
            {
                tool.StopMining();
                SetNewTarget(playerTransform, fullSpeedRadius, Follow, state.transitioning);
            }
        }

    }

    private void FixedUpdate()
    {
        if (isStatic) return;

        Vector2 direction = target.position - transform.position;

        if (botState == state.following)
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
            //Debug.Log(rb.velocity);
            return;
        }

        action();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        resources.Add(other.gameObject);
        if (botState != state.mining && botState != state.miningTransition)
        {
            SetNewTarget(other.transform, 1f, StartMining, state.miningTransition);
        }
    }

    void StartMining()
    {
        rb.velocity = Vector2.zero;
        isStatic = true;
        botState = state.mining;
        tool.StartMining();
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
        resources.TryRemove(other.gameObject);
    }

    void EndMining()
    {
        if (resources.Count == 0)    //current resource is counted
        {
            tool.StopMining();
            SetNewTarget(playerTransform, fullSpeedRadius, Follow, state.transitioning);
            return;
        }
        SetNewTarget(resources[0].transform, 1f, StartMining, state.miningTransition);

    }

    void Follow()
    {
        botState = state.following;
    }


}

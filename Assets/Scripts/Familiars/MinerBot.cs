using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerBot : MonoBehaviour
{
    enum state { following, mining }

    state botState = state.following;
    Transform playerTransform;
    [SerializeField] Tool tool;
    [SerializeField] Vector2 toolRange;
    [SerializeField] int toolPower;
    [SerializeField] float toolReloadTime;
    List<GameObject> resources = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = PlayerController.instance.transform;

        tool = Instantiate(PlayerManager.tool, transform.position, Quaternion.identity);
        tool.transform.SetParent(transform);
        tool.Initialize(toolRange, toolPower, toolReloadTime);
        tool.onNoRessourcesLeft.AddListener(EndMining);
        tool.onResourceExit.AddListener(gameObject => resources.TryRemove(gameObject));

    }

    // Update is called once per frame
    void Update()
    {
        if (botState == state.following)
        {
            transform.position = playerTransform.position + Vector3.right;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        resources.Add(other.gameObject);
        if (botState != state.mining)
        {
            Debug.Log("start mining");
            botState = state.mining;
            transform.position = other.transform.position - Vector3.down;
            tool.StartMining();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        resources.TryRemove(other.gameObject);
        //if (tool.resourcesInRange.Count == 0) EndMining();
    }

    void EndMining()
    {

        if (resources.Count == 0)    //current resource is counted
        {
            botState = state.following;
            return;
        }
        transform.position = resources[0].transform.position - Vector3.down;
        tool.StartMining();

    }


}

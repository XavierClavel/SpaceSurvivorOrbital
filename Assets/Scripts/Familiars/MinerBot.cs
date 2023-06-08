using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerBot : MonoBehaviour
{
    enum state { following, mining }

    state botState = state.following;
    Transform playerTransform;
    [SerializeField] Tool tool;
    [SerializeField] int toolPower;
    [SerializeField] float toolReloadTime;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = PlayerController.instance.transform;
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
        if (botState != state.mining) botState = state.mining;
        transform.position = other.transform.position - Vector3.down;

    }


}

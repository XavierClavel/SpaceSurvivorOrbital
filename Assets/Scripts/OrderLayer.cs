using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderLayer : MonoBehaviour
{
    private float order;
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
       sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        order = -transform.position.y + 2000 + transform.position.x;
        sprite.sortingOrder = (int) order;
    }
}

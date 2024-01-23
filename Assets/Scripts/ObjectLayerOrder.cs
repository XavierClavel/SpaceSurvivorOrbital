using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLayerOrder : MonoBehaviour
{
    private int order;
    [SerializeField] private bool isChild = false;


    // Start is called before the first frame update
    void Start()
    {
        order = (int) Mathf.Abs((transform.position.y+60) * 10);
        GetComponent<SpriteRenderer>().sortingOrder = order;
        //Debug.Log(GetComponent<SpriteRenderer>().sortingOrder); 

        if(isChild){
            transform.parent.GetComponent<SpriteRenderer>().sortingOrder = order;
        }

    }
}

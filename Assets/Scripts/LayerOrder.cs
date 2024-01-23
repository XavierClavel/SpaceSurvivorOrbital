using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerOrder : MonoBehaviour
{
    [SerializeField] private GameObject referenceObject;
    private int defaultOrder;
    private HashSet<Collider2D> colliders = new HashSet<Collider2D>();


    private void Start() {
        /*if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }*/
        defaultOrder = referenceObject.GetComponent<SpriteRenderer>().sortingOrder;
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Obstacle"){
            int order = other.transform.GetComponent<SpriteRenderer>().sortingOrder-2;
            colliders.Add(other);
            if (order< referenceObject.GetComponent<SpriteRenderer>().sortingOrder){
                foreach(Transform child in referenceObject.transform)
                {
                    child.GetComponent<SpriteRenderer>().sortingOrder=order+1;
                }
                referenceObject.GetComponent<SpriteRenderer>().sortingOrder=order;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Obstacle"){
            colliders.Remove(other);
            UpdateLayer();
        }
        
    }

    private void UpdateLayer(){
        int minSort = defaultOrder;
        foreach (Collider2D coll in colliders)
        {
            if((coll.transform.GetComponent<SpriteRenderer>().sortingOrder-2)<minSort){
                minSort=coll.transform.GetComponent<SpriteRenderer>().sortingOrder-2;
            }
        }
        referenceObject.GetComponent<SpriteRenderer>().sortingOrder=minSort;
        foreach(Transform child in referenceObject.transform){
            child.GetComponent<SpriteRenderer>().sortingOrder=minSort+1;
        }
    }
}

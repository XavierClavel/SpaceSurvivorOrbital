using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LiquidResource : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] Image image;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] new CircleCollider2D collider;
    [Header("Parameters")]
    [SerializeField] float timeToFill = 10f;
    [SerializeField] float referenceDPS = 50f;
    [SerializeField] Vector2Int resourcesAmount = new Vector2Int(2, 5);
    int nbResources;
    float increment;
    int currentIncrement;


    float factor;
    float fillAmount = 1;

    bool lookingAtRessource = false;

    int interactibleLayer;

    // Start is called before the first frame update
    void Start()
    {
        factor = 1f / (timeToFill * referenceDPS); //Calculated so that it takes timeToFill seconds to mine at referenceDPS dps

        nbResources = Random.Range(resourcesAmount.x, resourcesAmount.y);
        increment = 1f / (float)nbResources;
        currentIncrement = nbResources;

        ObjectManager.dictObjectToInteractable.Add(gameObject, this);

        interactibleLayer = LayerMask.GetMask(Vault.layer.Interactible);
    }

    public void StartInteracting() { }

    public void StopInteracting() { }

    public void Interacting() { }


}

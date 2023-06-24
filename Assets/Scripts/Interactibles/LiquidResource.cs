using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LiquidResource : MonoBehaviour, IInteractable
{
    bool interacting = false;
    [Header("References")]
    [SerializeField] Image image;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] new CircleCollider2D collider;
    [Header("Parameters")]
    [SerializeField] float timeToFill = 2f;
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

        interactibleLayer = LayerMask.GetMask("Interactible");
    }

    public void StartInteracting() { }

    public void StopInteracting() { }

    public void Interacting() { }

    private void OnTriggerEnter2D(Collider2D other)
    {
        image.gameObject.SetActive(true);
        StartCoroutine(nameof(Interact), (float)InteractorHandler.currentInteractor.dps);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        StopCoroutine(nameof(Interact));
        image.gameObject.SetActive(false);
    }

    IEnumerator Interact(float dps)
    {
        while (fillAmount > 0)
        {
            yield return Helpers.GetWaitFixed;
            if (!UpdateLookStatus()) continue;


            fillAmount -= Time.fixedDeltaTime * factor * dps;

            int value = (int)(fillAmount / increment);
            if (value != currentIncrement)
            {
                currentIncrement = value;
                PlayerController.instance.IncreaseViolet();
            }
            image.fillAmount = fillAmount;
        }
        Break();
    }

    bool UpdateLookStatus()
    {
        //TODO : interactibleOnly for interactionRadius to avoid RayCastAll and save performances
        RaycastHit2D hit = Physics2D.Raycast(PlayerController.instance.transform.position, ObjectManager.instance.armTransform.right, 99f, interactibleLayer);
        if (hit && hit.collider.gameObject == gameObject)
        {
            if (!lookingAtRessource) InteractorHandler.playerInteractorHandler.StartMiningPurple();
            lookingAtRessource = true;
            return true;
        }
        else
        {
            if (lookingAtRessource) InteractorHandler.playerInteractorHandler.StopMiningPurple();
            lookingAtRessource = false;
            return false;
        }
    }


    void Break()
    {
        InteractorHandler.playerInteractorHandler.StopMiningPurple();
        SoundManager.instance.PlaySfx(transform, sfx.breakResource);
        Destroy(gameObject);
    }


}

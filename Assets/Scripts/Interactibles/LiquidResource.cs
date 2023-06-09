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
    [SerializeField] Vector2Int resourcesAmount = new Vector2Int(2, 5);
    int nbResources;
    float increment;
    int currentIncrement;


    float factor;
    float fillAmount = 1;

    // Start is called before the first frame update
    void Start()
    {
        factor = 1f / timeToFill;
        nbResources = Random.Range(resourcesAmount.x, resourcesAmount.y);
        increment = 1f / (float)nbResources;
        currentIncrement = nbResources;

        ObjectManager.dictObjectToInteractable.Add(gameObject, this);
    }

    public void StartInteracting()
    {
        interacting = true;
    }

    public void Interacting()
    {
        if (!interacting) return;
        fillAmount -= Time.fixedDeltaTime * factor;

        if (fillAmount <= 0) Break();

        int value = (int)(fillAmount / increment);
        if (value != currentIncrement)
        {
            currentIncrement = value;
            PlayerController.instance.IncreaseViolet();
        }
        image.fillAmount = fillAmount;
    }

    public void StopInteracting()
    {
        interacting = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        image.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        image.gameObject.SetActive(false);
    }

    void Break()
    {
        SoundManager.instance.PlaySfx(transform, sfx.breakResource);
        InteractionRadius.interactables.Remove(ObjectManager.dictObjectToInteractable[gameObject]);
        ObjectManager.dictObjectToInteractable.Remove(gameObject);
        Destroy(gameObject);
    }


}

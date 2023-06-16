using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LiquidResource : MonoBehaviour, IResource
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

        ObjectManager.dictObjectToResource.Add(gameObject, this);
    }

    public void StartMining()
    {
        StartCoroutine(nameof(Mine));
        image.gameObject.SetActive(true);
    }

    public void StopMining()
    {
        StopCoroutine(nameof(Mine));
    }

    public void Hit(int damage) { }

    IEnumerator Mine()
    {
        while (fillAmount > 0)
        {
            yield return Helpers.GetWaitFixed;

            fillAmount -= Time.fixedDeltaTime * factor;

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

    public void Mining()
    {

    }


    void Break()
    {
        SoundManager.instance.PlaySfx(transform, sfx.breakResource);
        Destroy(gameObject);
    }


}

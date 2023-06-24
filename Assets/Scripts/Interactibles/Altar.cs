using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Altar : MonoBehaviour, IInteractable
{
    [SerializeField] Image image;
    [SerializeField] float timeToLaunch = 5f;
    float factor;
    float fillAmount = 1;

    private void Start()
    {
        ObjectManager.dictObjectToInteractable.Add(gameObject, this);
        ObjectManager.altar = this;

        factor = 1f / timeToLaunch;
    }

    public void StartInteracting() { }

    public void StopInteracting() { }
    public void Interacting() { }

    private void OnTriggerEnter2D(Collider2D other)
    {
        image.gameObject.SetActive(true);
        StartCoroutine(nameof(PrepareAltar));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        StopCoroutine(nameof(PrepareAltar));
        fillAmount = 1;
        image.fillAmount = fillAmount;
        image.gameObject.SetActive(false);
    }

    IEnumerator PrepareAltar()
    {
        while (fillAmount > 0)
        {
            yield return Helpers.GetWaitFixed;
            fillAmount -= Time.fixedDeltaTime * factor;
            image.fillAmount = fillAmount;
        }

        ActivateAltar();
    }

    void ActivateAltar()
    {
        ObjectManager.DisplayAltarUI();
    }
}

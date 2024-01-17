using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Altar : MonoBehaviour, IInteractable
{
    [SerializeField] Image image;
    [SerializeField] float timeToLaunch = 5f;
    [SerializeField] ParticleSystem auraAltar;
    [SerializeField] ParticleSystem altarActivate;
    float factor;
    float fillAmount = 1;
    private Animator animator;
    private static readonly int Deplete = Animator.StringToHash("Deplete");

    private void Awake()
    {
        ObjectManager.altar = this;
    }

    private void Start()
    {
        ObjectManager.dictObjectToInteractable.Add(gameObject, this);
        animator = GetComponent<Animator>();

        factor = 1f / timeToLaunch;
    }

    public void StartInteracting() { }

    public void StopInteracting() { }
    public void Interacting() { }

    private void OnTriggerEnter2D(Collider2D other)
    {
        image.gameObject.SetActive(true);
        SoundManager.PlaySfx(transform, key: "Altar_Loading");
        altarActivate.Play();
        StartCoroutine(nameof(PrepareAltar));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        StopCoroutine(nameof(PrepareAltar));
        altarActivate.Stop();
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

    public void DepleteAltar()
    {
        animator.SetTrigger(Deplete);
        GetComponent<Collider2D>().enabled = false;
        auraAltar.Stop();
    }

    void ActivateAltar()
    {
        ObjectManager.DisplayAltarUI();
        AltarPanel.UpdateAltarDisplay();
    }
}

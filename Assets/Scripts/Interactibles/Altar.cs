using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Altar : MonoBehaviour, IInteractable
{
    [SerializeField] Image image;
    [SerializeField] float timeToLaunch = 4f;
    [SerializeField] ParticleSystem auraAltar;
    [SerializeField] ParticleSystem altarActivate;
    [SerializeField] ParticleSystem altarLoading;
    [SerializeField] ParticleSystem playerEffect;
    float factor;
    float fillAmount = 1;
    public Animator animator;
    private static readonly int Deplete = Animator.StringToHash("Deplete");
    
    public static List<IAltarListener> listeners = new List<IAltarListener>();

    public static void registerListener(IAltarListener listener)
    {
        listeners.TryAdd(listener);
    }

    public static void unregisterListener(IAltarListener listener)
    {
        listeners.TryRemove(listener);
    }
    
    private void Start()
    {
        ObjectManager.dictObjectToInteractable.Add(gameObject, this);
        //animator = GetComponent<Animator>();

        factor = 1f / timeToLaunch;
        listeners.ForEach(it => it.onAltarSpawned(this));
    }

    public void StartInteracting() { }

    public void StopInteracting() { }
    public void Interacting() { }

    private void OnTriggerEnter2D(Collider2D other)
    {
        image.gameObject.SetActive(true);
        altarActivate.Play();
        //altarLoading.Play();

        StartCoroutine(nameof(PrepareAltar));
        StartCoroutine(nameof(SfxLoadingAltar));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        StopCoroutine(nameof(PrepareAltar));
        playerEffect.Stop();
        altarActivate.Stop();
        PlayerController.StopShake();
        
        fillAmount = 1;
        image.fillAmount = fillAmount;
        image.gameObject.SetActive(false);
    }

    IEnumerator PrepareAltar()
    {
        playerEffect.Play();
        //PlayerController.StartShake(1f, 1f);

        while (fillAmount > 0)
        {
            yield return Helpers.getWaitFixed();
            fillAmount -= Time.fixedDeltaTime * factor;
            
            image.fillAmount = fillAmount;
        }
        
        //PlayerController.StopShake();

        playerEffect.Stop();

        ActivateAltar();
    }

    IEnumerator SfxLoadingAltar()
    {
       yield return new WaitForSeconds(timeToLaunch - 1);
       SoundManager.PlaySfx(transform, key: "Altar_Loading");     
    }

    public void DepleteAltar()
    {
        animator.SetTrigger(Deplete);
        GetComponent<Collider2D>().enabled = false;
        auraAltar.Stop();
        altarLoading.Stop();
        listeners.ForEach(it => it.onAltarUsed(this));
    }

    void ActivateAltar()
    {
        ObjectManager.altar = this;
        ObjectManager.DisplayAltarUI();
        AltarPanel.UpdateAltarDisplay();
    }
}

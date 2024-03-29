using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Altar : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] float timeToLaunch = 4f;
    [SerializeField] ParticleSystem auraAltar;
    [SerializeField] ParticleSystem altarActivate;
    [SerializeField] ParticleSystem altarLoading;
    [SerializeField] ParticleSystem playerEffect;
    [SerializeField] GameObject lightAltar;
    float factor;
    float fillAmount = 1;
    public Animator animator;
    private static readonly int Deplete = Animator.StringToHash("Deplete");
    
    private void Start()
    {
        factor = 1f / timeToLaunch;
        EventManagers.altar.dispatchEvent(it => it.onAltarSpawned(this));
    }

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
        ShakeManager.StopShake();
        
        fillAmount = 1;
        image.fillAmount = fillAmount;
        image.gameObject.SetActive(false);
    }

    IEnumerator PrepareAltar()
    {
        playerEffect.Play();
        
        ShakeManager.StartShake(1f);

        while (fillAmount > 0)
        {
            yield return Helpers.getWaitFixed();
            fillAmount -= Time.fixedDeltaTime * factor;
            
            image.fillAmount = fillAmount;
        }
        
        ShakeManager.StopShake();

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
        lightAltar.SetActive(false);
        EventManagers.altar.dispatchEvent(it => it.onAltarUsed(this));
    }

    void ActivateAltar()
    {
        ObjectManager.altar = this;
        ObjectManager.DisplayAltarUI();
        AltarPanel.UpdateAltarDisplay();
    }
}

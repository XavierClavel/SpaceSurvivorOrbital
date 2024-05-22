using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * <pre>
 * <p> FloatA -> Shockwave max range </p>
 * <p> IntA -> Shockwave damage </p>
 * <p> IntB -> Shockwave knockback </p>
 * <p> ElementA -> Element of the shockwave </p>
 * <p> BoolA -> Double shockwave ? </p>
 * <p> BoolB -> Protecting wave ? </p>
 * <p> AttackSpeed -> Spawn rate </p>
 * </pre>
 */
public class SynthWave : Power, IPlayerEvents
{
    [SerializeField] private Shockwave shockwave;
    private Shockwave shockwave2;
    private Shockwave protectingShockwave;
    [SerializeField] ParticleSystem protectingPS;
    
    private bool doubleShockwave;
    private int shockwaveDamage;
    private float shockwaveMaxRange;
    private status shockwaveElement;
    private int shockwaveKnockback;
    private bool isProtectingWaveActive;

    public override void Setup(PlayerData _)
    {
        base.Setup(_);
        shockwaveMaxRange = _.generic.floatA;
        shockwaveDamage = _.generic.intA;
        shockwaveKnockback = _.generic.intB;
        shockwaveElement = _.generic.elementA;
        doubleShockwave = _.generic.boolA;
        isProtectingWaveActive = _.generic.boolB;
        if (isProtectingWaveActive) EventManagers.player.registerListener(this);
        
        if (doubleShockwave)
        {
            shockwave2 = Instantiate(shockwave, player.transform, true);
            shockwave2.Setup(shockwaveMaxRange, shockwaveDamage, shockwaveElement, shockwaveKnockback);
            shockwave2.gameObject.name = "Synthwave2";
        }

        if (isProtectingWaveActive)
        {
            protectingShockwave = Instantiate(shockwave, player.transform, true);
            protectingShockwave.Setup(
                ConstantsData.protectingWaveRange, 
                ConstantsData.protectingWaveDamage, 
                shockwaveElement, 
                ConstantsData.protectingWaveKnockback);
            protectingShockwave.gameObject.name = "Protecting Wave";
        }
        
        shockwave = Instantiate(shockwave, player.transform, true);
        shockwave.Setup(shockwaveMaxRange, shockwaveDamage, shockwaveElement, shockwaveKnockback);
        shockwave.gameObject.name = "Synthwave1";

        InvokeRepeating(nameof(DoFirstWave), 0f, stats.cooldown);
        if (doubleShockwave) {InvokeRepeating(nameof(DoSecondWave), 0.5f, stats.cooldown); }
    }

    private void DoFirstWave()
    {
        DoShockwave(shockwave);
        SoundManager.PlaySfx(transform, key: "SynthWave_Sound");
    } 
    
    private void DoSecondWave()
    {
        DoShockwave(shockwave2);
        SoundManager.PlaySfx(transform, key: "SynthWave_Sound");
    } 
    
    private void DoProtectingWave()
    {
        Debug.Log("ProtectingWave");

        DoShockwave(protectingShockwave);
        protectingPS.Play();
        SoundManager.PlaySfx(transform, key: "SynthWave_Sound");
    } 


    private void DoShockwave(Shockwave shockwave)
    {
        var transform1 = shockwave.transform;
        transform1.localScale = Vector3.zero;
        transform1.localPosition = Vector3.zero;
        shockwave.doShockwave();
        SoundManager.PlaySfx(transform, key: "SynthWave_Sound");
    }
    
    
    /**
     * Called on player hit. If protecting wave is active and recharged, damage is cancelled.
     */
    public bool onPlayerHit(bool shieldHit)
    {
        bool value = isProtectingWaveActive;
        isProtectingWaveActive = false;
        if (value) DoProtectingWave();
        StartCoroutine(nameof(RechargeProtectingWave));
        return value;
    }

    private IEnumerator RechargeProtectingWave()
    {
        yield return Helpers.getWait(ConstantsData.protectingWaveCooldown);
        isProtectingWaveActive = true;
    }

    public bool onPlayerDeath()
    {
        return false;
    }

    public void onResourcePickup(resourceType type)
    {
        
    }

    private void OnDestroy()
    {
        EventManagers.player.unregisterListener(this);
    }
}

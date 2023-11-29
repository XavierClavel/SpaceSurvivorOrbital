using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Shapes;
/**
 * <pre>
 * <p> BaseDamage -> DPS </p> 
 * <p> Range -> Max distance for reraching targets </p>
 * <p> Spread -> Width of the laser </p>
 * <p> Cooldown -> Time before overheat </p>
 * <p> BoolA -> Whether shockwave is generated </p>
 * <p> FloatA -> Shockwave max range </p>
 * <p> IntA -> Shockwave damage </p>
 * <p> BoolB -> Energy shield </p>
 * <p> FloatB -> Overheat jauge increase from damage absorbtion </p>
 * <p> ElementA -> Element of the shockwave </p>
 * </pre>
 */
public class Laser : Interactor
{
    private int dps;
    private float width;
    [SerializeField] protected Transform firePoint;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] private Shockwave shockwave;
    playerDirection _aimDirection_value = playerDirection.front;

    private bool isShockwaveEnabled;
    private int shockwaveDamage;
    private float shockwaveMaxRange;
    
    private bool isEnergyShieldEnabled;
    private float energyShieldOverheatCost;

    private status shockwaveElement;
    

    private float _heatValue = 0f;

    private AudioSource laserSfxSource;

    private float heatValue
    {
        get
        {
            return _heatValue;
        }

        set
        {
            _heatValue = value;
            reloadSlider.value = value;
            if (value <= 0f)
            {
                if (reloadSlider.gameObject.activeSelf) reloadSlider.gameObject.SetActive(false);
            }
            else
            {
                if (!reloadSlider.gameObject.activeSelf) reloadSlider.gameObject.SetActive(true);
            }
            
            if (value >= ConstantsData.laserOverheatThreshold)
            {
                if (!overheating)
                {
                    onLaserStop();
                    onOverheat();
                }
                overheating = true;
            } else if (value <= 0f)
            {
                overheating = false;
                if (isUsing) onLaserStart();
            }
        }
    }
    
    private bool overheating = false;
    private bool laserBeamActive;
    private const float laserSfxTransitionTime = 0.3f;

    protected override void Start()
    {
        base.Start();
        isShockwaveEnabled = fullStats.generic.boolA;
        shockwaveMaxRange = fullStats.generic.floatA;
        shockwaveDamage = fullStats.generic.intA;
        
        isEnergyShieldEnabled = fullStats.generic.boolB;
        energyShieldOverheatCost = fullStats.generic.floatB;

        shockwaveElement = fullStats.generic.elementA;
    
        width = 0.1f * stats.spread;
        lineRenderer.enabled = true;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        
        reloadSlider.gameObject.SetActive(true);
        reloadSlider.maxValue = ConstantsData.laserOverheatThreshold;
        dps = stats.baseDamage.x;

        shockwave = Instantiate(shockwave, player.transform, true);
        shockwave.transform.localScale = Vector3.zero;
        shockwave.transform.localPosition = Vector3.zero;
        shockwave.Setup(shockwaveMaxRange, shockwaveDamage, shockwaveElement);

        laserSfxSource = GetComponent<AudioSource>();
        laserSfxSource.volume = 0f;
        
        if (!ScriptableObjectManager.dictKeyToSfx.ContainsKey("Laser"))
        {
            Debug.LogWarning($"Sfx key Laser not found");
            return;
        }
        Sfx sfx = ScriptableObjectManager.dictKeyToSfx["Laser"];
        laserSfxSource.clip = sfx.getClip();
        laserSfxSource.Play();
        Debug.Log(laserSfxSource.isPlaying);

    }



    protected override void onStartUsing()
    {
        if (!overheating) onLaserStart();
    }

    protected override void onStopUsing()
    {
        if (!overheating) onLaserStop();
    }

    private void FixedUpdate()
    {
        heatValue = Mathf.Clamp(heatValue + Time.fixedDeltaTime * getHeatValueChangeFactor(), 0f, ConstantsData.laserOverheatThreshold);
        
        
        
        if (overheating || !isUsing)
        {
            UpdateBeamVisibility(false);
            return;
        }
        
        if (isUsing)
        {
            UpdateBeamVisibility(true);
            UpdateLaserBeam();
        }

        
    }

    public override bool isDamageAbsorbed()
    {
        if (!isEnergyShieldEnabled) return false;
        if (!isUsing) return false;
        if (heatValue + energyShieldOverheatCost >= ConstantsData.laserOverheatThreshold) return false;
        heatValue += energyShieldOverheatCost;
        return true;
    }

    void onOverheat()
    {
        if (!isShockwaveEnabled) return;
        shockwave.doShockwave();
    }

    void onLaserStart()
    {
        DOTween.To(() => laserSfxSource.volume, x => laserSfxSource.volume = x, 1f, laserSfxTransitionTime);
    }

    void onLaserStop()
    {
        DOTween.To(() => laserSfxSource.volume, x => laserSfxSource.volume = x, 0f, laserSfxTransitionTime);
    }

    float getHeatValueChangeFactor()
    {
        if (overheating) return -ConstantsData.laserOverheatCoolingFactor;
        return isUsing ? ConstantsData.laserHeatingFactor : -ConstantsData.laserCoolingFactor;
    }

    void UpdateBeamVisibility(bool value)
    {
        if (value != lineRenderer.gameObject.activeInHierarchy) lineRenderer.gameObject.SetActive(value);
    }
    
    void UpdateLaserBeam()
    {
        lineRenderer.SetPosition(0, firePoint.position);
        

        RaycastHit2D[] hits = Physics2D.CircleCastAll(firePoint.position, width, firePoint.right, stats.range, currentLayerMask);
        if (hits.Length == 0)
        {
            lineRenderer.SetPosition(1, firePoint.position + firePoint.right * stats.range);
            return;
        }

        ApplyRaycast(hits);
    }

    void ApplyRaycast(RaycastHit2D[] hits)
    {
        int stopIndex = Mathf.Min(hits.Length, stats.pierce + 1);

        for (int i = 0; i < stopIndex; i++)
        {
            if (hits[i].collider.gameObject.layer == LayerMask.NameToLayer(Vault.layer.Obstacles))
            {
                lineRenderer.SetPosition(1, firePoint.position + firePoint.right * hits[i].distance);
                return;
            }
            HurtEnnemy(hits[i].collider.gameObject);
        }

        float range = hits.Length < stats.pierce
            ? stats.range
            : hits[stopIndex - 1].distance;
        lineRenderer.SetPosition(1, firePoint.position + firePoint.right * range);
    }
    

    void HurtEnnemy(GameObject go)
    {
        ObjectManager.dictObjectToHitable[go].StackDamage(dps, stats.element);
    }

    protected override void onUse()
    {

    }
}

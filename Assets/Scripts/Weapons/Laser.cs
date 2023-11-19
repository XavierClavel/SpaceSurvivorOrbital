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
    private float shockwaveMaxRange;
    private int shockwaveDamage;
    

    private float _heatValue = 0f;

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
        }
    }
    
    private bool overheating = false;
    private bool laserBeamActive;

    protected override void Start()
    {
        base.Start();
        isShockwaveEnabled = fullStats.generic.boolA;
        shockwaveMaxRange = fullStats.generic.floatA;
        shockwaveDamage = fullStats.generic.intA;
    
    
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
        shockwave.Setup(shockwaveMaxRange, shockwaveDamage, status.none);

    }



    protected override void onStartUsing()
    {
    
    }

    protected override void onStopUsing()
    {

    }

    private void FixedUpdate()
    {
        heatValue = Mathf.Clamp(heatValue + Time.fixedDeltaTime * getHeatValueChangeFactor(), 0f, ConstantsData.laserOverheatThreshold);
        
        if (heatValue >= ConstantsData.laserOverheatThreshold)
        {
            if (!overheating)
            {
                onOverheat();
            }
            overheating = true;
        } else if (heatValue <= 0f)
        {
            overheating = false;
        }
        
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

    void onOverheat()
    {
        if (!isShockwaveEnabled) return;
        shockwave.doShockwave();
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
        ObjectManager.dictObjectToHitable[go].StackDamage(dps);
    }

    protected override void onUse()
    {

    }
}

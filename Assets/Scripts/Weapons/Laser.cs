using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Laser : Interactor
{
    [SerializeField] protected Transform firePoint;
    [SerializeField] LineRenderer lineRenderer;
    playerDirection _aimDirection_value = playerDirection.front;

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
    private float overheatThreshold = 1f;
    private float heatingFactor = 0.45f;
    private float coolingFactor = 1f;
    private float overheatCoolingFactor = 0.5f;
    private bool overheating = false;
    private bool laserBeamActive;

    protected override void Start()
    {
        base.Start();
        lineRenderer.enabled = true;
        reloadSlider.gameObject.SetActive(true);
    }



    protected override void onStartUsing()
    {
    
    }

    protected override void onStopUsing()
    {

    }

    private void FixedUpdate()
    {
        heatValue = Mathf.Clamp(heatValue + Time.fixedDeltaTime * getHeatValueChangeFactor(), 0f, overheatThreshold);
        
        if (heatValue >= overheatThreshold)
        {
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

    float getHeatValueChangeFactor()
    {
        if (overheating) return -overheatCoolingFactor;
        return isUsing ? heatingFactor : -coolingFactor;
    }

    void UpdateBeamVisibility(bool value)
    {
        if (value != lineRenderer.gameObject.activeInHierarchy) lineRenderer.gameObject.SetActive(value);
    }
    
    void UpdateLaserBeam()
    {
        lineRenderer.SetPosition(0, firePoint.position);

        RaycastHit2D[] hits = Physics2D.RaycastAll(firePoint.position, firePoint.right, stats.range, currentLayerMask);
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
                lineRenderer.SetPosition(1, hits[i].point);
                return;
            }
            HurtEnnemy(hits[i].collider.gameObject);
        }
        lineRenderer.SetPosition(1, hits[stopIndex - 1].point);
    }

    void HurtEnnemy(GameObject go)
    {
        ObjectManager.dictObjectToHitable[go].StackDamage(stats.dps);
    }

    protected override void onUse()
    {

    }
}

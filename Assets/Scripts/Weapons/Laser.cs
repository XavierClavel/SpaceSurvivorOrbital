using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Interactor
{
    [SerializeField] protected Transform firePoint;
    [SerializeField] LineRenderer lineRenderer;

    protected override void Start()
    {
        base.Start();
        lineRenderer.enabled = false;
    }



    protected override void onStartUsing()
    {
        lineRenderer.enabled = true;
        StartCoroutine(nameof(UpdateLaserBeam));
    }

    protected override void onStopUsing()
    {
        lineRenderer.enabled = false;
        StopCoroutine(nameof(UpdateLaserBeam));
    }

    IEnumerator UpdateLaserBeam()
    {
        while (true)
        {
            yield return Helpers.GetWaitFixed;

            lineRenderer.SetPosition(0, firePoint.position);

            RaycastHit2D[] hits = Physics2D.RaycastAll(firePoint.position, firePoint.right, stats.range, currentLayerMask);
            if (hits.Length == 0)
            {
                lineRenderer.SetPosition(1, firePoint.position + firePoint.right * stats.range);
                continue;
            }

            ApplyRaycast(hits);
        }
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

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
        currentLayerMask = LayerMask.GetMask(Vault.layer_resources, Vault.layer_ennemies);
        while (true)
        {
            yield return Helpers.GetWaitFixed;

            lineRenderer.SetPosition(0, firePoint.position);

            RaycastHit2D[] hits = Physics2D.RaycastAll(firePoint.position, firePoint.right, range, currentLayerMask);
            if (hits.Length == 0)
            {
                lineRenderer.SetPosition(1, firePoint.position + firePoint.right * range);
                continue;
            }

            int stopIndex = Mathf.Min(hits.Length, pierce + 1);
            lineRenderer.SetPosition(1, hits[stopIndex - 1].point);
            for (int i = 0; i < stopIndex; i++)
            {
                HurtEnnemy(hits[i].collider.gameObject);
            }
        }
    }

    void HurtEnnemy(GameObject go)
    {
        ObjectManager.dictObjectToBreakable[go].StackDamage(dps);
    }

    protected override void onUse()
    {

    }
}

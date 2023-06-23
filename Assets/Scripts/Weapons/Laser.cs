using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Interactor
{

    float dps = 50f;
    [SerializeField] LineRenderer lineRenderer;



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

            lineRenderer.SetPosition(0, aimTransform.position);

            RaycastHit2D[] hits = Physics2D.RaycastAll(aimTransform.position, aimTransform.up, range, currentLayerMask);
            if (hits.Length == 0)
            {
                lineRenderer.SetPosition(0, aimTransform.position + aimTransform.up * range);
                yield break;
            }

            lineRenderer.SetPosition(0, hits[hits.Length - 1].point);
            for (int i = 0; i < Mathf.Max(hits.Length, pierce + 1); i++)
            {
                HurtEnnemy(hits[i].collider.gameObject);
            }
        }
    }

    void HurtEnnemy(GameObject ennemyGO)
    {
        ObjectManager.dictObjectToEnnemy[ennemyGO].StackDamage(dps);
    }

    protected override void onUse()
    {
        throw new System.NotImplementedException();
    }
}

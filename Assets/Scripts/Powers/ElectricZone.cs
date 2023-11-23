using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using Shapes;
using UnityEngine;

public class ElectricZone : MonoBehaviour
{
    [SerializeField] private Disc innerDisc;
    [SerializeField] private Disc outerDisc;

    private Color innerColor;
    private Color outerColor;

    public void Setup(float range)
    {
        Destroy(gameObject,2f);
        
        innerColor = innerDisc.ColorInner;
        outerColor = innerDisc.ColorOuter;

        innerColor.a = 0;
        outerColor.a = 0;
        transform.DOScale(range, 0.5f)
            .SetDelay(0.2f)
            .SetEase(Ease.OutCubic)
            .OnComplete(delegate
            {
                DOTween.To(() => innerDisc.ColorInner, x => innerDisc.ColorInner = x, innerColor, 1f).SetEase(Ease.OutQuad);
                DOTween.To(() => innerDisc.ColorOuter, x => innerDisc.ColorOuter = x, outerColor, 1f).SetEase(Ease.OutQuad);
                transform.DOScale(range * 0.8f, 1f);
            });
    }
}

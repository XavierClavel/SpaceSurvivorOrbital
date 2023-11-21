using System.Collections;
using System.Collections.Generic;
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
        innerColor = innerDisc.Color;
        outerColor = innerDisc.Color;

        innerColor.a = 0;
        outerColor.a = 0;
        transform.DOScale(range, 1f).OnComplete(delegate
        {
            DOTween.To(() => innerDisc.Color, x => innerDisc.Color = x, innerColor, 1f).SetEase(Ease.OutQuad);
            DOTween.To(() => outerDisc.Color, x => outerDisc.Color = x, outerColor, 1f).SetEase(Ease.OutQuad);
        });
    }
}

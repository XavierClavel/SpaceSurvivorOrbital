using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamageDisplayHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI damageDisplayPrefab;
    static DamageDisplayHandler instance;
    static Vector3 scaleTarget = new Vector3(1.5f, 1.5f, 1.5f);
    static Vector3 exitScale = new Vector3(0.5f, 0.5f, 0.5f);
    static Color targetColor_white = new Color(1f, 1f, 1f, 0f);
    static Color targetColor_red = new Color(1f, 0f, 0f, 0f);

    private void Start()
    {
        instance = this;

    }

    public static void DisplayDamage(int damage, Vector2 position, bool critical = false)
    {
        TextMeshProUGUI damageDisplay = Instantiate(instance.damageDisplayPrefab, position, Quaternion.identity);
        damageDisplay.text = damage.ToString();
        GameObject displayObject = damageDisplay.gameObject;
        RectTransform displayTransform = displayObject.GetComponent<RectTransform>();
        displayTransform.DOMoveY(displayTransform.position.y + 2f, 2f);
        if (critical) damageDisplay.color = Color.red;
        Color targetColor = critical ? targetColor_red : targetColor_white;
        damageDisplay.DOColor(targetColor, 1f).SetEase(Ease.InQuad);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(displayTransform.DOScale(scaleTarget, 0.25f));
        sequence.Append(displayTransform.DOScale(exitScale, 1.5f)).SetEase(Ease.OutQuad);
        Helpers.instance.WaitAndKill(2f, displayObject);
    }

}

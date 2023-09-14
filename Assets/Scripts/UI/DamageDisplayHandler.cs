using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public enum healthChange { hit, critical, heal, poison, fire };

public class DamageDisplayHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI damageDisplayPrefab;
    [SerializeField] Canvas canvas;
    static DamageDisplayHandler instance;
    static Vector3 scaleTarget = new Vector3(1.5f, 1.5f, 1.5f);
    static Vector3 exitScale = new Vector3(0.5f, 0.5f, 0.5f);
    static Color targetColor_white = new Color(1f, 1f, 1f, 0f);
    static Color targetColor_red = new Color(1f, 0f, 0f, 0f);
    static Color targetColor_green = new Color(0f, 1f, 0f, 0f);
    static Color poisonColor = new Color(0.0627f, 0.9215f, 0.70588f);
    static Color target_poisonColor = new Color(0.0627f, 0.9215f, 0.70588f, 0f);
    static Color fireColor = new Color(0.9411f, 0.6f, 0.08627f);
    static Color target_fireColor = new Color(0.9411f, 0.6f, 0.08627f, 0f);

    private void Start()
    {
        instance = this;

    }

    public static void DisplayDamage(int damage, Vector2 position, healthChange type = healthChange.hit)
    {
        GameObject container = new GameObject();
        container.transform.position = new Vector3(position.x, position.y, 2f);
        TextMeshProUGUI damageDisplay = Instantiate(instance.damageDisplayPrefab, position, Quaternion.identity);
        damageDisplay.transform.SetParent(container.transform);

        damageDisplay.text = damage.ToString();
        GameObject displayObject = damageDisplay.gameObject;
        RectTransform displayTransform = displayObject.GetComponent<RectTransform>();
        //displayTransform.DOMoveY(displayTransform.position.y + 2f, 2f);
        Color targetColor;
        switch (type)
        {
            case healthChange.hit:
                targetColor = targetColor_white;
                break;
            case healthChange.critical:
                damageDisplay.color = Color.red;
                targetColor = targetColor_red;
                break;
            case healthChange.heal:
                damageDisplay.color = Color.green;
                targetColor = targetColor_green;
                damageDisplay.text = "+" + damage.ToString();
                break;

            case healthChange.poison:
                damageDisplay.color = poisonColor;
                targetColor = target_poisonColor;
                break;

            case healthChange.fire:
                damageDisplay.color = fireColor;
                targetColor = target_fireColor;
                break;

            default:
                targetColor = targetColor_white;
                break;
        }
        //damageDisplay.DOColor(targetColor, 1f).SetEase(Ease.InQuad);
        //Sequence sequence = DOTween.Sequence();


        //sequence.Append(displayTransform.DOScale(scaleTarget, 0.25f));

        //sequence.Append(displayTransform.DOScaleX(scaleTarget.x, 0.25f));
        //sequence.Append(displayTransform.DOScaleY(scaleTarget.y, 0.25f));

        //sequence.Append(displayTransform.DOScale(exitScale, 1.5f)).SetEase(Ease.OutQuad);
        Destroy(container, 1f);
    }

}

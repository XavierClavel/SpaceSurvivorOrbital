using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageDisplay : MonoBehaviour
{
    static Color poisonColor = new Color(0.0627f, 0.9215f, 0.70588f);
    static Color fireColor = new Color(0.9411f, 0.6f, 0.08627f);
    
    
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TextMeshProUGUI textDisplay;
    [SerializeField] private Animator animator;
    private ComponentPool<DamageDisplay> pool;
    private GameObject target = null;
    private int damage = 0;
    private static readonly int Reset = Animator.StringToHash("Reset");

    public DamageDisplay setPool(ComponentPool<DamageDisplay> pool)
    {
        this.pool = pool;
        return this;
    }

    private void recall()
    {
        if (target != null) DamageDisplayHandler.dictObjectToDisplay.Remove(target);
        target = null;
        pool.recall(this);
    }

    public DamageDisplay setPosition(Vector2 position)
    {
        transform.position = position;
        return this;
    }

    public DamageDisplay setTarget(GameObject target)
    {
        this.target = target;
        DamageDisplayHandler.dictObjectToDisplay[target] = this;
        return this;
    }

    public DamageDisplay updateValue(int value)
    {
        damage += value;
        CancelInvoke(nameof(recall));
        animator.SetTrigger(Reset);
        return setValue(damage);
    }

    public DamageDisplay setValue(int value, healthChange type = healthChange.hit)
    {
        this.damage = value;
        textDisplay.SetText(type == healthChange.heal ? $"+{value}" : value.ToString());
        Invoke(nameof(recall),0.5f);
        return this;
    }

    public DamageDisplay setColor(healthChange type)
    {
        switch (type)
        {
            case healthChange.critical:
                textDisplay.color = Color.red;
                break;
            case healthChange.heal:
                textDisplay.color = Color.green;
                break;

            case healthChange.poison:
                textDisplay.color = poisonColor;
                break;

            case healthChange.fire:
                textDisplay.color = fireColor;
                break;
        }

        return this;
    }
}

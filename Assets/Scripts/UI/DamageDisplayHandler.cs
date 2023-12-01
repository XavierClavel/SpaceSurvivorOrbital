using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public enum healthChange { hit, critical, heal, poison, fire };

public class DamageDisplayHandler : MonoBehaviour
{
    [SerializeField] DamageDisplay damageDisplayPrefab;
    [SerializeField] GameObject canvas;
    static DamageDisplayHandler instance;

    public static Dictionary<GameObject, DamageDisplay> dictObjectToDisplay =
        new Dictionary<GameObject, DamageDisplay>();

    private static ComponentPool<DamageDisplay> damageDisplayPool;


    private void Start()
    {
        instance = this;
        dictObjectToDisplay = new Dictionary<GameObject, DamageDisplay>();
        damageDisplayPool = new ComponentPool<DamageDisplay>(damageDisplayPrefab);
    }

    public static void DisplayStackedDamage(GameObject target, int damage)
    {
        if (!dictObjectToDisplay.ContainsKey(target))
        {
            DamageDisplay damageDisplay = DisplayDamage(damage, target.transform.position);
            damageDisplay.setTarget(target);
        }
        else
        {
            DamageDisplay damageDisplay = dictObjectToDisplay[target];
            damageDisplay.setPosition(target.transform.position + 0.5f * Vector3.up)
                .updateValue(damage);
        }
    }

    public static DamageDisplay DisplayDamage(int damage, Vector2 position, healthChange type = healthChange.hit)
    {
        DamageDisplay damageDisplay = damageDisplayPool.get(position + 0.5f * Vector2.up);
        damageDisplay
            .setPool(damageDisplayPool)
            .setColor(type)
            .setValue(damage, type);
        return damageDisplay;
    }
    
}

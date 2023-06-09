using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Breakable : MonoBehaviour
{
    [SerializeField] objects objectType;
    protected int maxHealth = 150;
    protected float baseSpeed;
    protected float damageResistance;
    protected int baseDamage;


    [SerializeField] protected SpriteRenderer spriteOverlay;
    float stackedDamage = 0f;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        ObjectManager.dictObjectToBreakable.Add(gameObject, this);
        ObjectData objectData = DataManager.dictObjects[objectType];
        maxHealth = objectData.maxHealth;
        baseSpeed = objectData.baseSpeed;
        damageResistance = objectData.damageResistance;
        baseDamage = objectData.baseDamage;
    }


    public virtual void Hit(int damage, status effect, bool critical)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(spriteOverlay.DOColor(Color.white, 0.1f));
        sequence.Append(spriteOverlay.DOColor(Helpers.color_whiteTransparent, 0.1f));
    }

    public void StackDamage(float dps)
    {
        stackedDamage += dps * Time.fixedDeltaTime;
        if (stackedDamage < 1f) return;

        int damage = (int)stackedDamage;
        stackedDamage -= damage;
        Hit(damage, status.none, false);
    }

    protected virtual void OnDestroy()
    {
        ObjectManager.dictObjectToBreakable.Remove(gameObject);
    }


}

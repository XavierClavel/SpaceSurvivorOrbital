using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Breakable : MonoBehaviour
{
    [SerializeField] private string key;
    protected int maxHealth = 150;
    protected Vector2Int baseSpeed;
    protected float damageResistance;
    protected Vector2Int baseDamage;
    protected int cost;

    [SerializeField] protected SpriteRenderer spriteOverlay;
    float stackedDamage = 0f;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        key = key.Trim();
        ObjectManager.dictObjectToHitable.Add(gameObject, this);
        if (!DataManager.dictObjects.ContainsKey(key))
        {
            foreach (var it in DataManager.dictObjects)
            {
                Debug.Log($"\"{it.Key}\"");
            }
            throw new System.ArgumentException($"Key \"{key}\" used for gameObject \"{gameObject.name}\" does not exist in file ObjectData.csv");
        }
        ObjectData objectData = DataManager.dictObjects[key];
        maxHealth = objectData.maxHealth;
        baseSpeed = objectData.baseSpeed;
        damageResistance = objectData.damageResistance;
        baseDamage = objectData.baseDamage;
        cost = objectData.cost;

    }

    public virtual void Hit(HitInfo hitInfo)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(spriteOverlay.DOColor(Color.white, 0.1f));
        sequence.Append(spriteOverlay.DOColor(Helpers.color_whiteTransparent, 0.1f));
    }


    public virtual void Hit(int damage, status effect, bool critical)
    {
        Hit(new HitInfo(damage, critical, effect));
    }

    protected virtual void StackHit(int damage, HashSet<status> elements)
    {
        
    }

    public virtual void StackDamage(float dps, HashSet<status> elements)
    {
        if (dps <= 0f) return;
        stackedDamage += dps * Time.fixedDeltaTime;
        if (stackedDamage < 1f) return;

        int damage = (int)stackedDamage;
        stackedDamage -= damage;
        StackHit(damage, elements);
        //Hit(damage, element, false);
    }

    protected virtual void OnDestroy()
    {
        ObjectManager.unregisterHitable(gameObject);
    }


}

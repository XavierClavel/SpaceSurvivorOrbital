using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectData
{
    public int maxHealth = 100;
    public float baseSpeed = 3.5f;
    public float damageResistance = 0f;
    public int cost = 0;
    public Vector2Int baseDamage = new Vector2Int(5, 5);
}

public class ObjectDataBuilder : DataBuilder<ObjectData>
{
    protected override ObjectData BuildData(List<string> s)
    {

        ObjectData value = new ObjectData();

        SetValue(ref value.maxHealth, Vault.key.upgrade.MaxHealth);
        SetValue(ref value.baseDamage, Vault.key.upgrade.BaseDamage);
        SetValue(ref value.baseSpeed, Vault.key.upgrade.BaseSpeed);
        SetValue(ref value.damageResistance, Vault.key.upgrade.DamageResistance);
        SetValue(ref value.cost, Vault.key.upgrade.Cost);

        return value;
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpike : MonoBehaviour
{
    private Vector2Int baseDamage;

    public IceSpike setup(Vector2Int baseDamage)
    {
        this.baseDamage = baseDamage;
        return this;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag(Vault.tag.Ennemy)) return;
        HitInfo hitInfo = new HitInfo(Power.getDamage(baseDamage), false, status.ice);
        
        SoundManager.PlaySfx(transform, key: "Ennemy_Hit");
        ObjectManager.HitObject(other.gameObject, hitInfo);
    }
}

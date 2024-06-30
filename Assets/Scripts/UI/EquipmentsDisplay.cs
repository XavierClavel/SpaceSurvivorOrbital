using System;
using System.Linq;
using UnityEngine;
    
public class EquipmentsDisplay : ItemGridDisplay<EquipmentHandler>
{
    protected override string getKey(EquipmentHandler item)
    {
        return item.getKey();
    }

    protected override Sprite getSprite(EquipmentHandler item)
    {
        return item.getIcon();
    }

    private void Start()
    {
        addItem(PlayerManager.equipments.ToList());
    }
    
}

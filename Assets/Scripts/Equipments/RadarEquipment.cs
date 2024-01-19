using UnityEngine;


public class RadarEquipment : Equipment
{
    protected override void onUse()
    {
        ObjectManager.ActivateRadar();
    }
}

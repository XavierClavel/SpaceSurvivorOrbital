using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum handlerVisibility {ShowInDemo, ShowInFullGame, Hide}

public class ObjectHandler : ScriptableObject
{
    [SerializeField] protected string key;
    [SerializeField] protected Sprite icon;

    public string getKey() { return key.Trim(); }
    public Sprite getIcon() { return icon; }
}

public class HidableObjectHandler : ObjectHandler
{
    [SerializeField] private handlerVisibility visibility = handlerVisibility.ShowInFullGame;
    public bool canBeSelected()
    {
        return visibility == handlerVisibility.ShowInDemo ||
            (visibility == handlerVisibility.ShowInFullGame && !PlayerManager.isDemo);
    }
}


[CreateAssetMenu(fileName = "PowerHandler", menuName = Vault.other.scriptableObjectMenu + "PowerHandler", order = 0)]
public class PowerHandler : HidableObjectHandler
{
    [SerializeField] private Power power;

    public Power getPower() { return power; }

    public void Activate()
    {
        Power instance = GameObject.Instantiate(power);
        instance.Setup(PlayerManager.dictKeyToStats[key].Clone());
        DisplayPower();
    }

    private void DisplayPower()
    {
        PowerDisplay powerDisplay = ObjectManager.AddPowerDisplay();
        powerDisplay.setSprite(getIcon());
    }
}
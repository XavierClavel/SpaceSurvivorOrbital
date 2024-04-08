using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum handlerVisibility {ShowInDemo, HideInDemo, Hide}
public enum availability {Start, Boss1, Boss2, Boss3, Boss4, Boss5}

public class ObjectHandler : ScriptableObject
{
    [SerializeField] protected string key;
    [SerializeField] protected Sprite icon;

    public string getKey() { return key.Trim(); }
    public Sprite getIcon() { return icon; }
}

public class HidableObjectHandler : ObjectHandler
{
    [SerializeField] private handlerVisibility visibility = handlerVisibility.HideInDemo;
    [SerializeField] private availability availableAfter = availability.Start;
    public bool canBeSelected()
    {
        if (visibility == handlerVisibility.Hide) return false;
        if (visibility == handlerVisibility.HideInDemo && PlayerManager.isDemo) return false;
        return true;
    }

    public bool isDiscovered()
    {
        if (DebugManager.doOverrideProgressionUnlocks()) return true;
        return SaveManager.getProgression().Contains(availableAfter);
    }

    public bool isDiscoveredAt(availability gameProgress)
    {
        Debug.Log(availableAfter == gameProgress);
        return availableAfter == gameProgress;
    }

    public availability getAvailibility() => availableAfter;
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
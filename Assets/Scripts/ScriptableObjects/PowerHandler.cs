using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHandler : ScriptableObject
{
    [SerializeField] protected string key;
    [SerializeField] protected Sprite icon;

    public string getKey() { return key.Trim(); }
    public Sprite getIcon() { return icon; }
}


[CreateAssetMenu(fileName = "PowerHandler", menuName = Vault.other.scriptableObjectMenu + "PowerHandler", order = 0)]
public class PowerHandler : ObjectHandler
{
    [SerializeField] private Power power;

    public Power getPower() { return power; }

    public void Activate()
    {
        interactorStats stats = PlayerManager.dictKeyToStats[key].interactor;
        Power instance = GameObject.Instantiate(power);
        instance.Setup(stats);
    }
}
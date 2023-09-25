using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerHandler", menuName = Vault.other.scriptableObjectMenu + "PowerHandler", order = 0)]
public class PowerHandler : ScriptableObject
{
    [SerializeField] private string key;
    [SerializeField] private Sprite icon;
    [SerializeField] private Power power;

    public string getKey() {return key;}
    public Sprite getIcon() {return icon;}
    public Power getPower() {return power;}

    public void Activate() {
        interactorStats stats = PlayerManager.dictKeyToStats[key];
        Power instance = GameObject.Instantiate(power);
        instance.Setup(stats);
    }
}
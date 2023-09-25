using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerHandler", menuName = Vault.other.scriptableObjectMenu + "PowerHandler", order = 0)]
public class PowerHandler : ScriptableObject
{
    [SerializeField] private string name;
    [SerializeField] private Sprite icon;
    [SerializeField] private Power power;

    public string getKey() {return name;}
    public Sprite getIcon() {return icon;}
    public Power getPower() {return power;}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ButtonSprite", menuName = Vault.other.scriptableObjectMenu + "ButtonSprite", order = 0)]
public class ButtonSprite : ScriptableObject
{
    public string key;
    public Sprite available;
    public Sprite purchased;
    public Sprite locked;
}

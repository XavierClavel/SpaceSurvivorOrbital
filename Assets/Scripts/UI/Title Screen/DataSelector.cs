using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DataSelector : MonoBehaviour, UIPanel
{
    [SerializeField] Button startButton;
    public static string selectedCharacter = string.Empty;
    public static string selectedWeapon = string.Empty;

    public RectTransform getUITransform()
    {
        return GetComponent<RectTransform>();
    }

    public static void Reset()
    {
        selectedCharacter = string.Empty;
        selectedWeapon = string.Empty;
    }

    public void SelectGeneric<T>(T value) where T : ObjectHandler
    {
        if (typeof(T) == typeof(CharacterHandler)) SelectCharacter(value.getKey());
        else if (typeof(T) == typeof(WeaponHandler)) SelectWeapon(value.getKey());
    }

    public void SelectCharacter(string value)
    {
        selectedCharacter = value;
        if (selectedWeapon != string.Empty) startButton.interactable = true;
    }

    public void SelectWeapon(string value)
    {
        selectedWeapon = value;
        if (selectedCharacter != string.Empty) startButton.interactable = true;
    }

    public void Validate()
    {
        PlayerData data = new PlayerData();
        data.interactor = DataManager.dictWeapons[selectedWeapon];
        PlayerManager.setWeapon(data, ScriptableObjectManager.dictKeyToWeaponHandler[selectedWeapon]);
    }

    public static WeaponHandler getSelectedWeapon()
    {
        return ScriptableObjectManager.dictKeyToWeaponHandler[selectedWeapon];
    }

    public static CharacterHandler getSelectedCharacter()
    {
        return ScriptableObjectManager.dictKeyToCharacterHandler[selectedCharacter];
    }
}
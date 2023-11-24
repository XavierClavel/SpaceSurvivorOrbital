using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DataSelector : MonoBehaviour, UIPanel
{
    private static DataSelector instance;
    [SerializeField] Button startButton;
    public static string selectedCharacter = string.Empty;
    public static string selectedWeapon = string.Empty;

    private void Awake()
    {
        instance = this;
    }

    public RectTransform getUITransform()
    {
        return GetComponent<RectTransform>();
    }

    public static Sprite getIcon(string key)
    {
        if (ScriptableObjectManager.dictKeyToCharacterHandler.ContainsKey(key))
        {
            return ScriptableObjectManager.dictKeyToCharacterHandler[key].getIcon();
        }

        if (ScriptableObjectManager.dictKeyToWeaponHandler.ContainsKey(key))
        {
            return ScriptableObjectManager.dictKeyToWeaponHandler[key].getIcon();
        }
        
        Debug.LogWarning("Selected key does not exist");
        return null;
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
    
    public static void SelectGeneric(string value) 
    {
        if (ScriptableObjectManager.dictKeyToCharacterHandler.ContainsKey(value))
        {
            instance.SelectCharacter(value);
            return;
        }

        if (ScriptableObjectManager.dictKeyToWeaponHandler.ContainsKey(value))
        {
            instance.SelectWeapon(value);
            return;
        }
        
        Debug.LogWarning("Selected key does not exist");
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
        data.interactor.DuckCopyShallow(DataManager.dictWeapons[selectedWeapon]);
        PlayerManager.setWeapon(data, ScriptableObjectManager.dictKeyToWeaponHandler[selectedWeapon]);
        SceneTransitionManager.TransitionToScene(gameScene.planetJungle);
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DataSelector : MonoBehaviour
{
    [SerializeField] ObjectReferencer objectReferencer;
    [SerializeField] Button startButton;
    character selectedCharacter = character.None;
    weapon selectedWeapon = weapon.None;
    tool selectedTool = tool.None;

    public void SelectGeneric<TEnum>(int value) where TEnum : struct, IConvertible, IComparable, IFormattable
    {
        if (typeof(TEnum) == typeof(character)) SelectCharacter(value);
        else if (typeof(TEnum) == typeof(weapon)) SelectWeapon(value);
        else if (typeof(TEnum) == typeof(tool)) SelectTool(value);
    }

    public void SelectCharacter(character value)
    {
        this.selectedCharacter = value;
        if (selectedWeapon != weapon.None) startButton.interactable = true;
    }

    public void SelectCharacter(int value)
    {
        SelectCharacter((character)value);
    }

    public void SelectWeapon(weapon value)
    {
        this.selectedWeapon = value;
        if (selectedCharacter != character.None) startButton.interactable = true;
    }

    public void SelectWeapon(int value)
    {
        SelectWeapon((weapon)value);
    }

    public void SelectTool(tool value)
    {
        this.selectedTool = value;
        if (selectedCharacter != character.None && selectedWeapon != weapon.None) startButton.interactable = true;
    }

    public void SelectTool(int value)
    {
        SelectTool((tool)value);
    }

    public void Validate()
    {
        PlayerManager.setWeapon(DataManager.dictWeapons[selectedWeapon].interactorStats, objectReferencer.getInteractor(selectedWeapon));
        if (selectedTool != tool.None) PlayerManager.setTool(DataManager.dictTools[selectedTool].interactorStats, objectReferencer.getInteractor(selectedTool));
    }
}
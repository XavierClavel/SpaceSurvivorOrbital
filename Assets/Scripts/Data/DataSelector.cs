using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataSelector : MonoBehaviour
{
    [SerializeField] ObjectReferencer objectReferencer;
    [SerializeField] Button startButton;
    character selectedCharacter = character.None;
    weapon selectedWeapon = weapon.None;
    weapon selectedTool = weapon.None;

    public void SelectCharacter(character value)
    {
        this.selectedCharacter = value;
        if (selectedWeapon != weapon.None) startButton.interactable = true;
    }

    public void SelectWeapon(weapon value)
    {
        this.selectedWeapon = value;
        if (selectedCharacter != character.None) startButton.interactable = true;
    }

    public void SelectTool(weapon value)
    {
        this.selectedTool = value;
    }

    public void Validate()
    {
        PlayerManager.setWeapon(DataManager.dictInteractors[selectedWeapon].interactorStats, objectReferencer.getInteractor(selectedWeapon));
        if (selectedTool != weapon.None) PlayerManager.setTool(DataManager.dictInteractors[selectedTool].interactorStats, objectReferencer.getInteractor(selectedWeapon));
    }
}
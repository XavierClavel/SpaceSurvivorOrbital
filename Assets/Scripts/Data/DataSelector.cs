using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSelector : MonoBehaviour
{
    CharacterData selectedCharacter;
    InteractorData selectedWeapon;
    InteractorData selectedTool;

    public void OnSelectCharacter(CharacterData characterData)
    {
        this.selectedCharacter = characterData;
    }

    public void OnSelectWeapon(InteractorData interactorData)
    {
        this.selectedWeapon = interactorData;
    }

    public void OnSelectTool(InteractorData interactorData)
    {
        this.selectedTool = interactorData;
    }

    public void onValidate()
    {
        //selectedCharacter.Apply();
        //selectedWeapon.Apply();
        //if (selectedTool != null) selectedTool.Apply();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterData : PlayerData
{

    public CharacterData(List<string> s)
    {
        setEffects(s);

        if (name == Vault.character_Base) DataManager.baseStats = this;
        else
        {
            character currentCharacter = (character)System.Enum.Parse(typeof(character), name, true);
            DataManager.dictCharacters.Add(currentCharacter, this);
        }
    }

    public static void Initialize(List<string> s)
    {
        OverrideInitialize(s);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterManager
{
    public static int getHealth()
    {
        if (DataSelector.selectedCharacter == "") return 4;
        return 3;
    }

    public static int getShields()
    {
        if (DataSelector.selectedCharacter == "") return 2;
        return 0;
    }
}

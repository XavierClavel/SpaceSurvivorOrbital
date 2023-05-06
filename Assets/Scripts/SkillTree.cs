using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{
    private void Awake()
    {
        if (!PlayerManager.isPlayingWithGamepad) Cursor.visible = true;
        SkillButton.greenRessource = PlayerManager.amountGreen;
        SkillButton.yellowRessource = PlayerManager.amountOrange;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillTree : MonoBehaviour
{
    [SerializeField] EventSystem eventSystem;
    static SkillTree instance;
    private void Awake()
    {
        instance = this;
        if (!PlayerManager.isPlayingWithGamepad) Cursor.visible = true;
        SkillButton.greenRessource = PlayerManager.amountGreen;
        SkillButton.yellowRessource = PlayerManager.amountOrange;
    }

    public static void setSelectedButton(Button button)
    {
        instance.eventSystem.SetSelectedGameObject(button.gameObject);
    }
}

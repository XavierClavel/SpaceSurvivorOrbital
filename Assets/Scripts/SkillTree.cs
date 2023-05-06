using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class SkillTree : MonoBehaviour
{
    [SerializeField] EventSystem eventSystem;
    static SkillTree instance;
    public static List<SkillButton> skillButtons = new List<SkillButton>();
    public static List<skillButtonStatus> skillButtonStatuses;
    [SerializeField] GameObject buttonsContainer;

    private void Awake()
    {
        instance = this;
        skillButtons = buttonsContainer.GetComponentsInChildren<SkillButton>().ToList();

        if (skillButtonStatuses == null)
        {
            skillButtonStatuses = new List<skillButtonStatus>();
            foreach (SkillButton skillButton in skillButtons)
            {
                skillButtonStatuses.Add(skillButton.isFirst ? skillButtonStatus.unlocked : skillButtonStatus.locked);
            }
        }

        if (!PlayerManager.isPlayingWithGamepad) Cursor.visible = true;
        SkillButton.greenRessource = PlayerManager.amountGreen;
        SkillButton.yellowRessource = PlayerManager.amountOrange;
    }

    public static void UpdateButton(SkillButton skillButton, skillButtonStatus newStatus)
    {
        int index = skillButtons.IndexOf(skillButton);
        Debug.Log(index);
        Debug.Log(skillButtonStatuses.Count);
        skillButtonStatuses[index] = newStatus;
    }

    public static void UpdateList(List<SkillButton> skillButtons, skillButtonStatus newStatus)
    {
        foreach (SkillButton skillButton in skillButtons)
        {
            UpdateButton(skillButton, newStatus);
        }
    }

    private void Start()
    {
        for (int i = 0; i < skillButtonStatuses.Count; i++)
        {
            skillButtons[i].button.interactable = skillButtonStatuses[i] == skillButtonStatus.unlocked;
        }

    }

    public static void setSelectedButton(Button button)
    {
        instance.eventSystem.SetSelectedGameObject(button.gameObject);
    }
}

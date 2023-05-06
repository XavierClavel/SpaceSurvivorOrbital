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
                skillButtonStatus status = skillButton.isFirst ? skillButtonStatus.unlocked : skillButtonStatus.locked;
                skillButtonStatuses.Add(status);
            }
        }

        if (!PlayerManager.isPlayingWithGamepad) Cursor.visible = true;
        SkillButton.greenRessource = PlayerManager.amountGreen;
        SkillButton.yellowRessource = PlayerManager.amountOrange;
    }

    public static void UpdateButton(SkillButton skillButton, skillButtonStatus newStatus)
    {
        int index = skillButtons.IndexOf(skillButton);
        skillButtonStatuses[index] = newStatus;
        skillButton.UpdateStatus(newStatus);
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
            //skillButtons[i].button.interactable = skillButtonStatuses[i] == skillButtonStatus.unlocked;
            skillButtons[i].UpdateStatus(skillButtonStatuses[i]);
        }

    }
}

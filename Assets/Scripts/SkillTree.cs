using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.InputSystem;
using DG.Tweening;

public class SkillTree : MonoBehaviour
{
    [SerializeField] EventSystem eventSystem;
    static SkillTree instance;
    public static List<SkillButton> skillButtons = new List<SkillButton>();
    public static List<skillButtonStatus> skillButtonStatuses;
    [SerializeField] GameObject buttonsContainer;
    [SerializeField] ScrollRect scrollRect;
    RectTransform scrollRectTransform;
    RectTransform contentPanel;
    GameObject previousSelected;


    private void Awake()
    {
        scrollRectTransform = scrollRect.GetComponent<RectTransform>();
        contentPanel = scrollRect.content;

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

    private void Update()
    {
        //TODO : make input switch from playerManager
        if (!PlayerManager.isPlayingWithGamepad) return;
        if (eventSystem.currentSelectedGameObject == previousSelected) return;
        previousSelected = eventSystem.currentSelectedGameObject;
        RectTransform selectedRectTransform = eventSystem.currentSelectedGameObject.GetComponent<RectTransform>();
        // The position of the selected UI element is the absolute anchor position,
        // ie. the local position within the scroll rect + its height if we're
        // scrolling down. If we're scrolling up it's just the absolute anchor position.
        //float selectedPositionY = Mathf.Abs(selectedRectTransform.anchoredPosition.y) + selectedRectTransform.rect.height;
        float selectedPositionY = selectedRectTransform.anchoredPosition.y + selectedRectTransform.rect.height;
        // The upper bound of the scroll view is the anchor position of the content we're scrolling.
        float scrollViewMinY = contentPanel.anchoredPosition.y;
        // The lower bound is the anchor position + the height of the scroll rect.
        float scrollViewMaxY = contentPanel.anchoredPosition.y + scrollRectTransform.rect.height;

        // If the selected position is below the current lower bound of the scroll view we scroll down.
        if (selectedPositionY > scrollViewMaxY)
        {
            float newY = selectedPositionY - scrollRectTransform.rect.height;
            contentPanel.DOAnchorPos(new Vector2(contentPanel.anchoredPosition.x, newY), 0.5f);
        }
        // If the selected position is above the current upper bound of the scroll view we scroll up.
        else if (selectedRectTransform.anchoredPosition.y < scrollViewMinY)
        {
            contentPanel.DOAnchorPos(new Vector2(contentPanel.anchoredPosition.x, Mathf.Abs(selectedRectTransform.anchoredPosition.y)), 0.5f);
        }
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
            skillButtons[i].UpdateStatus(skillButtonStatuses[i]);
        }

    }
}

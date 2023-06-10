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
    [SerializeField] TreePanelsManager treePanelsManager;
    static SkillTree instance;
    public static List<SkillButton> skillButtons = new List<SkillButton>();
    public static List<skillButtonStatus> skillButtonStatuses;
    [SerializeField] GameObject buttonsContainer;
    [SerializeField] ScrollRect scrollRect;
    RectTransform scrollRectTransform;
    RectTransform contentPanel;
    GameObject previousSelected;
    InputMaster inputActions;


    private void Awake()
    {


        SetupDisplay();


        inputActions = new InputMaster();
        inputActions.UI.Navigate.started += ctx => StartCoroutine(nameof(OnNavigate));
        inputActions.UI.Navigate.canceled += ctx => StopCoroutine(nameof(OnNavigate));
        inputActions.UI.Navigate.performed += xtc => UpdateDisplay();
        inputActions.Enable();

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

    void KillAllChildren()
    {
        foreach (Transform child in buttonsContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    void SetupDisplay()
    {
        KillAllChildren();
        AddPanelToDisplay(treePanelsManager.getCharacterPanel());
        AddPanelToDisplay(treePanelsManager.getWeaponPanel());
        AddPanelToDisplay(treePanelsManager.getToolPanel());
        AddPanelToDisplay(treePanelsManager.getShipPanel());

        GameObject panelPower1 = treePanelsManager.getPower1();
        GameObject panelPower2 = treePanelsManager.getPower2();

        if (panelPower1 != null) AddPanelToDisplay(panelPower1);
        if (panelPower2 != null) AddPanelToDisplay(panelPower2);

        AddPanelToDisplay(treePanelsManager.getNext());
    }

    void AddPanelToDisplay(GameObject panel)
    {
        GameObject newPanel = Instantiate(panel);
        newPanel.transform.SetParent(buttonsContainer.transform);
        newPanel.transform.SetSiblingIndex(buttonsContainer.transform.childCount - 2);
        RectTransform newPanelRectTransform = newPanel.GetComponent<RectTransform>();
        newPanelRectTransform.localScale = Vector3.one;
        newPanelRectTransform.anchoredPosition3D = new Vector3(newPanelRectTransform.anchoredPosition.x, newPanelRectTransform.anchoredPosition.y, 0f);
    }


    IEnumerator OnNavigate()
    {
        while (true)
        {
            UpdateDisplay();
            yield return null;
        }
    }

    void UpdateDisplay()
    {
        if (eventSystem.currentSelectedGameObject == previousSelected) return;

        previousSelected = eventSystem.currentSelectedGameObject;
        RectTransform selectedRectTransform = eventSystem.currentSelectedGameObject.GetComponent<RectTransform>();
        RectTransform parentRectTransform = selectedRectTransform.parent.GetComponent<RectTransform>(); ;
        // The position of the selected UI element is the absolute anchor position,
        // ie. the local position within the scroll rect + its height if we're
        // scrolling down. If we're scrolling up it's just the absolute anchor position.
        //float selectedPositionY = Mathf.Abs(selectedRectTransform.anchoredPosition.y) + selectedRectTransform.rect.height;
        float selectedPositionY = selectedRectTransform.anchoredPosition.y + selectedRectTransform.rect.height;
        selectedPositionY += parentRectTransform.anchoredPosition.y;
        // The upper bound of the scroll view is the anchor position of the content we're scrolling.
        float scrollViewBottom = contentPanel.anchoredPosition.y;
        // The lower bound is the anchor position + the height of the scroll rect.
        float scrollViewTop = contentPanel.anchoredPosition.y + scrollRectTransform.rect.height;

        //selectedPositionY *= -1f;


        // If the selected position is below the current lower bound of the scroll view we scroll down.
        if (selectedPositionY > scrollViewTop)
        {
            float newY = selectedPositionY - scrollRectTransform.rect.height - 800f;
            //contentPanel.anchoredPosition = new Vector2(contentPanel.anchoredPosition.x, newY);
            contentPanel.DOAnchorPos(new Vector2(contentPanel.anchoredPosition.x, newY), 0.5f);
        }
        // If the selected position is above the current upper bound of the scroll view we scroll up.
        else if (selectedPositionY < scrollViewBottom)
        {
            //contentPanel.DOAnchorPos(new Vector2(contentPanel.anchoredPosition.x, Mathf.Abs(selectedRectTransform.anchoredPosition.y)), 0.5f);
            contentPanel.DOAnchorPos(new Vector2(contentPanel.anchoredPosition.x, -selectedPositionY - 500f), 0.5f);
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

    private void OnDisable()
    {
        inputActions.Disable();
    }
}

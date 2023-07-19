using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PanelSelector : MonoBehaviour
{
    [SerializeField] ObjectReferencer objectReferencer;
    [SerializeField] List<NodeManager> panels;
    NodeManager currentActivePanel;
    [SerializeField] List<Button> buttons;

    [Header("Default display")]
    [SerializeField] character defaultCharacter = character.Pistolero;
    [SerializeField] weapon defaultWeapon = weapon.Gun;
    [SerializeField] tool defaultTool = tool.Pickaxe;
    EventSystem eventSystem;
    InputMaster inputActions;


    // Start is called before the first frame update
    void Start()
    {
        currentActivePanel = panels[0];

        if (DataSelector.selectedCharacter == character.None)   //Default buttons sprites if game launched from ship scene
        {
            DataSelector.selectedCharacter = defaultCharacter;
            DataSelector.selectedWeapon = defaultWeapon;
            DataSelector.selectedTool = defaultTool;
        }

        if (DataSelector.selectedTool == tool.None)
        {
            panels[2].gameObject.SetActive(false);
            panels.RemoveAt(2);
        }

        NodeManager.dictKeyToButton = new Dictionary<string, TreeButton>();

        foreach (NodeManager nodeManager in panels)
        {
            nodeManager.Initialize();
            nodeManager.gameObject.SetActive(nodeManager == currentActivePanel);
        }

        SetPanelsTarget();
        SetPanelSelectionButtonsSprite();


        eventSystem = EventSystem.current;
        InputManager.setSelectedObject(currentActivePanel.firstSelectedButton);
        eventSystem.SetSelectedGameObject(currentActivePanel.firstSelectedButton);
        eventSystem.firstSelectedGameObject = currentActivePanel.firstSelectedButton;

        /*
        inputActions = new InputMaster();
        inputActions.UI.Navigate.started += ctx => StartCoroutine(nameof(OnNavigate));
        inputActions.UI.Navigate.canceled += ctx => StopCoroutine(nameof(OnNavigate));
        inputActions.UI.Navigate.performed += xtc => UpdateDisplay();
        inputActions.Enable();
        µ*/
    }

    void SetPanelsTarget()
    {
        panels[0].target = Vault.key.target.Pistolero;
        panels[1].target = Vault.key.target.Gun;
        panels[2].target = Vault.key.target.Pickaxe;
        panels[3].target = Vault.key.target.Ship;
    }

    void SetPanelSelectionButtonsSprite()
    {
        buttons[0].image.sprite = objectReferencer.getCharacterSprite();
        buttons[1].image.sprite = objectReferencer.getWeaponSprite();
        buttons[2].image.sprite = objectReferencer.getToolSprite();
        buttons[3].image.sprite = objectReferencer.getShipSprite();
    }

    public void SetActivePanel(NodeManager nodeManager)
    {
        if (nodeManager == currentActivePanel) return;
        currentActivePanel.gameObject.SetActive(false);
        currentActivePanel = nodeManager;
        currentActivePanel.gameObject.SetActive(true);

        eventSystem.SetSelectedGameObject(currentActivePanel.firstSelectedButton);
    }

    public void SetActivePanel(int index)
    {
        SetActivePanel(panels[index]);
    }
}

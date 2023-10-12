using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Shapes;

public class PanelSelector : MonoBehaviour
{
    [Header("UI Elements")]
    public SkillButton button;
    public Polyline line;
    [SerializeField] List<NodeManager> panels;
    NodeManager currentActivePanel;
    [SerializeField] List<Button> buttons;

    [Header("Default display")]
    [SerializeField] string defaultCharacter = "Knil";
    [SerializeField] string defaultWeapon = "Gun";
    EventSystem eventSystem;
    InputMaster inputActions;
    public static PanelSelector instance;
    static int nbPanelsInitialized = 0;
    [SerializeField] Sprite shipSprite;


    // Start is called before the first frame update
    void Start()
    {
        nbPanelsInitialized = 0;
        instance = this;

        currentActivePanel = panels[0];

        if (DataSelector.selectedCharacter == string.Empty)   //Default buttons sprites if game launched from ship scene
        {
            DataSelector.selectedCharacter = defaultCharacter;
            DataSelector.selectedWeapon = defaultWeapon;
        }

        NodeManager.dictKeyToButton = new Dictionary<string, TreeButton>();

        foreach (NodeManager nodeManager in panels)
        {
            nodeManager.Initialize();
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

    //TODO: move to DataManager


    public static void PanelInitialized()
    {
        nbPanelsInitialized++;
        if (nbPanelsInitialized < instance.panels.Count) return;
        foreach (NodeManager nodeManager in instance.panels)
        {
            nodeManager.gameObject.SetActive(nodeManager == instance.currentActivePanel);
        }
    }

    void SetPanelsTarget()
    {
        panels[0].target = Vault.key.target.Pistolero;
        panels[1].target = Vault.key.target.Gun;
        panels[2].target = Vault.key.target.Ship;
    }

    public void SetPanelSelectionButtonsSprite()
    {
        buttons[0].image.sprite = DataSelector.getSelectedCharacter().getIcon();
        buttons[1].image.sprite = PlayerManager.weaponPrefab.spriteRenderer.sprite;
        buttons[2].image.sprite = shipSprite;
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

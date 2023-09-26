using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Shapes;

public class PanelSelector : MonoBehaviour
{
    [SerializeField] ObjectReferencer objectReferencer;
    [SerializeField] SpriteReferencer spriteReferencer;
    [Header("UI Elements")]
    public SkillButton button;
    public Polyline line;
    [SerializeField] List<NodeManager> panels;
    NodeManager currentActivePanel;
    [SerializeField] List<Button> buttons;

    [Header("Default display")]
    [SerializeField] character defaultCharacter = character.Pistolero;
    [SerializeField] weapon defaultWeapon = weapon.Gun;
    [SerializeField] tool defaultTool = tool.Pickaxe;
    EventSystem eventSystem;
    InputMaster inputActions;
    public static PanelSelector instance;
    public static Dictionary<string, ButtonSprite> dictKeyToButtonSprites;
    public static Dictionary<string, PowerHandler> dictKeyToPowerHandler;
    static int nbPanelsInitialized = 0;


    // Start is called before the first frame update
    void Start()
    {
        nbPanelsInitialized = 0;
        instance = this;

        LoadScriptableObjects();

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

    void LoadScriptableObjects() {
        dictKeyToButtonSprites = new Dictionary<string, ButtonSprite>();
        ButtonSprite[] buttonSprites = Resources.LoadAll<ButtonSprite>("ButtonSprites/");
        foreach (ButtonSprite buttonSprite in buttonSprites)
        {
            dictKeyToButtonSprites[buttonSprite.key] = buttonSprite;
        }

        dictKeyToPowerHandler = new Dictionary<string, PowerHandler>();
        PowerHandler[] powerHandlers = Resources.LoadAll<PowerHandler>("PowerHandlers/");
        foreach (PowerHandler powerHandler in powerHandlers)
        {
            dictKeyToPowerHandler[powerHandler.getKey()] = powerHandler;
        }
    }

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
        panels[2].target = Vault.key.target.Pickaxe;
        panels[3].target = Vault.key.target.Ship;
    }

    public void SetPanelSelectionButtonsSprite()
    {
        buttons[0].image.sprite = spriteReferencer.getCharacterSprite();
        buttons[1].image.sprite = PlayerManager.weaponPrefab.spriteRenderer.sprite;
        buttons[2].image.sprite = spriteReferencer.getToolSprite();
        buttons[3].image.sprite = spriteReferencer.getShipSprite();
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

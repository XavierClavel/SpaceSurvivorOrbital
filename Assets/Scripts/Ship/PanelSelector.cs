using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Shapes;
using DG.Tweening;

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


    public void Setup()
    {
        
        nbPanelsInitialized = 0;
        instance = this;

        currentActivePanel = panels[0];

        if (DataSelector.selectedCharacter == string.Empty) //Default buttons sprites if game launched from ship scene
        {
            DataSelector.selectedCharacter = defaultCharacter;
            DataSelector.selectedWeapon = defaultWeapon;
        }

        NodeManager.dictKeyToButton = new Dictionary<string, TreeButton>();

        SetupNodeManagers();
        

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

    public static void PanelInitialized()
    {
        nbPanelsInitialized++;
        if (nbPanelsInitialized < PlayerManager.powers.Count + 2) return;

        Debug.Log("here");
        foreach (NodeManager nodeManager in instance.panels)
        {
            nodeManager.gameObject.SetActive(nodeManager == instance.currentActivePanel);
        }
        UIManager.DisplayUpgradesUI();
    }

    void SetupNodeManagers()
    {
        List<string> keys = new List<string>
        {
            PlayerManager.character.getKey(),
            PlayerManager.weapon.getKey(),
        };
        keys.AddList(PlayerManager.powers.Select(it => it.getKey()).ToList());

        List<Sprite> icons = new List<Sprite>
        {
            DataSelector.getSelectedCharacter().getIcon(),
            PlayerManager.weaponPrefab.spriteRenderer.sprite,
        };
        icons.AddList(PlayerManager.powers.Select(it => it.getIcon()).ToList());
        
        for (int i = 0; i < panels.Count; i++)
        {
            if (i >= keys.Count)
            {
                buttons[i].gameObject.SetActive(false);
                panels[i].gameObject.SetActive(false);
            }
            else
            {
                buttons[i].image.sprite = icons[i];
                int value = i;
                buttons[i].onClick.AddListener(delegate { SetActivePanel(value); });
                panels[i].setup(keys[i]);
            }
                
        }
    }

    public void UpdateButtonSprites()
    {
        buttons[2].image.sprite = PlayerManager.weaponPrefab.spriteRenderer.sprite;
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

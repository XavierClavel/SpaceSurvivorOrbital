using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Shapes;
using DG.Tweening;
using UnityEngine.Serialization;

public interface UIPanel
{
    public RectTransform getUITransform();
    public void Setup() {}
    public void onPanelFocus() {}
    public void onPanelUnfocus() {}
    
}

public class UpgradesDisplayManager :  MonoBehaviour, UIPanel
{
    
#region variables

    //Input
    [Header("UI Elements")]
    public SkillButton button;
    public Polyline line;
    private List<NodeManager> panels = new List<NodeManager>();
    [SerializeField] List<UpgradePanelButton> buttons;
    [SerializeField] private NodeManager prefabPanelSkill;
    [SerializeField] private NodeManager prefabPanelUpgrade;

    [Header("Default display")]
    [SerializeField] string defaultCharacter = "Knil";
    [SerializeField] string defaultWeapon = "Gun";
    [SerializeField] Sprite shipSprite;
    
    //static API
    public static UpgradesDisplayManager instance;
    
    //private
    EventSystem eventSystem;
    InputMaster inputActions;
    private static int nbPanelsInitialized = 0;
    public NodeManager currentActivePanel;

    private int weaponIndex = 0;
    
    
#endregion
    
#region API

    public void onPanelUnfocus()
    {
        DeactivatePanelsNotDisplayed();
    }

    public RectTransform getUITransform()
    {
        return GetComponent<RectTransform>();
    }

    public void Setup()
    {
        nbPanelsInitialized = 0;
        instance = this;

        if (DataSelector.selectedWeapon == string.Empty) //Default buttons sprites if game launched from ship scene
        {
            //DataSelector.selectedCharacter = defaultCharacter;
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
    
#endregion

#region staticAPI

    public static void PanelInitialized()
    {
        nbPanelsInitialized++;
        //Debug.Log("Panel intialized");
        //Debug.Log($"expected {PlayerManager.powers.Count + PlayerManager.equipments.Count + 1} panels");
        if (nbPanelsInitialized < PlayerManager.powers.Count + PlayerManager.equipments.Count + 1) return;

        foreach (NodeManager nodeManager in instance.panels)
        {
            nodeManager.gameObject.SetActive(nodeManager == instance.currentActivePanel);
        }
        UIManager.DisplayUpgradesUI();
    }

#endregion

    

    void SetupNodeManagers()
    {
        List<string> keys = new List<string>
        {
            PlayerManager.weapon.getKey(),
        };
        keys.AddList(PlayerManager.equipments.Select(it => it.getKey()).ToList());
        keys.AddList(PlayerManager.powers.Select(it => it.getKey()).ToList());

        List<Sprite> icons = new List<Sprite>
        {
            DataSelector.getSelectedWeapon().getIcon(),
        };
        icons.AddList(PlayerManager.equipments.Select(it => it.getIcon()).ToList());
        icons.AddList(PlayerManager.powers.Select(it => it.getIcon()).ToList());

        for (int i = 0; i < 1 + PlayerManager.equipments.Count; i++)
        {
            InstantiatePanel(prefabPanelSkill, keys[i]);
        }
        
        for (int i = 1 + PlayerManager.equipments.Count; i < 1 + PlayerManager.equipments.Count + PlayerManager.powers.Count; i++)
        {
            InstantiatePanel(prefabPanelUpgrade, keys[i]);
        }
        
        currentActivePanel = panels[0];
        
        
        for (int i = 0; i < panels.Count; i++)
        {
            if (i >= keys.Count)
            {
                buttons[i].Disable();
                panels[i].gameObject.SetActive(false);
            }
            else
            {
                buttons[i].setSprite(icons[i]);
                int value = i;
                buttons[i].setAction(delegate { SetActivePanel(value); });
                panels[i].setup(keys[i]);
            }
        }

        for (int i = panels.Count; i < buttons.Count; i++)
        {
            buttons[i].Disable();
        }
    }

    private void InstantiatePanel(NodeManager panel, string key)
    {
        panel = Instantiate(panel, transform);
        panel.gameObject.name = $"Panel {key}";
        panels.Add(panel);
    }
    
    public void UpdateButtonSprites()
    {
        buttons[weaponIndex].setSprite(PlayerManager.weaponPrefab.spriteRenderer.sprite);
    }

    public void SetActivePanel(NodeManager nodeManager)
    {
        if (nodeManager == currentActivePanel) return;
        float sign = panels.IndexOf(nodeManager) > panels.IndexOf(currentActivePanel) ? -1f : 1f;
        nodeManager.panelRect.anchoredPosition = sign * UIManager.posAboveCamera * Vector2.up;
        
        nodeManager.panelRect.DOAnchorPosY(0, 1f).SetEase(Ease.InOutQuint);
        currentActivePanel.panelRect.DOAnchorPosY(sign * UIManager.posBelowCamera, 1f).SetEase(Ease.InOutQuint);
        
        //currentActivePanel.gameObject.SetActive(false);
        currentActivePanel = nodeManager;
        currentActivePanel.gameObject.SetActive(true);

        eventSystem.SetSelectedGameObject(currentActivePanel.firstSelectedButton);
    }

    public void DeactivatePanelsNotDisplayed()
    {
        panels.ForEach(it => it.gameObject.SetActive(it == currentActivePanel));
    }

    public void SetActivePanel(int index)
    {

        SoundManager.PlaySfx(transform, key: "Power_Switch");
        SetActivePanel(panels[index]);
    }
}

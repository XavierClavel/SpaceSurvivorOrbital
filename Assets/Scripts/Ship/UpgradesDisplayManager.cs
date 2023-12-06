using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Shapes;
using DG.Tweening;

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
    [SerializeField] List<NodeManager> panels;
    [SerializeField] List<Button> buttons;

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
    NodeManager currentActivePanel;
    
    
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
    
#endregion

#region staticAPI

    public static void PanelInitialized()
    {
        nbPanelsInitialized++;
        if (nbPanelsInitialized < PlayerManager.powers.Count + 2) return;

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
        buttons[1].image.sprite = PlayerManager.weaponPrefab.spriteRenderer.sprite;
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

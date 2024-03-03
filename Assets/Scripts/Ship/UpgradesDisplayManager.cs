using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Shapes;
using DG.Tweening;
using MyBox;
using UnityEngine.Serialization;
using TMPro;

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
    public Line line;
    private List<NodeManager> panels = new List<NodeManager>();
    [SerializeField] List<UpgradePanelButton> buttons;
    [SerializeField] private NodeManager prefabPanelSkill;
    [SerializeField] private NodeManager prefabPanelUpgrade;
    [SerializeField] private RbLbNavigator rbLbNavigator;
    [SerializeField] private RectTransform upgradesParent;
    [SerializeField] private AudioSource buyAudio;
    [SerializeField] [Range(0f, 1f)] private float buySfxVolume = 1f;
    [SerializeField] private float buyShakeIntensity = 0.5f;
    public static List<string> newPanels = new List<string>();

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

    public static void RemoveNewFlag(string key)
    {
        newPanels.TryRemove(key);
    }

    public static void Reset()
    {
        newPanels = new List<string>();
    }

    public static void addNewPanel(string key)
    {
        newPanels.Add(key);
    }

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
        
        inputActions = new InputMaster();
        inputActions.Enable();
        inputActions.UI.Validate.started += ctx => StartBuying();
        inputActions.UI.Validate.canceled += ctx => StopBuying();

        selectedButton = null;

        /*
        inputActions = new InputMaster();
        inputActions.UI.Navigate.started += ctx => StartCoroutine(nameof(OnNavigate));
        inputActions.UI.Navigate.canceled += ctx => StopCoroutine(nameof(OnNavigate));
        inputActions.UI.Navigate.performed += xtc => UpdateDisplay();
        inputActions.Enable();
        µ*/
    }

    private void OnDestroy()
    {
        inputActions.Disable();
    }

    public void StartBuying()
    {
        if (selectedButton == null) return;
        buyAudio.DOKill();
        buyAudio.volume = SaveManager.getOptions().sfxVolume * buySfxVolume;
        buyAudio.Play();
        //ShakeManager.StartShake(1f);
        StartCoroutine(nameof(holdCoroutine));
    }

    public void StopBuying()
    {
        buyAudio.DOFade(0, 0.15f);
        //ShakeManager.StopShake();
        StopCoroutine(nameof(holdCoroutine));
    }
    
    private IEnumerator holdCoroutine()
    {
        yield return Helpers.getWait(1.5f);
        UpgradeDisplay.Buy();
        StopBuying();
    }
    
#endregion

#region staticAPI

    private static TreeButton selectedButton;
    private static bool mouseDriven;

    public static void onSelect(TreeButton btn)
    {
        selectedButton = btn;
    }

    public static void onDeselect(TreeButton btn)
    {
        selectedButton = null;
        instance.StopBuying();
    }

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
        NodeManager.dictKeyToButton.ForEach(it => it.Value.UpdateStatus());
        UIManager.DisplayUpgradesUI();
        UpgradeDisplay.RefreshUpgradeDisplay();
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
                string key = keys[i];
                int value = i;
                buttons[i]
                    .setSprite(icons[i])
                    .setAction(delegate { SetActivePanel(value); })
                    .setTextKey(key + Vault.key.ButtonTitle);
                        
                panels[i].setup(key);
                if (newPanels.Contains(key))
                {
                    buttons[i]
                        .setAction(delegate { RemoveNewFlag(key); })
                        .flagNew();
                }
            }
        }

        for (int i = 0; i < keys.Count; i++)
        {
            Button prevButton = i == 0 ? null : buttons[i - 1].button;
            Button nextButton = i == keys.Count - 1 ? null : buttons[i + 1].button;
            rbLbNavigator.addRbLbObject(
                new RbLbObject(prevButton, nextButton)
                );
        }

        for (int i = panels.Count; i < buttons.Count; i++)
        {
            buttons[i].Disable();
        }
    }

    private void InstantiatePanel(NodeManager panel, string key)
    {
        panel = Instantiate(panel, upgradesParent);
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

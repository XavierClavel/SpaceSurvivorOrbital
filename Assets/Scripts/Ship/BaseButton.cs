using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MyBox;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public abstract class TreeButton : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler, IPointerExitHandler
{

    private List<string> buttonsToActivate = new List<string>();
    private List<string> buttonsToDeactivate = new List<string>();
    [HideInInspector] public string key;
    private List<Effect> effects = new List<Effect>();
    private bool mouseDriven = false;


    public skillButtonStatus status { get; private set; }
    [HideInInspector] public Button button;
    public RectTransform rectTransform;
    private Image image;
    protected delegate void buttonAction();
    protected UpgradeData upgradeData;
    ButtonSprite buttonSprite;
    private Node node;
    private Tween radialTween;

    [SerializeField] private Image radialFill;


    public virtual void Initialize(Node node)
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();

        this.key = node.key;
        this.node = node;

        upgradeData = DataManager.dictUpgrades[key];

        effects = upgradeData.effects.Copy();

        buttonsToActivate = upgradeData.upgradesEnabled;
        buttonsToDeactivate = upgradeData.upgradesDisabled;
        

        if (ScriptableObjectManager.dictKeyToButtonSprites.TryGetValue(upgradeData.spriteKey, out var sprite))
        {
            buttonSprite = sprite;
        }
        else Debug.LogWarning($"sprite key {upgradeData.spriteKey} is not associated with button sprites");
    }


    public void OnClick()
    {
        SoundManager.PlaySfx("Button_Switch");
    }


    protected void ApplyEffects()
    {
        foreach (Effect effect in effects)
        {
            effect.Apply();
        }
    }
    

    private void getLeafNodes(List<Node> nodes, Node node)
    {
        foreach (var childNode in node.childNodes)
        {
            nodes.TryAdd(childNode);
            getLeafNodes(nodes, childNode);
        }
    }

    private void getRootNodes(List<Node> nodes, List<Node> leafNodes)
    {
        foreach (var leafNode in leafNodes)
        {
            nodes.TryAdd(leafNode.parentNodes);
            getRootNodes(nodes, leafNode.parentNodes);
        }
    }

    private void getUpgradesToDeactivate()
    {
        Node currentNode = UpgradesDisplayManager.instance.currentActivePanel.dictKeyToNode[key];
        List<Node> nodes = new List<Node>();
        //getLeafNodes(nodes, currentNode);
        //getRootNodes(nodes, nodes);

        foreach (Node node in nodes)
        {
            if (node.tier <= currentNode.tier && NodeManager.dictKeyToButton[node.key].status != skillButtonStatus.bought)
            {
                buttonsToDeactivate.TryAdd(node.key);
            }
        }
    }

    protected void Execute(buttonAction action)
    {
        if (status != skillButtonStatus.unlocked) return;
        if (!DebugManager.areUpgradesFree() && !SpendResources()) return;
        ResourcesDisplay.UpdateResourcesDisplay();
        getUpgradesToDeactivate();
        
        NodeManager.UpdateButton(this, skillButtonStatus.bought);
        NodeManager.UpdateList(buttonsToActivate, skillButtonStatus.unlocked);
        NodeManager.UpdateList(buttonsToDeactivate, skillButtonStatus.discarded);

        action();
    }

    protected abstract bool SpendResources();

    public void UpdateStatus(skillButtonStatus status = skillButtonStatus.undefined)
    {
        if (node.parentNodes.Count > 0 && node.parentNodes.TrueForAll(it => NodeManager.dictKeyToStatus[it.key] == skillButtonStatus.discarded))
        {
            status = skillButtonStatus.discarded;
        }
        
        if (status != skillButtonStatus.undefined)
        {
            this.status = status;
            NodeManager.dictKeyToStatus[key] = this.status;
        }
        else
        {
            this.status = NodeManager.dictKeyToStatus[key];
        }
        
        Debug.Log($"{node.key} : {this.status}");
        
        //button.interactable = status == skillButtonStatus.unlocked;

        if (buttonSprite == null) return;
        switch (this.status)
        {
            case skillButtonStatus.bought:
                image.sprite = buttonSprite.purchased;
                SoundManager.PlaySfx(transform, key: "Button_Buy");
                HideCost();
                node.incomingPaths.ForEach(it =>
                {
                    if (NodeManager.dictKeyToStatus[it.Key.key] == skillButtonStatus.bought)
                    {
                        it.Value.ForEach(line =>
                        {
                            line.Dashed = false;
                            line.Color = new Color32(250, 176, 59, 255);
                        });
                    } else if (NodeManager.dictKeyToStatus[it.Key.key] == skillButtonStatus.discarded)
                    {
                        it.Value.ForEach(line =>
                        {
                            line.Dashed = true;
                            line.Color = new Color32(255,255,255,0);
                        });
                    }
                });
                break;

            case skillButtonStatus.unlocked:
                image.sprite = buttonSprite.available;
                node.incomingPaths.ForEach(it =>
                {
                    if (NodeManager.dictKeyToStatus[it.Key.key] == skillButtonStatus.bought)
                    {
                        it.Value.ForEach(line =>
                        {
                            line.Dashed = true;
                            line.Color = new Color32(250, 176, 59, 255);
                        });
                    } else if (NodeManager.dictKeyToStatus[it.Key.key] == skillButtonStatus.unlocked)
                    {
                        it.Value.ForEach(line =>
                        {
                            line.Dashed = true;
                            line.Color = new Color32(255, 255, 255, 120);
                        });
                    } else if (NodeManager.dictKeyToStatus[it.Key.key] == skillButtonStatus.discarded)
                    {
                        it.Value.ForEach(line =>
                        {
                            line.Dashed = true;
                            line.Color = new Color32(255,255,255,0);
                        });
                    }
                });
                break;

            case skillButtonStatus.locked:
                image.sprite = buttonSprite.locked;
                break;
            
            case skillButtonStatus.discarded:
                image.sprite = buttonSprite.locked;
                node.incomingPaths.ForEach(it =>
                {
                    it.Value.ForEach(line =>
                    {
                        line.Dashed = true;
                        line.Color = new Color32(255,255,255,0);
                    });
                });
                break;
        }

        node.childNodes.ForEach(it => NodeManager.dictKeyToButton[it.key].UpdateStatus());
    }

    protected abstract void HideCost();

    // When highlighted with mouse.
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseDriven = true;
        UpgradesDisplayManager.onSelect(this);
    }

    // When selected.
    public void OnSelect(BaseEventData eventData)
    {
        UpgradeDisplay.DisplayUpgrade(key, upgradeData.target);
        UpgradeDisplay.SetupBuyButton(delegate { Execute(ApplyEffects); }, gameObject);
        if (mouseDriven) return;
        UpgradesDisplayManager.onSelect(this);
    }

    //TODO : check input type
    
    public void OnDeselect(BaseEventData eventData)
    {
        if (mouseDriven) return;
        UpgradesDisplayManager.onDeselect(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UpgradesDisplayManager.onDeselect(this);
    }

    public void onStartBuying(float duration)
    {
        if (this.status != skillButtonStatus.unlocked) return;
        radialTween = DOTween.To(() => radialFill.fillAmount, x => radialFill.fillAmount = x, 1, duration - 0.2f).SetEase(Ease.Linear).SetDelay(0.2f);
    }

    public void onStopBuying()
    {
        radialTween?.Kill();
        radialFill.fillAmount = 0;
    }
    
}

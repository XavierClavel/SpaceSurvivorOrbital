using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Shapes;
using System.Collections;
using UnityEngine.EventSystems;

public class SelectButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private Button button;
    [HideInInspector] public RectTransform rectTransform;
    public Image background;
    [SerializeField] private GameObject cross;
    [HideInInspector] public string key;
    private SelectorLayout selectorLayout;
    [HideInInspector] public bool isUnlocked;
    [HideInInspector] public int cost = 0;
    [SerializeField] private GameObject costPanel;
    [SerializeField] private TextMeshProUGUI costDiplay;
    [SerializeField] private Image sprite;
    [SerializeField] private ShapeRenderer rectangleFill;
    [SerializeField] private ShapeRenderer discFill;
    [SerializeField] private Color colorFilled;
    [SerializeField] private Color colorUnfilled;
    [SerializeField] ParticleSystem unlockedPS;
    private bool isSelected = false;
    private InputMaster inputActions;


    private void Awake()
    {
        button = GetComponent<Button>();
        rectTransform = GetComponent<RectTransform>();
        
        inputActions = new InputMaster();
        inputActions.UI.Validate.started += ctx => startHolding();
        inputActions.UI.Validate.canceled += ctx => stopHolding();
    }

    private void OnDestroy()
    {
        inputActions.Disable();
    }

    public void Setup(string key, SelectorLayout selectorLayout)
    {
        this.key = key;
        this.selectorLayout = selectorLayout;
        gameObject.name = key;

        isUnlocked = SaveManager.isOptionUnlocked(key);

        if (DataManager.dictCost.TryGetValue(key, out var value))
        {
            cost = value;
            if (cost == 0) {isUnlocked = true;}
            else costDiplay.SetText(cost.ToString());
        }
        else
        {
            Debug.LogWarning($"Key {key} is missing from cost data csv");
            costDiplay.SetText("x");
        }
        
        
        cross.SetActive(!isUnlocked);
        costPanel.SetActive(!isUnlocked);
        
        sprite.sprite = DataSelector.getIcon(key);
        button.onClick.AddListener(Select);

        DataSelector.instance.dictKeyToButton[key] = this;
    }
    
    
    public void startHolding()
    {
        if (isUnlocked) return;
        StartCoroutine(nameof(holdCoroutine));
    }

    public void stopHolding()
    {
        StopCoroutine(nameof(holdCoroutine));
    }
    
    private IEnumerator holdCoroutine()
    {
        yield return Helpers.getWait(1.5f);
        Buy();
    }


    public void Select()
    {
        DataSelector.DisplayGeneric(key, this);
        if (!isUnlocked) return;
        DataSelector.SelectGeneric(key);
        selectorLayout.UpdateSelectedButton(this);
    }

    public void onSelect()
    {
        if (rectangleFill == null) return;
        rectangleFill.Color = colorFilled;
        discFill.Color = colorFilled;
    }

    public void onDeselect() {
        if (rectangleFill == null) return;
        rectangleFill.Color = colorUnfilled;
        discFill.Color = colorUnfilled;
        stopHolding();
    }


    public void Buy()
    {
        Debug.Log("paying");
        if (!DataSelector.Transaction(cost)) return;
        
        SaveManager.unlockOption(key);
        SoundManager.PlaySfx(transform, key: "Button_Buy");
        StartCoroutine(nameof(BuyCoroutine));

    }

    IEnumerator BuyCoroutine()
    {
        cross.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(0.5f);
        unlockedPS.Play();
        cross.SetActive(false);
        costPanel.SetActive(false);
        isUnlocked = true;

        Select();
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log($"OnSelect - {gameObject.name}");
        inputActions.Enable();
        DataSelector.DisplayGeneric(key, this);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        inputActions.Disable();
    }
}
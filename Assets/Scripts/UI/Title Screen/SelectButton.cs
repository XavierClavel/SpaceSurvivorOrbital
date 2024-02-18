using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Shapes;

public class SelectButton : MonoBehaviour
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


    private void Awake()
    {
        button = GetComponent<Button>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void Setup(string key, SelectorLayout selectorLayout)
    {
        this.key = key;
        this.selectorLayout = selectorLayout;

        isUnlocked = SaveManager.isOptionUnlocked(key);

        if (DataManager.dictCost.ContainsKey(key))
        {
            cost = DataManager.dictCost[key];
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


    public void Select()
    {
        DataSelector.DisplayGeneric(key, this);
        if (!isUnlocked && !TitleScreen.isSelectionFree) return;
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
    }


    public void Buy()
    {
        if (!DataSelector.Transaction(cost)) return;
        
        SaveManager.unlockOption(key);
        SoundManager.PlaySfx(transform, key: "Button_Buy");
        cross.SetActive(false);
        costPanel.SetActive(false);
        isUnlocked = true;
        
        Select();
    }

    

}
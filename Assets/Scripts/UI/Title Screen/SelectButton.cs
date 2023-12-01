using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        
        button.image.sprite = DataSelector.getIcon(key);
        button.onClick.AddListener(Select);
    }


    private void Select()
    {
        DataSelector.DisplayGeneric(key, this);
        if (!isUnlocked && !TitleScreen.isSelectionFree) return;
        DataSelector.SelectGeneric(key);
        selectorLayout.UpdateSelectedButton(this);
    }

    public void Buy()
    {
        if (!Transaction()) return;
        
        SaveManager.unlockOption(key);
        cross.SetActive(false);
        costPanel.SetActive(false);
        isUnlocked = true;
        
        Select();
    }

    bool Transaction()
    {
        PlayerManager.setSouls();
        if (PlayerManager.getSouls() < cost) return false;
        PlayerManager.spendSouls(cost);
        TitleScreen.UpdateSoulsDisplay();
        return true;
    }

}
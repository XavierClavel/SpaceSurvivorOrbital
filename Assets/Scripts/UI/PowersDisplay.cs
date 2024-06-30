using System.Linq;
using UnityEngine;
    
public class PowersDisplay : ItemGridDisplay<PowerHandler>, IPowerListener, ISelectableItemListener
{
    [SerializeField] private SelectableItem prefab;
    [SerializeField] private StringLocalizer titleDisplay;
    [SerializeField] private StringLocalizer descriptionDisplay;
    
    protected override string getKey(PowerHandler item)
    {
        return item.getKey();
    }

    protected override Sprite getSprite(PowerHandler item)
    {
        return item.getIcon();
    }

    private void Start()
    {
        addItem(PlayerManager.powers.ToList());
        EventManagers.powers.registerListener(this);
    }

    private void OnDestroy()
    {
        EventManagers.powers.unregisterListener(this);
    }

    public void onPowerPickup(PowerHandler power)
    {
        addItem(power);
    }
    
    protected override void AddItem(string key)
    {
        SelectableItem item = Instantiate(prefab);
        item.name = key;
        item.transform.setParent(transform);
        item.setup(key, getSprite(key));
        item.registerListener(this);
    }
    
    public void onSelect(SelectableItem item)
    {
        titleDisplay.setKey(item.getKey() + Vault.key.ButtonTitle);
        descriptionDisplay.setKey(item.getKey() + Vault.key.ButtonDescription);
    }

    public void onStartHighlight(SelectableItem item)
    {
        
    }
}

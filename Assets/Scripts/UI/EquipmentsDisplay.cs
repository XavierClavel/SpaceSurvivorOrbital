using System.Linq;
using UnityEngine;
    
public class EquipmentsDisplay : ItemGridDisplay<EquipmentHandler>, ISelectableItemListener
{
    [SerializeField] private SelectableItem prefab;
    [SerializeField] private StringLocalizer titleDisplay;
    [SerializeField] private StringLocalizer descriptionDisplay;
    
    protected override string getKey(EquipmentHandler item)
    {
        return item.getKey();
    }

    protected override Sprite getSprite(EquipmentHandler item)
    {
        return item.getIcon();
    }

    private void Start()
    {
        addItem(PlayerManager.equipments.ToList());
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

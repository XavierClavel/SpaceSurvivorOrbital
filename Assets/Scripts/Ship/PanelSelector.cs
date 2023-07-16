using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelSelector : MonoBehaviour
{
    [SerializeField] ObjectReferencer objectReferencer;
    [SerializeField] List<NodeManager> panels;
    NodeManager currentActivePanel;
    [SerializeField] List<Button> buttons;


    // Start is called before the first frame update
    void Start()
    {
        currentActivePanel = panels[0];
        foreach (NodeManager nodeManager in panels)
        {
            nodeManager.Initialize();
            nodeManager.gameObject.SetActive(nodeManager == currentActivePanel);
        }
        panels[0].target = Vault.key.target.Pistolero;
        panels[1].target = Vault.key.target.Gun;
        panels[2].target = Vault.key.target.Pickaxe;
        panels[3].target = Vault.key.target.Ship;

        if (DataSelector.selectedCharacter == character.None)   //Default buttons if game launched from ship scene
        {
            DataSelector.selectedCharacter = character.Pistolero;
            DataSelector.selectedWeapon = weapon.Gun;
            DataSelector.selectedTool = tool.Pickaxe;
        }

        buttons[0].image.sprite = objectReferencer.getCharacterSprite();
        buttons[1].image.sprite = objectReferencer.getWeaponSprite();
        buttons[2].image.sprite = objectReferencer.getToolSprite();
        buttons[3].image.sprite = objectReferencer.getShipSprite();

    }

    public void SetActivePanel(NodeManager nodeManager)
    {
        if (nodeManager == currentActivePanel) return;
        currentActivePanel.gameObject.SetActive(false);
        currentActivePanel = nodeManager;
        currentActivePanel.gameObject.SetActive(true);
    }

    public void SetActivePanel(int index)
    {
        SetActivePanel(panels[index]);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSelector : MonoBehaviour
{
    [SerializeField] List<NodeManager> panels;
    NodeManager currentActivePanel;


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

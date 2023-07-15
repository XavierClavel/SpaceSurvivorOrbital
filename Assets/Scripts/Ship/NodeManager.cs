using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    [SerializeField] Transform gridLayout;
    [SerializeField] GameObject emptyGameObject;
    [SerializeField] SkillButton button;

    // Start is called before the first frame update
    void Start()
    {
        CreateNodes();
        InitializeNodes();
    }

    void CreateNodes()
    {
        foreach (UpgradeData upgradeData in DataManager.dictUpgrades.Values)
        {
            new Node(upgradeData.name, upgradeData.upgradesEnabled);
        }
    }

    void InitializeNodes()
    {
        foreach (Node node in Node.dictKeyToNode.Values)
        {
            node.Initialize();
        }

        foreach (Node node in Node.dictKeyToNode.Values)
        {
            node.CheckForParentNodes();
        }

        List<Node> tier1Nodes = Node.firstTierNodes;
        List<Node> tier2Nodes = new List<Node>();
        List<Node> tier3Nodes = new List<Node>();
        List<Node> tier4Nodes = new List<Node>();

        foreach (Node node in tier1Nodes)
        {
            tier2Nodes = tier2Nodes.TryAdd(node.childNodes);
        }

        foreach (Node node in tier2Nodes)
        {
            tier3Nodes = tier3Nodes.TryAdd(node.childNodes);
        }

        foreach (Node node in tier3Nodes)
        {
            tier4Nodes = tier4Nodes.TryAdd(node.childNodes);
        }

        Debug.Log(tier1Nodes.Count);
        Debug.Log(tier2Nodes.Count);
        Debug.Log(tier3Nodes.Count);
        Debug.Log(tier4Nodes.Count);

        int leavesAmount = tier4Nodes.Count;

        string[,] nodeMatrix = new string[leavesAmount, 4];
        Debug.Log($"{nodeMatrix.GetLength(0)},{nodeMatrix.GetLength(1)}");

        for (int i = 0; i < leavesAmount; i++)
        {
            nodeMatrix[i, 3] = tier4Nodes[i].key;
        }


        nodeMatrix = FillTier(3, nodeMatrix);
        nodeMatrix = FillTier(2, nodeMatrix);
        nodeMatrix = FillTier(1, nodeMatrix);


        for (int y = 0; y < nodeMatrix.GetLength(0); y++)
        {
            for (int x = 0; x < nodeMatrix.GetLength(1); x++)
            {
                if (nodeMatrix[y, x] == null)
                {
                    GameObject go = Instantiate(emptyGameObject);
                    Helpers.SetParent(go, gridLayout);
                }
                else
                {
                    SkillButton newButton = Instantiate(button);
                    Helpers.SetParent(newButton.transform, gridLayout);
                    newButton.setText(nodeMatrix[y, x]);
                }
            }
        }


        foreach (Node node in tier1Nodes)
        {
            int leafAmount = node.getLeafNodes().Count;
        }
    }

    string[,] FillTier(int tier, string[,] nodeMatrix)
    {
        List<string> placedKeys = new List<string>();
        for (int i = 0; i < nodeMatrix.GetLength(0); i++)
        {
            if (nodeMatrix[i, tier] == null) continue;
            string key = nodeMatrix[i, tier];
            Debug.Log(key);
            Node node = Node.dictKeyToNode[key];
            if (node.parentNodes == null || node.parentNodes.Count == 0) continue;
            foreach (Node parentNode in node.parentNodes)
            {
                string parentKey = parentNode.key;
                if (placedKeys.Contains(parentKey)) continue;
                placedKeys.Add(parentKey);
                nodeMatrix[i, tier - 1] = parentKey;
                break;
            }

        }
        return nodeMatrix;
    }

}

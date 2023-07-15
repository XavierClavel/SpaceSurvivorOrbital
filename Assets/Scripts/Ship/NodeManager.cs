using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        CreateNodes();
        InitializeNodes();
    }

    void CreateNodes() { }

    void InitializeNodes()
    {
        foreach (Node node in Node.dictKeyToNode.Values)
        {
            node.Initialize();
        }

        List<Node> tier1Nodes = Node.firstTierNodes;
        List<Node> tier2Nodes = new List<Node>();
        List<Node> tier3Nodes = new List<Node>();
        List<Node> tier4Nodes = new List<Node>();

        foreach (Node node in tier1Nodes)
        {
            tier2Nodes.TryAdd(node.childNodes);
        }

        foreach (Node node in tier2Nodes)
        {
            tier3Nodes.TryAdd(node.childNodes);
        }

        foreach (Node node in tier3Nodes)
        {
            tier4Nodes.TryAdd(node.childNodes);
        }

        int leavesAmount = tier4Nodes.Count;

        string[,] nodeMatrix = new string[4, leavesAmount];
        for (int i = 0; i < leavesAmount; i++)
        {
            nodeMatrix[3, i] = tier4Nodes[i].key;
        }


        nodeMatrix = FillTier(3, nodeMatrix);
        nodeMatrix = FillTier(2, nodeMatrix);
        nodeMatrix = FillTier(1, nodeMatrix);


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
            if (nodeMatrix[tier, i] == "") continue;
            foreach (string parentKey in Node.dictKeyToNode[nodeMatrix[tier, i]].parentKeys)
            {
                if (placedKeys.Contains(parentKey)) continue;
                placedKeys.Add(parentKey);
                nodeMatrix[tier - 1, i] = parentKey;
                break;
            }

        }
        return nodeMatrix;
    }

}

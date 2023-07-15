using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{

    public string key;
    public List<string> parentKeys;
    public List<string> childKeys;

    public List<Node> parentNodes = new List<Node>();
    public List<Node> childNodes = new List<Node>();
    UpgradeData upgradeData;

    static List<Node> tier1Nodes = new List<Node>();
    static List<Node> tier2Nodes = new List<Node>();

    public static Dictionary<string, Node> dictKeyToNode = new Dictionary<string, Node>();

    public static List<Node> firstTierNodes = new List<Node>();


    public Node(string key, List<string> parentKeys, List<string> childKeys)
    {
        this.key = key;
        this.parentKeys = parentKeys;
        this.childKeys = childKeys;

        dictKeyToNode[key] = this;
    }

    public void Initialize()
    {
        foreach (string s in parentKeys)
        {
            parentNodes.Add(dictKeyToNode[s]);
        }

        foreach (string s in childKeys)
        {
            childNodes.Add(dictKeyToNode[s]);
        }

        if (parentNodes.Count == 0) firstTierNodes.Add(this);
    }

    public List<Node> getLeafNodes(Node node)
    {
        List<Node> leafNodes = new List<Node>();
        foreach (Node childNode in node.childNodes)
        {
            if (childNode.childNodes.Count == 0) leafNodes.TryAdd(node);
            else leafNodes.TryAdd(getLeafNodes(childNode));
        }
        return leafNodes;
    }

    public List<Node> getLeafNodes()
    {
        return getLeafNodes(this);
    }

    public List<Node> getRootNodes(Node node)
    {
        List<Node> rootNodes = new List<Node>();
        foreach (Node parentNode in node.parentNodes)
        {
            if (parentNode.parentNodes.Count == 0) rootNodes.TryAdd(node);
            else rootNodes.TryAdd(getRootNodes(parentNode));
        }
        return rootNodes;
    }

    public List<Node> getRootNodes()
    {
        return getRootNodes(this);
    }
}

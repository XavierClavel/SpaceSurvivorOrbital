using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{

    public string key;
    List<string> childKeys;
    public int tier;
    public int row;

    public List<Node> parentNodes = new List<Node>();
    public List<Node> childNodes = new List<Node>();

    public static Dictionary<string, Node> dictKeyToNode = new Dictionary<string, Node>();

    public static List<Node> firstTierNodes = new List<Node>();
    public Vector2Int position = new Vector2Int();


    public Node(string key, List<string> childKeys, int row)
    {
        this.key = key;
        this.childKeys = childKeys.Copy();
        this.row = row;

        dictKeyToNode[key] = this;
    }

    public void Initialize()
    {
        foreach (string s in childKeys)
        {
            childNodes.Add(dictKeyToNode[s]);
        }

        foreach (Node childNode in childNodes)
        {
            childNode.parentNodes.Add(this);
        }

    }

    public void CheckForParentNodes()
    {
        if (parentNodes.Count != 0) return;
        firstTierNodes.Add(this);
        tier = 1;
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

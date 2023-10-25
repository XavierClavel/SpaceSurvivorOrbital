using System;
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
    public Vector2Int position = new Vector2Int();
    NodeManager nodeManager;


    public Node(string key, List<string> childKeys, int row, NodeManager nodeManager)
    {
        this.key = key;
        this.childKeys = childKeys.Copy();
        this.row = row;
        this.nodeManager = nodeManager;

        nodeManager.dictKeyToNode[key] = this;
    }

    public Node(int x, int y)
    {
        key = String.Empty;
        childKeys = new List<string>();
        this.row = y;
        this.tier = x;
    }

    public void Initialize()
    {
        foreach (string s in childKeys)
        {
            childNodes.Add(nodeManager.dictKeyToNode[s]);
        }

        foreach (Node childNode in childNodes)
        {
            childNode.parentNodes.Add(this);
        }

    }

    public void CheckForParentNodes()
    {
        if (parentNodes.Count != 0) return;
        nodeManager.firstTierNodes.Add(this);
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

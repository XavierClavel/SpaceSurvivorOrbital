using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    [HideInInspector] public string target;
    [SerializeField] Transform gridLayout;
    [SerializeField] GameObject emptyGameObject;
    SkillButton button;
    public static Dictionary<string, skillButtonStatus> dictKeyToStatus = new Dictionary<string, skillButtonStatus>();
    Dictionary<int, List<Node>> dictTierToPlacedNodes = new Dictionary<int, List<Node>>();
    Dictionary<int, List<Node>> dictTierToNodes = new Dictionary<int, List<Node>>();
    public Dictionary<string, Node> dictKeyToNode = new Dictionary<string, Node>();
    public List<Node> firstTierNodes = new List<Node>();
    const int maxTier = 4;
    Node[,] nodeMatrix;
    int maxRow = -1;
    [HideInInspector] public GameObject firstSelectedButton;
    public static Dictionary<string, TreeButton> dictKeyToButton = new Dictionary<string, TreeButton>();

    public void LoadPlanetSelector()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(Vault.scene.PlanetSelector);
    }

    public static void Reset()
    {
        dictKeyToStatus = new Dictionary<string, skillButtonStatus>();
    }

    public void Initialize()
    {
        button = PanelSelector.instance.button;
        CreateNodes();
        InitializeNodes();
    }

    void CreateNodes()
    {
        foreach (UpgradeData upgradeData in DataManager.dictKeyToDictUpgrades[target].Values)
        {
            new Node(upgradeData.key, upgradeData.upgradesEnabled, upgradeData.row, this);
        }
    }

    void InitializeNodes()
    {
        foreach (Node node in dictKeyToNode.Values)
        {
            node.Initialize();
            if (node.row > maxRow) maxRow = node.row;
        }

        foreach (Node node in dictKeyToNode.Values)
        {
            node.CheckForParentNodes();
        }


        FillDictionary();

        FillNodeMatrix();


        CreateButtons();

    }

    void FillDictionary()
    {
        dictTierToNodes[1] = firstTierNodes;
        int tier = 1;
        while (true)
        {
            if (FillDictionaryTier(tier + 1) == 0) break;
            tier++;
        }
    }

    int FillDictionaryTier(int tier)
    {
        dictTierToNodes[tier] = new List<Node>();
        foreach (Node node in dictTierToNodes[tier - 1])
        {
            node.tier = tier - 1;
            dictTierToNodes[tier].TryAdd(node.childNodes);
        }
        if (dictTierToNodes[tier].Count == 0)
        {
            dictTierToNodes.Remove(tier);
            return 0;
        }
        return dictTierToNodes.Count;
    }

    void FillNodeMatrix()
    {
        //int leavesAmount = getLeavesAmount();

        nodeMatrix = new Node[maxRow, maxTier];

        /*
        for (int i = 0; i < leavesAmount; i++)
        {
            placeNode(i, 3, dictTierToNodes[maxTier][i]);
        }
        */
        /*
        for (int i = 1; i < maxTier; i++)
        {
            SetNodesTier(i);
        }
        */

        foreach (List<Node> nodeList in dictTierToNodes.Values)
        {
            foreach (Node node in nodeList)
            {
                PlaceNode(node);
            }
        }
    }

    void PlaceNode(Node node)
    {
        nodeMatrix[node.row - 1, node.tier - 1] = node;
    }


    void placeNode(int x, int y, Node node)
    {
        dictTierToPlacedNodes[y + 1].Add(node);
        node.position = new Vector2Int(x, y);
        nodeMatrix[x, y] = node;
    }

    int getLeavesAmount()
    {
        List<Node> leafNodes = new List<Node>();
        foreach (Node node in dictTierToNodes[1])
        {
            leafNodes.TryAdd(node.getLeafNodes());
        }
        return leafNodes.Count;
    }

    void CreateButtons()
    {
        for (int y = 0; y < nodeMatrix.GetLength(0); y++)
        {
            for (int x = 0; x < nodeMatrix.GetLength(1); x++)
            {
                Node node = nodeMatrix[y, x];
                if (node == null)
                {
                    GameObject go = Instantiate(emptyGameObject);
                    Helpers.SetParent(go, gridLayout);
                }
                else
                {
                    SkillButton button = SetupButton(node);

                    dictKeyToButton[node.key] = button;
                    if (x == 0 && firstSelectedButton == null) firstSelectedButton = button.gameObject;
                }
            }
        }
    }

    SkillButton SetupButton(Node node)
    {
        SkillButton newButton = Instantiate(button);
        Helpers.SetParent(newButton.transform, gridLayout);
        newButton.Initialize(node.key);
        newButton.UpdateStatus(getStatus(node));

        return newButton;
    }

    skillButtonStatus getStatus(Node node)
    {
        if (!dictKeyToStatus.ContainsKey(node.key))
        {
            if (node.parentNodes.Count == 0) dictKeyToStatus[node.key] = skillButtonStatus.unlocked;
            else dictKeyToStatus[node.key] = skillButtonStatus.locked;
        }

        return dictKeyToStatus[node.key];
    }

    void FillTier(int tier)
    {
        dictTierToPlacedNodes[tier] = new List<Node>();
        for (int i = 0; i < nodeMatrix.GetLength(0); i++)
        {
            if (nodeMatrix[i, tier] == null) continue;
            Node node = nodeMatrix[i, tier];
            if (node.parentNodes == null || node.parentNodes.Count == 0) continue;
            foreach (Node parentNode in node.parentNodes)
            {
                if (dictTierToPlacedNodes[tier].Contains(parentNode)) continue;
                //placeNode(i, tier - 1, parentNode);
                break;
            }

        }
    }

    void FillEarlyLeaves(int tier)
    {
        List<Node> difference = dictTierToPlacedNodes[tier].Difference(dictTierToNodes[tier]);
        if (difference.Count == 0) return;
        foreach (Node node in difference)
        {
            Node parentNode = node.parentNodes[0];
            if (nodeMatrix[parentNode.position.x + 1, parentNode.position.y + 1] == null)
            {
                //placeNode(parentNode.position.x + 1, parentNode.position.y + 1, n)
            }
        }
    }



    Dictionary<Vector2Int, Node> getRootTree(Node node, Vector2Int offset)
    {
        Dictionary<Vector2Int, Node> dictPosToNode = new Dictionary<Vector2Int, Node>();
        dictPosToNode.Add(offset, node);
        if (node.parentNodes == null || node.parentNodes.Count == 0) return dictPosToNode;
        int childAmount = node.childNodes.Count;
        int offsetY = childAmount > 2 ? -1 : 0;
        for (int i = 0; i < childAmount; i++)
        {
            Dictionary<Vector2Int, Node> newDict = getRootTree(node, offset + new Vector2Int(i + offsetY, 1));
            foreach (Vector2Int pos in newDict.Keys)
            {
                dictPosToNode.Add(pos, newDict[pos]);
            }
        }
        return dictPosToNode;
    }




    void SetNodesTier(int tier)
    {
        dictTierToPlacedNodes[tier] = new List<Node>();
        for (int i = 0; i < nodeMatrix.GetLength(0); i++)
        {
            if (nodeMatrix[i, tier] == null) continue;
            Node node = nodeMatrix[i, tier];
            if (node.childNodes == null || node.childNodes.Count == 0) continue;
            foreach (Node childNode in node.childNodes)
            {
                if (dictTierToPlacedNodes[tier].Contains(childNode)) continue;
                //placeNode(i, tier - 1, childNode);
                childNode.tier = tier + 1;
                break;
            }

        }
    }

    public static void UpdateButton(TreeButton skillButton, skillButtonStatus newStatus)
    {
        dictKeyToStatus[skillButton.key] = newStatus;
        skillButton.UpdateStatus(newStatus);
    }

    public static void UpdateList(List<string> keys, skillButtonStatus newStatus)
    {
        foreach (string key in keys)
        {
            UpdateButton(NodeManager.dictKeyToButton[key], newStatus);
        }
    }

}

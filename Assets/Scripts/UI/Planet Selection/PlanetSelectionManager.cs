using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using MyBox;
using Shapes;
using UnityEngine;
using UnityEngine.UI;

//all rows filled for first column
// then at least two
//a link between two nodes can only move vertically by one node
//each node must have at least one exit
//no crossing two paths
//the node that first generates paths is taken randomly
//last node is centered
        
//Algorithm :
//generates all nodes, at least two by columns
//for each row, generates path starting with a randomly selected node
public class PlanetSelectionManager : MonoBehaviour, UIPanel
{
    //Consts
    const int maxX = 10;
    const int maxY = 7;
    
    //Static
    private static Node[,] nodeMatrix;
    private static Dictionary<string, Planet> dictKeyToPlanet = new Dictionary<string, Planet>();
    public static Node currentNode = null;
    private static List<Node> accessibleNodes;
    
    //Static API
    public static Dictionary<string, PlanetData> dictKeyToPlanetData = new Dictionary<string, PlanetData>();
    
    //Exposed
    [SerializeField] private GameObject emptyGameObject;
    [SerializeField] private Planet planetObject;
    [SerializeField] private Transform gridLayoutTransform;
    [SerializeField] private GridLayoutGroup gridLayout;
    [SerializeField] private Line lineAccessible;
    [SerializeField] private Line lineInaccessible;
    [SerializeField] private RectTransform planetsPanel;
    
    //Public
    [HideInInspector] public GameObject firstSelectedButton;
    
    //Private
    private RectTransform panelRect;
    private List<Node> nodeList = new List<Node>();
    private RectTransform panelTransform;
    private List<Vector3> planetsPos = new List<Vector3>();
    
    //Consts
    private const float minDistanceBetweenPlanetsSqr = 100;
    private const int maxAttempts = 50;
    private Vector2 randomizePositionFactor = new Vector2(0f,00f);

    private static Node endNode = null;

    
#region staticAPI

    public static void Reset()
    {
        dictKeyToPlanet = new Dictionary<string, Planet>();
        dictKeyToPlanetData = new Dictionary<string, PlanetData>();
        currentNode = null;
    }

    public static PlanetData getStartPlanetData()
    {
        currentNode = null;
        Planet.currentTier = 1;
        return dictKeyToPlanetData["x0-y3"];
    }

    public static void SelectNode(Node node)
    {
        currentNode = node;
        Debug.Log($"Current node : {node.key}");
        Debug.Log($"Current node : {node.tier}");
        Debug.Log($"Current node : {node.position}");
    }

    public static Node GetSelectedNode()
    {
        return currentNode;
    }

    public static List<Node> getPossiblePathNodes()
    {
        if (currentNode == null)
        {
            var list = new List<Node>();
            for (int i = 0; i < maxY; i++)
            {
                Node node = nodeMatrix[0, i];
                if (node == null) continue;
                list.Add(node);
            }

            return list;
        }
            
        else return currentNode.childNodes;
    }

#endregion

#region API

    private void OnDestroy()
    {
        Planet.currentTier++;
    }

public RectTransform getUITransform()
    {
        return GetComponent<RectTransform>();
    }

    public static void GenerateData()
    {
        Reset();
        
        
        GenerateNodeMatrix();
        GeneratePaths();
        Cull();
        GeneratePlanetData();
        
        getAccessibleNodes();
        foreach (var node in nodeMatrix)
        {
            if (node == null) continue;
            if (!accessibleNodes.Contains(node)) nodeMatrix[node.tier, node.row] = null;
        }
    }

    private static void Cull()
    {
        int tier = maxX - 2;
        while (tier >= 0)
        {
            for (int y = 0; y < maxY; y++)
            {
                Node node = nodeMatrix[tier, y];
                if (node == null) continue;
                if (!node.childNodes.isEmpty()) continue;
                node.Destroy();
                nodeMatrix[tier, y] = null;
            }

            tier--;
        }
    }

    public void DisplayData()
    {
        PopulateGrid();
        getAccessibleNodes();
        CreateLinks();
    }

    public void MoveScreen()
    {
        if (currentNode == null) return;
        Debug.Log(gridLayout.spacing.x);
        Debug.Log(currentNode.position.x * gridLayout.spacing.x * Vector2.left);

        planetsPanel.anchoredPosition = currentNode.tier * gridLayout.spacing.x * Vector2.left;

        Debug.Log(planetsPanel.anchoredPosition);
        Debug.Log(planetsPanel.anchoredPosition3D);
    }

    private static void getAccessibleNodes()
    {
        if (currentNode != null)
        {
            accessibleNodes = new List<Node>
            {
                currentNode
            };
            getAccessibleNodes(currentNode, accessibleNodes);
        }
        else
        {
            accessibleNodes = new List<Node>();
            for (int i = 0; i < maxY; i++)
            {
                Node node = nodeMatrix[0, i];
                if (node == null) continue;
                accessibleNodes.Add(node);
                getAccessibleNodes(node, accessibleNodes);
            }
        }
        
    }

    private static void getAccessibleNodes(Node node, List<Node> nodes)
    {
        if (node.childNodes.IsNullOrEmpty()) return;
        foreach (var childNode in node.childNodes)
        {
            nodes.TryAdd(childNode);
            getAccessibleNodes(childNode, nodes);
        }
    }


    public void Setup()
    {
        panelRect = GetComponent<RectTransform>();

        DisplayData();
    }

#endregion

    private static void GenerateNodeMatrix()
    {
        nodeMatrix = new Node[maxX, maxY];
        int middleYIndex = (int)(0.5 * maxY);

        for (int i = 0; i < maxX - 1; i++)
        {
            GenerateNodeColumn(i);
        }


        nodeMatrix[maxX - 1, middleYIndex] = new Node(maxX - 1, middleYIndex);
        endNode = nodeMatrix[maxX - 1, middleYIndex];
    }

    private static void GenerateNodeColumn(int x)
    {
        List<int> columnIndexes = Enumerable.Range(0, maxY).ToList();
        columnIndexes.Add(-1);
        for (int i = 0; i < maxY; i++)
        {
            int y = columnIndexes.popRandom();
            if (y == -1) continue;
            nodeMatrix[x, y] = new Node(x, y);
        }
    }

    private static void GeneratePaths()
    {
        for (int tier = 0; tier < maxX - 1; tier++)
        {
            for (int y = 0; y < maxY; y++)
            {
                Node node = nodeMatrix[tier, y];
                if (node == null) continue;

                List<Node> options = getPathOptions(tier, y);
                options = selectPaths(options);
                foreach (Node neighborNode in options)
                {
                    node.childNodes.Add(neighborNode);
                    neighborNode.parentNodes.Add(node);
                }
            }
        }
    }

    private static List<Node> getPathOptions(int x, int y)
    {
        //Debug.Log($"=======================================");
        //Debug.Log($"Node : {x}-{y}");
        List<Node> pathOptions = new List<Node>();
        for (int potentialRow = y - 1; potentialRow <= y + 1; potentialRow++)
        {
            if (potentialRow < 0 || potentialRow >= maxY) continue;
            Node neighborNode = nodeMatrix[x+1, potentialRow];
            //Debug.Log($"Neighbor node : {x+1}-{potentialRow}");
            if (neighborNode != null) pathOptions.Add(neighborNode);
        }

        return pathOptions;
    }

    private static List<Node> selectPaths(List<Node> options)
    {
        if (options.Count == 0) return options;
        if (options[0].tier is maxX) return options; 
        switch (options.Count)
        {
            case 1 : return options;
            case 2 :
                if (Helpers.ProbabilisticBool(0.3f)) options.popRandom();
                return options;
            case 3 :
                options.popRandom();
                return options;
            default :
                return options;
        }
        
    }

    private static void GeneratePlanetData()
    {
        for (int y = 0; y < maxY; y++)
        {
            for (int x = 0; x < maxX; x++)
            {
                Node node = nodeMatrix[x, y];
                if (node == null) continue;
                
                node.key = $"x{x}-y{y}";
                if (x == maxX - 1)
                {
                    dictKeyToPlanetData[node.key] = PlanetData.Boss();
                }
                else
                {
                    dictKeyToPlanetData[node.key] = PlanetData.getRandom(x);
                }
            }
        }
    }
    
    void PopulateGrid()
    {
        for (int y = 0; y < maxY; y++)
        {
            for (int x = 0; x < maxX; x++)
            {
                Node node = nodeMatrix[x, y];
                if (node == null)
                {
                    GameObject go = Instantiate(emptyGameObject);
                    Helpers.SetParent(go.transform, gridLayoutTransform, scale: 0.3f);
                }
                else
                {
                    node.key = $"x{x}-y{y}";
                    Planet planet = SetupPlanet(node);
                    if (y == 0 && firstSelectedButton == null) firstSelectedButton = planet.gameObject;
                }
            }
        }
    }

    void CullGrid()
    {
        int culled = 0;
        for (int y = 0; y < maxY; y++)
        {
            for (int x = 0; x < maxX; x++)
            {
                Node node = nodeMatrix[x, y];
                if (node == null)
                {
                    continue;
                }   
                else
                {
                    if (node.childNodes.Count == 0)// || node.parentNodes.Count == 0)
                    {
                        Debug.Log("culled");
                        nodeMatrix[x, y] = null;
                        culled++;
                    }
                }
            }
        }
        if (culled != 0) { CullGrid(); }
    }

    Vector3 getPlanetPosition(Vector3 position, string me)
    {
        return position;
        Vector3? newPos = null;
        int currentAttempt = 0;

        while (newPos == null && currentAttempt < maxAttempts)
        {
            newPos = tryPosition(position);
            currentAttempt++;
        }
        
        if (newPos == null)
        {
            planetsPos.Add(position);
            return position;
        }
        
        //Debug.Log($"Current Attempt : {currentAttempt}, closestNeighbor : {closestNeighbor((Vector3)newPos, me)}");
        //Debug.Log($"===============================================");
        
        planetsPos.Add((Vector3)newPos);
        return (Vector3)newPos;

    }
    
    float closestNeighbor(Vector3 newPos, string me)
    {
        float closestNeighbor = float.MaxValue;
        int neighborIndex = 0;
        for (var index = 0; index < planetsPos.Count; index++)
        {
            var planetPos = planetsPos[index];
            if (Vector3.SqrMagnitude((newPos - planetPos) * 1920f) < closestNeighbor)
            {
                closestNeighbor = Vector3.SqrMagnitude((newPos - planetPos) * 1920f);
                neighborIndex = index;
            }
        }
        
        //Debug.Log($"Closest neighborship : {nodeList[neighborIndex].key}, {me}");

        return closestNeighbor;
    }

    Vector3? tryPosition(Vector3 position)
    {
        Vector3 newPos = position;// + Helpers.getRandomPositionInRadius(randomizePositionFactor);
        foreach (var planetPos in planetsPos)
        {
            
            if (Vector3.SqrMagnitude((newPos - planetPos) * 1920f) < minDistanceBetweenPlanetsSqr)
            {
                return null;
            }
            
        }

        return  newPos * 1f/1920f;
    }

    Planet SetupPlanet(Node node)
    {
        nodeList.Add(node);
        Planet newPlanet = Instantiate(planetObject);
        
        
        dictKeyToPlanet[node.key] = newPlanet;
        newPlanet.setup(node, getPlanetPosition(newPlanet.planet.transform.position, node.key));
        Helpers.SetParent(newPlanet.transform, gridLayoutTransform, -2, getScale(newPlanet));
        newPlanet.name = node.key;

        return newPlanet;
    }

    float getScale(Planet planet)
    {
        return gridLayout.cellSize.y * planet.planetData.size switch
        {
            planetSize.small => 0.45f,
            planetSize.medium => 0.75f,
            planetSize.large => 1.1f,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    void CreateLinks()
    {
        StartCoroutine(nameof(CreateLinksCoroutine));
    }

    private IEnumerator CreateLinksCoroutine()
    {
        yield return null;
        foreach (Node parentNode in nodeList)
        {
            Line line = accessibleNodes.Contains(parentNode) ? lineAccessible : lineInaccessible;
            foreach (Node childNode in parentNode.childNodes)
            {
                Line polyline = Instantiate(line, transform, true);
                polyline.transform.SetParent(planetsPanel.transform);
                polyline.transform.localScale = Vector3.one;
                polyline.transform.localPosition = Vector3.zero;
                polyline.name = $"Line_{parentNode.key}_to_{childNode.key}";
                
                polyline.Start = panelRect.InverseTransformPoint(dictKeyToPlanet[parentNode.key].planet.GetComponent<RectTransform>().position);
                polyline.End = panelRect.InverseTransformPoint(dictKeyToPlanet[childNode.key].planet.GetComponent<RectTransform>().position);

                //polyline.GetComponent<RectTransform>().anchoredPosition3D = 10 * Vector3.back;
            }
        }
        MoveScreen();
    }
    
}

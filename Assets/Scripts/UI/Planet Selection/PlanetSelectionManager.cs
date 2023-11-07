using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Shapes;
using UnityEngine;

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
public class PlanetSelectionManager : MonoBehaviour
{
    //Consts
    const int maxX = 10;
    const int maxY = 3;
    
    //Static
    private static Node[,] nodeMatrix;
    private static Dictionary<string, Planet> dictKeyToPlanet = new Dictionary<string, Planet>();
    private static Node currentNode = null;
    
    //Static API
    public static Dictionary<string, PlanetData> dictKeyToPlanetData = new Dictionary<string, PlanetData>();
    
    //Exposed
    [SerializeField] private GameObject emptyGameObject;
    [SerializeField] private Planet planetObject;
    [SerializeField] private Transform gridLayout;
    [SerializeField] private Polyline line;
    
    //Public
    [HideInInspector] public GameObject firstSelectedButton;
    
    //Private
    private RectTransform panelRect;
    private List<Node> nodeList = new List<Node>();

    
#region staticAPI

    public static void Reset()
    {
        dictKeyToPlanet = new Dictionary<string, Planet>();
        dictKeyToPlanetData = new Dictionary<string, PlanetData>();
        nodeMatrix = new Node[maxX, maxY];
        currentNode = null;
    }

    public static void SelectNode(Node node)
    {
        currentNode = node;
    }

    public static Node GetSelectedNode()
    {
        return currentNode;
    }

    public static List<Node> getPossiblePathNodes()
    {
        if (currentNode == null)
            return new List<Node>()
            {
                nodeMatrix[0, 1],
            };
        else return currentNode.childNodes;
    }

#endregion

#region API

    public void Setup()
    {
        panelRect = GetComponent<RectTransform>();
        
        if (dictKeyToPlanet.Count == 0) 
        {
            GenerateNodeMatrix();
        }
        
        GeneratePaths();   
        
        PopulateGrid();
        
        CreateLinks();
    }

#endregion

    void GenerateNodeMatrix()
    {
        nodeMatrix = new Node[maxX, maxY];
        int middleYIndex = (int)(0.5 * maxY);
        nodeMatrix[0, middleYIndex] = new Node(0, middleYIndex);
        //currentNode = nodeMatrix[0, middleYIndex];

        for (int i = 1; i < maxX - 1; i++)
        {
            GenerateNodeColumn(i);
        }


        nodeMatrix[maxX - 1, middleYIndex] = new Node(maxX - 1, middleYIndex);
    }

    void GenerateNodeColumn(int x)
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

    void GeneratePaths()
    {
        for (int tier = 0; tier < maxX - 1; tier++)
        {
            for (int y = 0; y < maxY; y++)
            {
                Debug.Log($"Loop index {tier}-{y}");
                if (nodeMatrix[tier, y] == null) continue;

                List<Node> options = getPathOptions(tier, y);
                options = selectPaths(options);
                foreach (Node neighborNode in options)
                {
                    nodeMatrix[tier, y].childNodes.Add(neighborNode);
                }
            }
        }
    }

    List<Node> getPathOptions(int x, int y)
    {
        Debug.Log($"=======================================");
        Debug.Log($"Node : {x}-{y}");
        List<Node> pathOptions = new List<Node>();
        for (int potentialRow = y - 1; potentialRow <= y + 1; potentialRow++)
        {
            if (potentialRow < 0 || potentialRow >= maxY) continue;
            Node neighborNode = nodeMatrix[x+1, potentialRow];
            Debug.Log($"Neighbor node : {x+1}-{potentialRow}");
            if (neighborNode != null) pathOptions.Add(neighborNode);
        }

        return pathOptions;
    }

    List<Node> selectPaths(List<Node> options)
    {
        if (options[0].tier == 1 || options[0].tier == maxX) return options; 
        switch (options.Count)
        {
            case 1 : return options;
            case 2 :
                //if (Helpers.ProbabilisticBool(0.5f)) options.popRandom();
                return options;
            case 3 :
                //options.popRandom();
                return options;
            default :
                return options;
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
                    Helpers.SetParent(go.transform, gridLayout, scale: 0.3f);
                }
                else
                {
                    node.key = $"{x}-{y}";
                    Planet planet = SetupPlanet(node);
                    if (y == 0 && firstSelectedButton == null) firstSelectedButton = planet.gameObject;
                }
            }
        }
    }
    
    Planet SetupPlanet(Node node)
    {
        nodeList.Add(node);
        Planet newPlanet = Instantiate(planetObject);
        newPlanet.setup(node);
        newPlanet.name = node.key;
        
        Helpers.SetParent(newPlanet.transform, gridLayout, -2, getScale(newPlanet));
        dictKeyToPlanet[node.key] = newPlanet;

        return newPlanet;
    }

    float getScale(Planet planet)
    {
        return planet.planetData.size switch
        {
            planetSize.small => 0.22f,
            planetSize.medium => 0.3f,
            planetSize.large => 0.38f,
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
            foreach (Node childNode in parentNode.childNodes)
            {
                Polyline polyline = Instantiate(line);
                polyline.transform.SetParent(transform);
                polyline.transform.localScale = Vector3.one;
                polyline.transform.position = Vector3.zero;
                polyline.name = $"Line_{parentNode.key}_to_{childNode.key}";
                
                Vector2 startPoint = panelRect.InverseTransformPoint(dictKeyToPlanet[parentNode.key].planet.GetComponent<RectTransform>().position);
                Vector2 endPoint = panelRect.InverseTransformPoint(dictKeyToPlanet[childNode.key].planet.GetComponent<RectTransform>().position);

                List<Vector2> points = new List<Vector2>()
                {
                    startPoint,
                    endPoint,
                };
                
                polyline.SetPoints(points);

                polyline.GetComponent<RectTransform>().anchoredPosition3D = 10 * Vector3.back;
            }
        }
    }
    
}

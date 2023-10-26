using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        for (int i = 0; i < maxY; i++)
        {
            nodeMatrix[0, i] = new Node(0,i);
        }

        for (int i = 1; i < maxX; i++)
        {
            GenerateNodeColumn(i);
        }
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
        
        Helpers.SetParent(newPlanet.transform, gridLayout, -2, 0.3f);
        dictKeyToPlanet[node.key] = newPlanet;

        return newPlanet;
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
                
                Vector2 startPoint = panelRect.InverseTransformPoint(dictKeyToPlanet[parentNode.key].GetComponent<RectTransform>().position);
                Vector2 endPoint = panelRect.InverseTransformPoint(dictKeyToPlanet[childNode.key].GetComponent<RectTransform>().position);

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

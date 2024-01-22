using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TileManager : MonoBehaviour
{
    public PlanetData planetData;
    public SpriteRenderer groundSprite;
    [SerializeField] TilesBankManager bank;
    TileWaveFunction[,] map;
    [SerializeField] DistanceConstraintsManager distanceConstraintsManager;
    public Vector2Int tileSize = new Vector2Int(10, 10);
    [HideInInspector] public List<Tile> tiles;
    List<TileWaveFunction> uncollapsedTiles = new List<TileWaveFunction>();
    Dictionary<Vector2Int, GameObject> dictPositionToTile = new Dictionary<Vector2Int, GameObject>();
    PlayerController player;
    public static int planetDiameter = 9;
    Vector2Int mapSize;
    Vector2Int activationRadius = new Vector2Int(3, 3); //radius around player in which tiles are activated
    Vector2Int lastPos = Vector2Int.zero;
    Vector2Int mapRadius;
    List<Tile> tilesToPlace = new List<Tile>();
    private int mask;

    [HideInInspector] public static int tilesInAdvance => instance.uncollapsedTiles.Count - instance.tilesToPlace.Count;
    [HideInInspector] public static int tilesToPlaceAmount => instance.tilesToPlace.Count;

    float noiseFactor = 0.3f;   //chance to collapse a random tile
    public static TileManager instance;
    const float tileRotationPeriod = 0.05f;
    //TODO : adapat tilerotationperiod to planet size

    public bool generateMap;

    public static Tile getTileToPlace(List<Tile> possibleStates)
    {
        List<Tile> intersection = instance.tilesToPlace.Intersection(possibleStates);
        if (intersection.Count == 0) return null;
        return intersection.getRandom();
    }


    // Start is called before the first frame update
    void Awake()
    {
        mask = LayerMask.GetMask(Vault.layer.Ennemies);
        instance = this;
        if (!PlanetManager.hasData()) PlanetManager.setData(planetData);
        if (!generateMap) return;
        
        bank.setTiles();
        tiles.Add(bank.spaceship);
        bank.altar1.setSpecificAmount(PlanetManager.getAltarAmount());
        tiles.Add(bank.altar1);
        tiles.Add(bank.empty1);
        tiles.Add(bank.empty2);
        tiles.Add(bank.empty3);
        SetupPlanet();
        mapRadius = (mapSize - Vector2Int.one) / 2;
        player = PlayerController.instance;



        foreach (Tile tile in tiles)
        {
            tile.Reset();
            tilesToPlace.AddMultiple(tile, tile.minAmount);
            //Debug.Log(tile.name + " " + tile.minAmount);
        }

        if (tilesToPlace.Count > mapSize.x * mapSize.y)
        {
            Debug.LogError("Too many to place to place, constraints cannot be respected");
            Debug.Log($"tiles to place : {tilesToPlace.Count}");
            Debug.Log($"total tiles : {mapSize.x * mapSize.y}" );
        }

        foreach (DistanceConstraint distanceConstraint in distanceConstraintsManager.distanceConstraints)
        {
            distanceConstraint.Apply();
        }

        foreach (DistanceConstraintGroup distanceConstraintGroup in distanceConstraintsManager.distanceConstraintGroups)
        {
            distanceConstraintGroup.Apply();
        }

        foreach (SelfDistanceConstraint selfDistanceConstraint in distanceConstraintsManager.selfDistanceConstraints)
        {
            selfDistanceConstraint.Apply();
        }

        InitalizeMap();
        PlaceTiles();

        StartCoroutine(nameof(TileManagement));
    }

    void SetupPlanet()
    {
        planetDiameter = PlanetManager.getSize();
        AllocateDens();
        AllocateResource(PlanetManager.getRessourceAmount(), bank.ressource1, bank.ressource2, bank.ressource3);
        
        mapSize = new Vector2Int(planetDiameter, planetDiameter);

        if (mapSize.x % 2 == 0 || mapSize.y % 2 == 0) throw new System.ArgumentOutOfRangeException("map size must be odd");
    }

    void AllocateResource(int resourceAmount, Tile size1, Tile size2, Tile size3)
    {
        int amountBlock1 = resourceAmount;
        int amountBlock2 = 0;
        int amountBlock3 = 0;
        if (resourceAmount != 0)
        {
            amountBlock3 = Random.Range(0, resourceAmount / 3);
            amountBlock1 -= amountBlock3 * 3;

            amountBlock2 = Random.Range(0, amountBlock1 / 2);
            amountBlock1 -= amountBlock2 * 2;
        }
        size3.setSpecificAmount(amountBlock3);
        size2.setSpecificAmount(amountBlock2);
        size1.setSpecificAmount(amountBlock1);

        if (size1.maxAmount != 0) tiles.TryAdd(size1);
        if (size2.maxAmount != 0) tiles.TryAdd(size2);
        if (size3.maxAmount != 0) tiles.TryAdd(size3);
    }

    void AllocateDens()
    {
        bank.den1.setSpecificAmount(PlanetManager.getDensAmount());
        tiles.Add(bank.den1);
    }
    
    void AllocateResource(int resourceAmount, Tile tile)
    {
        tile.setSpecificAmount(resourceAmount);
        if (tile.maxAmount != 0) tiles.TryAdd(tile);
    }

    void InitalizeMap()
    {
        map = new TileWaveFunction[mapSize.x, mapSize.y];
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                map[x, y] = new TileWaveFunction(tiles, new Vector2Int(x, y));
                uncollapsedTiles.Add(map[x, y]);
            }
        }
        map[mapRadius.x, mapRadius.y].possibleStates = new List<Tile> {bank.spaceship};
        map[mapRadius.x, mapRadius.y].entropy = int.MinValue;

        CollapseWaveFunction(map[mapRadius.x, mapRadius.y]);
    }


    void ApplyConstraint(List<Vector2Int> positions, Tile removeTile)
    {
        foreach (Vector2Int position in positions)
        {
            TileWaveFunction tileWaveFunction = map.overflow(position);
            if (tileWaveFunction == null) continue;
            tileWaveFunction.ReduceWaveFunction(removeTile);
        }
    }

    void ApplyConstraint(List<TileWaveFunction> tileWaveFunctions, Tile removeTile)
    {
        foreach (TileWaveFunction tileWaveFunction in tileWaveFunctions)
        {
            if (tileWaveFunction == null) continue;
            tileWaveFunction.ReduceWaveFunction(removeTile);
        }
    }

    void PlaceTiles()
    {
        while (true)
        {
            TileWaveFunction newTileWaveFunction = GetUncollapsedTileOfLeastEntropy();
            if (newTileWaveFunction == null)    //no TileWaveFunction left to collapse
            {
                break;
            }
            CollapseWaveFunction(newTileWaveFunction);
        }

    }

    ///<summary>
    ///Returns the Tile with the least amount of possible states.
    ///</summary>
    TileWaveFunction GetUncollapsedTileOfLeastEntropy()
    {
        if (uncollapsedTiles.Count == 0) return null;
        List<TileWaveFunction> uncollapsedTilesOfLeastEntropy = new List<TileWaveFunction>();
        int minEntropy = int.MaxValue;

        if (Helpers.ProbabilisticBool(noiseFactor))
        {
            return uncollapsedTiles.getRandom();
            //chance to collapse a random tile to add diversity 
            //and avoid concentrating objects with limited amount around the center of the map

        }

        foreach (TileWaveFunction tileWaveFunction in uncollapsedTiles)
        {
            if (tileWaveFunction.entropy < minEntropy)
            {
                uncollapsedTilesOfLeastEntropy = new List<TileWaveFunction>();
                uncollapsedTilesOfLeastEntropy.Add(tileWaveFunction);
                minEntropy = tileWaveFunction.entropy;
            }
            else if (tileWaveFunction.entropy == minEntropy)
            {
                uncollapsedTilesOfLeastEntropy.Add(tileWaveFunction);
                //list used to randomize the output in case of multiple tiles with same entropy
            }
        }

        return uncollapsedTilesOfLeastEntropy.getRandom();
    }

    GameObject CollapseWaveFunction(TileWaveFunction tileWaveFunction)
    {
        uncollapsedTiles.Remove(tileWaveFunction);

        Tile newTile = tileWaveFunction.CollapseWaveFunction();
        tilesToPlace.TryRemove(newTile);
        Vector2Int index = tileWaveFunction.index;


        List<Vector2Int> tilesToCollapse;
        foreach (TileConstraint constraint in newTile.constraints)
        {
            tilesToCollapse = index.GetPosInRange(constraint.distance);
            //Debug.Log(newTile.name + " and " + constraint.otherTile.name + " : " + constraint.distance);
            ApplyConstraint(tilesToCollapse, constraint.otherTile);
        }

        if (newTile.hasLimitedAmount)
        {
            newTile.currentAmount++;
            if (newTile.currentAmount >= newTile.maxAmount)
            {
                ApplyConstraint(uncollapsedTiles, newTile);
            }
        }

        Vector2Int position = IndexToPosition(index);
        Vector3 worldPosition = PositionToWorld(position);
        GameObject tile = Instantiate(newTile.getTileObject(), worldPosition, Quaternion.identity);
        dictPositionToTile.Add(position, tile);
        map[index.x, index.y] = null;

        SpriteRenderer ground = Instantiate(bank.ground);
        ground.transform.position = worldPosition;
        ground.transform.SetParent(tile.transform);
        int x = Mathf.Abs(position.x);
        int y = Mathf.Abs(position.y);
        if (x >= 4 || y >= 4) {
            ground.color = bank.groundColor3;
            ground.sortingOrder = -13;
        } else if (x >= 2 || y >= 2) {
            ground.color = bank.groundColor2;
            ground.sortingOrder = -12;
        } else
        {
            ground.color = bank.groundColor1;
            ground.sortingOrder = -11;
        }
        return tile;
    }

    Vector2Int IndexToPosition(Vector2Int index)
    {
        return index - mapRadius;
    }


    Vector3 PositionToWorld(Vector2Int position)
    {
        return new Vector3(position.x * tileSize.x, position.y * tileSize.y);
    }

    IEnumerator TileManagement()
    {
        while (true)
        {
            yield return Helpers.getWait(tileRotationPeriod);
            UpdateActiveTiles();
        }
    }

    public void SetMap(List<TileRow> tileRows, Vector2Int tileSize)
    {
        this.tileSize = tileSize;
        mapSize = new Vector2Int(tileRows.Count, tileRows.Count);
        mapRadius = (mapSize - Vector2Int.one) / 2;
        player = PlayerController.instance;

        Vector2Int index = Vector2Int.zero;
        foreach (TileRow tileRow in tileRows)
        {
            foreach (GameObject tile in tileRow.tileRow)
            {
                Vector2Int position = IndexToPosition(index);
                dictPositionToTile.Add(position, tile);
                index.x++;
            }
            index.y++;
            index.x = 0;
        }

        StartCoroutine(nameof(TileManagement));

    }

    void UpdateActiveTiles()
    {
        Vector2Int currentPos = Helpers.RoundToVector2IndexStep(player.transform.position, tileSize);
        Vector2Int offset = currentPos - lastPos;

        bool goingRight = offset.x >= 0;
        bool goingUp = offset.y >= 0;

        if (offset == Vector2Int.zero) return;

        List<Vector2Int> tilesToWrapAroundMap = new List<Vector2Int>();




        int signx = Helpers.IntSign(offset.x);
        int signy = Helpers.IntSign(offset.y);
        for (int x = lastPos.x - mapRadius.x; x <= lastPos.x + mapRadius.x; x++)
        {
            for (int y = lastPos.y - mapRadius.y; y <= lastPos.y + mapRadius.y; y++)
            {
                if (signx * x < signx * currentPos.x - mapRadius.x || signy * y < signy * currentPos.y - mapRadius.y)
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    tilesToWrapAroundMap.Add(pos);
                }
            }
        }


        foreach (Vector2Int pos in tilesToWrapAroundMap)
        {
            if (!dictPositionToTile.ContainsKey(pos)) continue;
            GameObject tile = dictPositionToTile[pos];
            dictPositionToTile.Remove(pos);
            Vector2Int newPos = pos;// - offset;
            if (newPos.x > currentPos.x + mapRadius.x) newPos.x -= mapSize.x;
            else if (newPos.x < currentPos.x - mapRadius.x) newPos.x += mapSize.x;

            if (newPos.y > currentPos.y + mapRadius.y) newPos.y -= mapSize.y;
            else if (newPos.y < currentPos.y - mapRadius.y) newPos.y += mapSize.y;
            
            
            Vector3 deltaPos = PositionToWorld(newPos - pos);
            Collider2D[] ennemies = Physics2D.OverlapAreaAll(
                (pos - 0.5f*Vector2.one) * tileSize, 
                (pos + 0.5f*Vector2.one) * tileSize, 
                mask);
            Debug.Log(ennemies.Length);
            foreach (var ennemy in ennemies)
            {
                if (ennemy.CompareTag("Stele")) continue;
                ennemy.transform.position += deltaPos;
            }
            
            
            dictPositionToTile.Add(newPos, tile);
            tile.transform.position = PositionToWorld(newPos);
        }

        lastPos = currentPos;
    }


}

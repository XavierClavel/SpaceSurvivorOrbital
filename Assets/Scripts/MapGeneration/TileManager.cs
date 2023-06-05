using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] SpriteRenderer groundSprite;
    [SerializeField] TilesBank tilesBank;
    TileWaveFunction[,] map;
    [SerializeField] DistanceConstraintsManager distanceConstraintsManager;
    public Vector2Int tileSize = new Vector2Int(10, 10);
    [SerializeField] List<Tile> tiles;
    [SerializeField] Tile spaceship;
    List<TileWaveFunction> uncollapsedTiles = new List<TileWaveFunction>();
    Dictionary<Vector2Int, GameObject> dictPositionToTile = new Dictionary<Vector2Int, GameObject>();
    PlayerController player;
    [SerializeField] int planetSize = 9;
    Vector2Int mapSize;
    Vector2Int activationRadius = new Vector2Int(3, 3); //radius around player in which tiles are activated
    Vector2Int lastPos = Vector2Int.zero;
    Vector2Int mapRadius;
    List<Tile> tilesToPlace = new List<Tile>();
    [SerializeField] Tile green1;
    [SerializeField] Tile green2;
    [SerializeField] Tile green3;

    [SerializeField] Tile orange1;
    [SerializeField] Tile orange2;
    [SerializeField] Tile orange3;
    [SerializeField] Tile violet;

    [HideInInspector] public static int tilesInAdvance => instance.uncollapsedTiles.Count - instance.tilesToPlace.Count;
    [HideInInspector] public static int tilesToPlaceAmount => instance.tilesToPlace.Count;

    float noiseFactor = 0.3f;   //chance to collapse a random tile
    public static TileManager instance;
    const float tileRotationPeriod = 1f;
    //TODO : adapat tilerotationperiod to planet size


    public static Tile getTileToPlace(List<Tile> possibleStates)
    {
        List<Tile> intersection = instance.tilesToPlace.Intersection(possibleStates);
        if (intersection.Count == 0) return null;
        return intersection.getRandom();
    }


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        SetupPlanet();

        mapRadius = (mapSize - Vector2Int.one) / 2;
        player = PlayerController.instance;

        foreach (Tile tile in tiles)
        {
            tile.Reset();
            tilesToPlace.AddMultiple(tile, tile.minAmount);
        }

        foreach (DistanceConstraint distanceConstraint in distanceConstraintsManager.distanceConstraints)
        {
            distanceConstraint.Apply();
        }

        foreach (DistanceConstraintGroup distanceConstraintGroup in distanceConstraintsManager.distanceConstraintGroups)
        {
            distanceConstraintGroup.Apply();
        }

        InitalizeMap();
        PlaceTiles();

        StartCoroutine("TileManagement");
    }

    void SetupPlanet()
    {
        if (PlanetManager.hasData())
        {
            groundSprite.color = PlanetManager.getGroundColor();
            planetSize = PlanetManager.getSize();

            Vector3Int greenResourceAllocation = AllocateResource(PlanetManager.getGreenAmount());
            Debug.Log(greenResourceAllocation);
            green1.setSpecificAmount(greenResourceAllocation.x);
            green2.setSpecificAmount(greenResourceAllocation.y);
            green3.setSpecificAmount(greenResourceAllocation.z);

            if (green1.maxAmount == 0) tiles.Remove(green1);
            if (green2.maxAmount == 0) tiles.Remove(green2);
            if (green3.maxAmount == 0) tiles.Remove(green3);

            Vector3Int orangeResourceAllocation = AllocateResource(PlanetManager.getOrangeAmount());
            Debug.Log(orangeResourceAllocation);
            orange1.setSpecificAmount(orangeResourceAllocation.x);
            orange2.setSpecificAmount(orangeResourceAllocation.y);
            orange3.setSpecificAmount(orangeResourceAllocation.z);

            if (orange1.maxAmount == 0) tiles.Remove(orange1);
            if (orange2.maxAmount == 0) tiles.Remove(orange2);
            if (orange3.maxAmount == 0) tiles.Remove(orange3);

            int violetAmount = PlanetManager.getVioletAmount();
            violet.setSpecificAmount(violetAmount);
            Debug.Log(violetAmount);

            if (violet.maxAmount == 0) tiles.Remove(violet);


        }
        mapSize = new Vector2Int(planetSize, planetSize);
    }

    Vector3Int AllocateResource(int resourceAmount)
    {
        int amountBlock1 = resourceAmount;
        int amountBlock2 = 0;
        int amountBlock3 = 0;
        if (resourceAmount == 0) return new Vector3Int(amountBlock1, amountBlock2, amountBlock3);

        amountBlock3 = Random.Range(0, resourceAmount / 3);
        amountBlock1 -= amountBlock3 * 3;

        amountBlock2 = Random.Range(0, amountBlock1 / 2);
        amountBlock1 -= amountBlock2 * 2;

        return new Vector3Int(amountBlock1, amountBlock2, amountBlock3);
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
        map[mapRadius.x, mapRadius.y].possibleStates = new List<Tile>();
        map[mapRadius.x, mapRadius.y].possibleStates.Add(spaceship);
        map[mapRadius.x, mapRadius.y].entropy = int.MinValue;

        PlayerController.instance.spaceship = CollapseWaveFunction(map[mapRadius.x, mapRadius.y]);
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
        GameObject tile = Instantiate(newTile.tileObject, worldPosition, Quaternion.identity);
        dictPositionToTile.Add(position, tile);
        map[index.x, index.y] = null;
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
            yield return Helpers.GetWait(tileRotationPeriod);
            UpdateActiveTiles();
        }
    }

    void UpdateActiveTiles()
    {
        Vector2Int currentPos = Helpers.RoundToVector2IndexStep(player.transform.position, tileSize);
        Vector2Int offset = currentPos - lastPos;

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
            Vector2Int newPos = Helpers.CentralSymmetry(pos, lastPos) + offset;
            dictPositionToTile.Add(newPos, tile);
            tile.transform.position = PositionToWorld(newPos);
        }


        List<GameObject> tilesToDeactivate = new List<GameObject>();
        List<GameObject> tilesToActivate = new List<GameObject>();

        /*
        for (int x = currentPos.x - activationRadius.x; x < -currentPos.x + activationRadius.x; x++)
        {
            for (int )
        }
        */

        lastPos = currentPos;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    TileWaveFunction[,] map;
    [SerializeField] Vector2Int tileSize = new Vector2Int(10, 10);
    [SerializeField] List<Tile> tiles;
    [SerializeField] Tile spaceship;
    List<TileWaveFunction> uncollapsedTiles = new List<TileWaveFunction>();
    Dictionary<Vector2Int, GameObject> dictPositionToTile = new Dictionary<Vector2Int, GameObject>();
    PlayerController player;
    Vector2Int mapSize = new Vector2Int(9, 9);
    Vector2Int activationRadius = new Vector2Int(3, 3); //radius around player in which tiles are activated
    Vector2Int lastPos = Vector2Int.zero;
    Vector2Int mapRadius;

    float noiseFactor = 0.3f;


    // Start is called before the first frame update
    void Awake()
    {
        mapRadius = (mapSize - Vector2Int.one) / 2;
        player = PlayerController.instance;

        foreach (Tile tile in tiles)
        {
            tile.Reset();
        }

        InitalizeMap();
        PlaceTiles();

        StartCoroutine("TileManagement");
    }

    void InitalizeMap()
    {
        map = new TileWaveFunction[mapSize.x, mapSize.y];
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                map[x, y] = new TileWaveFunction(tiles, new Vector2Int(x, y));
                if (x != mapRadius.x || y != mapRadius.y) uncollapsedTiles.Add(map[x, y]);
            }
        }
        map[mapRadius.x, mapRadius.y].possibleStates = new List<Tile>();
        map[mapRadius.x, mapRadius.y].possibleStates.Add(spaceship);
        map[mapRadius.x, mapRadius.y].entropy = int.MinValue;
        CollapseSpaceship(map[mapRadius.x, mapRadius.y]);
    }

    void CollapseSpaceship(TileWaveFunction tileWaveFunction)
    {
        Tile newTile = tileWaveFunction.CollapseWaveFunction();
        Vector2Int index = tileWaveFunction.index;

        List<Vector2Int> tilesToCollapse;
        foreach (TileConstraint constraint in newTile.constraints)
        {
            tilesToCollapse = index.GetPosInRange(constraint.distance);
            ApplyConstraint(tilesToCollapse, constraint.otherTile);
        }

        Vector2Int position = IndexToPosition(index);
        Vector3 worldPosition = PositionToWorld(position);
        GameObject tile = Instantiate(newTile.tileObject, worldPosition, Quaternion.identity);
        dictPositionToTile.Add(position, tile);
        map[index.x, index.y] = null;
        PlayerController.instance.spaceship = tile;
    }

    void ApplyConstraint(List<Vector2Int> positions, Tile removeTile)
    {
        foreach (Vector2Int position in positions)
        {
            if (map[position.x, position.y] == null) continue;
            map[position.x, position.y].ReduceWaveFunction(removeTile);
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
            if (newTileWaveFunction == null)
            {
                Debug.Log("null tile");
                Debug.Log("remaining tiles : " + uncollapsedTiles.Count);
                break;
            }
            CollapseWaveFunction(newTileWaveFunction);
        }

    }

    TileWaveFunction GetUncollapsedTileOfLeastEntropy()
    {
        if (uncollapsedTiles.Count == 0) return null;
        List<TileWaveFunction> uncollapsedTilesOfLeastEntropy = new List<TileWaveFunction>();
        int minEntropy = int.MaxValue;

        if (Helpers.ProbabilisticBool(noiseFactor))
        {
            return uncollapsedTiles.getRandom();
        }

        foreach (TileWaveFunction tileWaveFunction in uncollapsedTiles)
        {
            if (tileWaveFunction == null) continue;
            if (tileWaveFunction.entropy < minEntropy)
            {
                uncollapsedTilesOfLeastEntropy = new List<TileWaveFunction>();
                uncollapsedTilesOfLeastEntropy.Add(tileWaveFunction);
                minEntropy = tileWaveFunction.entropy;
            }
            else if (tileWaveFunction.entropy == minEntropy)
            {
                uncollapsedTilesOfLeastEntropy.Add(tileWaveFunction);
                //List to randomize the output in case of equality
            }
        }

        if (uncollapsedTilesOfLeastEntropy.Count == 0) return null;
        if (uncollapsedTilesOfLeastEntropy.Count == 1) return uncollapsedTilesOfLeastEntropy[0];
        else return uncollapsedTilesOfLeastEntropy.getRandom();
    }

    void CollapseWaveFunction(TileWaveFunction tileWaveFunction)
    {
        uncollapsedTiles.Remove(tileWaveFunction);

        Tile newTile = tileWaveFunction.CollapseWaveFunction();
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
    }

    Vector2Int IndexToPosition(Vector2Int index)
    {
        return index - mapRadius;
    }


    Vector3 PositionToWorld(Vector2Int index)
    {
        return new Vector3(index.x * tileSize.x, index.y * tileSize.y);
    }

    IEnumerator TileManagement()
    {
        while (true)
        {
            yield return Helpers.GetWait(5f);
            UpdateActiveTiles();
        }
    }

    void UpdateActiveTiles()
    {
        Vector2Int currentPos = Helpers.RoundToVector2IndexStep(player.transform.position, tileSize);
        Debug.Log(currentPos);
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
                    Debug.Log(pos);
                }
            }
        }

        Debug.Log("Tiles to wrap : " + tilesToWrapAroundMap.Count);

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

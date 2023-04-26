using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] Vector2Int tileSize = new Vector2Int(10, 10);
    [SerializeField] List<GameObject> tiles;
    [SerializeField] GameObject spaceship;
    [SerializeField] List<TileConstraint> tilesConstraints;
    Dictionary<Vector2Int, GameObject> dictPositionToTile = new Dictionary<Vector2Int, GameObject>();
    PlayerController player;
    Vector2Int mapSize = new Vector2Int(9, 9);
    Vector2Int activationRadius = new Vector2Int(3, 3); //radius around player in which tiles are activated
    Vector2Int lastPos = Vector2Int.zero;
    Vector2Int mapRadius;


    // Start is called before the first frame update
    void Start()
    {
        mapRadius = (mapSize - Vector2Int.one) / 2;
        player = PlayerController.instance;
        for (int x = -mapRadius.x; x <= mapRadius.x; x++)
        {
            for (int y = -mapRadius.y; y <= mapRadius.y; y++)
            {
                if (x == 0 && y == 0) continue;
                CreateTile(tiles.getRandom(), new Vector2Int(x, y));
            }
        }
        dictPositionToTile.Add(Vector2Int.zero, spaceship);
        StartCoroutine("TileManagement");
    }

    void CreateTile(GameObject tilePrefab, Vector2Int position)
    {
        Vector3 worldPosition = IndexToWorld(position);
        GameObject tile = Instantiate(tilePrefab, worldPosition, Quaternion.identity);
        dictPositionToTile.Add(position, tile);
    }

    Vector3 IndexToWorld(Vector2Int index)
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
        //Debug.Log(currentPos);
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
                    tilesToWrapAroundMap.Add(new Vector2Int(x, y));
                }
            }
        }

        Debug.Log("Tiles to wrap count : " + tilesToWrapAroundMap.Count);

        foreach (Vector2Int pos in tilesToWrapAroundMap)
        {
            GameObject tile = dictPositionToTile[pos];
            dictPositionToTile.Remove(pos);
            //Vector2Int newPos = Helpers.CentralSymmetry(pos, currentPos);
            Vector2Int newPos = Helpers.CentralSymmetry(pos, lastPos) + offset;
            dictPositionToTile.Add(newPos, tile);
            tile.transform.position = IndexToWorld((newPos));
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

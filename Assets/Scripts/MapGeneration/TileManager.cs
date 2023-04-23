using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] Vector2Int tileSize = new Vector2Int(10, 10);
    [SerializeField] List<GameObject> tiles;
    [SerializeField] List<TileConstraint> tilesConstraints;

    // Start is called before the first frame update
    void Start()
    {
        for (int x = -4; x < 4; x++)
        {
            for (int y = -4; y < 4; y++)
            {
                if (x == 0 && y == 0) continue;
                int randomIndex = Random.Range(0, tiles.Count);
                CreateTile(tiles[randomIndex], new Vector2Int(x, y));
            }
        }
    }

    void CreateTile(GameObject tilePrefab, Vector2Int position)
    {
        Vector2 worldPosition = position * tileSize;
        GameObject tile = Instantiate(tilePrefab, worldPosition, Quaternion.identity);
    }
}

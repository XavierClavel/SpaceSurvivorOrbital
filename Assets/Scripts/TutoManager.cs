using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileRow
{
    public List<GameObject> tileRow;
}

public class TutoManager : MonoBehaviour
{

    [SerializeField] Vector2Int tileSize;
    [SerializeField] List<TileRow> tiles;

    // Start is called before the first frame update
    void Start()
    {
        tiles.Reverse();
        TileManager.instance.SetMap(tiles, tileSize);
    }

}

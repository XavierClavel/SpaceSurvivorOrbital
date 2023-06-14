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
    [SerializeField] bool doMapMove;
    [SerializeField] Vector2Int tileSize;
    [SerializeField] List<TileRow> tiles;

    // Start is called before the first frame update
    void Start()
    {
        if (!doMapMove) return;
        tiles.Reverse();
        TileManager.instance.SetMap(tiles, tileSize);
    }

}

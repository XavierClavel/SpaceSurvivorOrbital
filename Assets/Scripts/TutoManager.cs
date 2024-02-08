using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class TileRow
{
    public List<GameObject> tileRow;
}

public class TutoManager : MonoBehaviour, IEnnemyListener, IAltarListener, IResourceListener, IMonsterStele
{
    [SerializeField] bool doMapMove;
    [SerializeField] Vector2Int tileSize;
    [SerializeField] List<TileRow> tiles;

    [SerializeField] private TextMeshProUGUI tutoText;

    private int ennemiesKilled = 0;
    private int altarUsed = 0;
    private int resourceDestroyed = 0;
    private int steleDestroyed = 0;
    

    // Start is called before the first frame update
    void Awake()
    {
        PlanetManager.setData(new PlanetData() {size = planetSize.small});
        if (!PlayerManager.isTuto)
        {
            return;
        }
        Ennemy.registerListener(this);
        Altar.registerListener(this);
        Resource.registerListener(this);
        MonsterStele.registerListener(this);
        
        tiles.Reverse();
        TileManager.instance.SetMap(tiles, tileSize);
        
        StartCoroutine(nameof(Tuto));
    }

    private void OnDestroy()
    {
        Ennemy.unregisterListener(this);
        Altar.unregisterListener(this);
    }

    private IEnumerator Tuto()
    {
        tutoText.SetText("aaaaaaa");
        yield return new WaitUntil(killedFirstWave);
        tutoText.SetText("bbbbbbbbb");
    }

    public bool killedFirstWave() => ennemiesKilled == 1;

    public void onEnnemyDeath(Ennemy ennemy) => ennemiesKilled++;
    public void onAltarUsed(Altar altar) => altarUsed++;
    public void onResourceDestroyed(Resource resource) => resourceDestroyed++;
    public void onSteleDestroyed(MonsterStele stele) => steleDestroyed++;
}

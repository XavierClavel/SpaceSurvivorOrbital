using System;
using System.Collections;
using System.Collections.Generic;
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

    private int ennemiesKilled = 0;
    private int altarUsed = 0;
    private int resourceDestroyed = 0;
    private int steleDestroyed = 0;
    

    // Start is called before the first frame update
    void Awake()
    {
        Ennemy.registerListener(this);
        Altar.registerListener(this);
        Resource.registerListener(this);
        StartCoroutine(nameof(Tuto));
    }

    private void OnDestroy()
    {
        Ennemy.unregisterListener(this);
        Altar.unregisterListener(this);
    }

    private IEnumerator Tuto()
    {
        yield return new WaitUntil(killedFirstWave);
        Debug.Log("Done !");
    }

    public bool killedFirstWave() => ennemiesKilled == 1;

    public void onEnnemyDeath(Ennemy ennemy) => ennemiesKilled++;
    public void onAltarUsed(Altar altar) => altarUsed++;
    public void onResourceDestroyed(Resource resource) => resourceDestroyed++;
    public void onSteleDestroyed(MonsterStele stele) => steleDestroyed++;
}

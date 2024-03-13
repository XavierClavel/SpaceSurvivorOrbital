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
    [SerializeField] GameObject tutoActive;

    [SerializeField] private StringLocalizer tutoText;
    [SerializeField] private StringLocalizer clickText;
    [SerializeField] private TileManager tileManager;

    private int ennemiesKilled = 0;
    private int altarUsed = 0;
    private int resourceDestroyed = 0;
    private int steleDestroyed = 0;

    private List<MonsterStele> steles = new List<MonsterStele>();
    private List<Resource> resources = new List<Resource>();
    public List<Altar> altars = new List<Altar>();
    public bool click = false;

    [SerializeField] private Ennemy ennemyPrefab;
    public static TutoManager instance;
    

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        if (!PlayerManager.isTuto)
        {
            return;
        }
        
        PlanetManager.setData(new PlanetData() {size = planetSize.small});
        
        Ennemy.registerListener(this);
        Altar.registerListener(this);
        Resource.registerListener(this);
        MonsterStele.registerListener(this);
        
        tiles.Reverse();
        tileManager.SetMap(tiles, tileSize);
        
        StartCoroutine(nameof(Tuto));
    }

    private void OnDestroy()
    {
        Ennemy.unregisterListener(this);
        Altar.unregisterListener(this);
        Resource.unregisterListener(this);
        MonsterStele.unregisterListener(this);
    }

    private IEnumerator Tuto()
    {
        tutoActive.SetActive(true);
        clickText.enabled = true;

        tutoText.setKey("Tuto_1");
        yield return Helpers.getWait(2f);
        clickText.setKey("Tuto_shoot");
        yield return new WaitUntil(doClick);

        tutoText.setKey("Tuto_2");
        yield return Helpers.getWait(2f);
        clickText.setKey("Tuto_shoot");
        yield return new WaitUntil(doClick);

        tutoText.setKey("Tuto_3");
        ShowFirstStele();
        clickText.setKey("Tuto_stele");
        yield return new WaitUntil(isSteleDestroyed);
        
        tutoText.setKey("Tuto_4");
        SpawnEnnemies(1);
        clickText.setKey("Tuto_monster");
        yield return new WaitUntil(killedFirstWave);
        
        tutoText.setKey("Tuto_5");
        ShowResources();
        clickText.setKey("Tuto_egg");
        yield return new WaitUntil(resourcesDestroyed);
        
        tutoText.setKey("Tuto_6");
        yield return Helpers.getWait(2f);
        clickText.setKey("Tuto_shoot");
        yield return new WaitUntil(doClick);

        tutoText.setKey("Tuto_7");
        ShowFirstAltar();
        clickText.setKey("Tuto_power");
        yield return new WaitUntil(isAltarUsed);
        
        tutoText.setKey("Tuto_8");
        yield return Helpers.getWait(2f);
        clickText.setKey("Tuto_shoot");
        yield return new WaitUntil(doClick);

        tutoText.setKey("Tuto_9");
        yield return Helpers.getWait(2f);
        clickText.setKey("Tuto_shoot");
        yield return new WaitUntil(doClick);

        tutoText.setKey("Tuto_10");
        SpawnEnnemies(3);
        clickText.setKey("Tuto_monster");
        yield return new WaitUntil(killedSecondWave);
        
        tutoText.setKey("Tuto_11");
        yield return Helpers.getWait(2f);
        clickText.setKey("Tuto_shoot");
        yield return new WaitUntil(doClick);

        tutoText.setKey("Tuto_12");
        yield return Helpers.getWait(2f);
        clickText.setKey("Tuto_shoot");
        yield return new WaitUntil(doClick);

        tutoText.setKey("Tuto_13");
        ShowSecondStele();
        PlayerManager.isTuto = false;
        clickText.setKey("Tuto_shoot");
        yield return new WaitUntil(isStele2Destroyed);
        
        tutoText.setKey("Tuto_14");
        clickText.setKey("Tuto_leave");
        tutoActive.SetActive(false);
    }
    public bool doClick() => click;
    
    public bool isSteleDestroyed() => steleDestroyed == 1;
    public bool killedFirstWave() => ennemiesKilled == 1;
    public bool isStele2Destroyed() => steleDestroyed == 2;
    public bool killedSecondWave() => ennemiesKilled == 4;
    public bool isAltarUsed() => altarUsed == 1;


    public bool resourcesDestroyed() => resourceDestroyed == 4;

    public void onEnnemyDeath(Ennemy ennemy) => ennemiesKilled++;
    public void onAltarUsed(Altar altar) => altarUsed++;
    public void onAltarSpawned(Altar altar)
    {
        altars.Add(altar);
        altar.gameObject.SetActive(false);
    }

    public void onResourceDestroyed(Resource resource) => resourceDestroyed++;
    public void onResourceSpawned(Resource resource)
    {
        resources.Add(resource);
        resource.gameObject.SetActive(false);
    }

    public void onSteleSpawned(MonsterStele stele)
    {
        steles.Add(stele);
        stele.gameObject.SetActive(false);
    }

    public void onSteleDestroyed(MonsterStele stele) => steleDestroyed++;

    private void ShowFirstStele() => steles[0].gameObject.SetActive(true);

    private void ShowSecondStele() => steles[1].gameObject.SetActive(true);

    private void ShowResources() => resources.ForEach(it => it.gameObject.SetActive(true));
    private void ShowFirstAltar() => altars[0].gameObject.SetActive(true);

    private void SpawnEnnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Spawner.SpawnEnnemy(ennemyPrefab);
        }
    }

    public void Click()
    {
        StartCoroutine(nameof(clickCoroutine));
    }

    private IEnumerator clickCoroutine()
    {
        click = true;
        yield return null;
        click = false;
    }
}

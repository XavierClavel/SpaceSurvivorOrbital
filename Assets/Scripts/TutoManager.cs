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
    [SerializeField] private TileManager tileManager;

    private int ennemiesKilled = 0;
    private int altarUsed = 0;
    private int resourceDestroyed = 0;
    private int steleDestroyed = 0;

    private List<MonsterStele> steles = new List<MonsterStele>();
    

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
        tileManager.SetMap(tiles, tileSize);
        
        StartCoroutine(nameof(Tuto));
    }

    private void OnDestroy()
    {
        Ennemy.unregisterListener(this);
        Altar.unregisterListener(this);
    }

    private IEnumerator Tuto()
    {
        tutoText.SetText("Bienvenue dans Cosmic Deserter !\r\nVotre patrie a �t� d�truite par une arm�e Alien. \r\nVous devez fuir de cette galaxie � tout prix !");
        yield return new WaitUntil(killedFirstWave);
        tutoText.SetText("Se d�placer : ZQSD \r\nTirer : Clic gauche");
        yield return new WaitUntil(killedFirstWave);
        tutoText.SetText("Pour fuir d'une plan�te, vous devez d�truire les st�les ennemis. \r\nLe nombre de st�les ennemis restantes est visible en bas � droite.");
        yield return new WaitUntil(killedFirstWave);
        tutoText.SetText("Trouvez et d�truisez une st�le !");
        yield return new WaitUntil(isSteleDestroyed);
        tutoText.SetText("Bien jou� ! Attention un ennemi ! D�truisez le !");
        yield return new WaitUntil(killedFirstWave);
        tutoText.SetText("Super ! Maintenant, r�coltez des ressources en d�truisant des oeufs.");

    }
    public bool isSteleDestroyed() => steleDestroyed == 1;
    public bool killedFirstWave() => ennemiesKilled == 1;

    public bool ressourcesDestroyed() => ennemiesKilled == 1;

    public void onEnnemyDeath(Ennemy ennemy) => ennemiesKilled++;
    public void onAltarUsed(Altar altar) => altarUsed++;
    public void onResourceDestroyed(Resource resource) => resourceDestroyed++;

    public void onSteleSpawned(MonsterStele stele)
    {
        steles.Add(stele);
        stele.gameObject.SetActive(false);
    }

    public void onSteleDestroyed(MonsterStele stele) => steleDestroyed++;

    private void ShowFirstStele()
    {
        steles[0].gameObject.SetActive(true);
    }

    private void ShowSecondStele()
    {
        steles[1].gameObject.SetActive(false);
    }
}

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

    [SerializeField] private TextMeshProUGUI tutoText;
    [SerializeField] private TextMeshProUGUI clickText;
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

        tutoText.SetText("Bienvenue dans Cosmic Deserter !\r\nSe déplacer : ZQSD \r\nTirer : Clic gauche");
        yield return Helpers.getWait(2f);
        clickText.enabled = true;
        clickText.SetText("Tirer");
        yield return new WaitUntil(doClick);
        clickText.enabled = false;

        tutoText.SetText("Votre planète a été détruite par une armée Alien. \r\nVous devez fuir cette galaxie à tout prix !");
        yield return Helpers.getWait(2f);
        clickText.enabled = true;
        clickText.SetText("Tirer");
        yield return new WaitUntil(doClick);
        clickText.enabled = false;

        tutoText.SetText("Pour fuir d'une planète, vous devez détruire les stèles ennemis. \r\nLe nombre de stèles ennemis restantes est visible en haut à droite. \n Trouvez et détruisez une stèle !");
        ShowFirstStele();
        yield return new WaitUntil(isSteleDestroyed);
        
        tutoText.SetText("Super ! Attention un monstre !");
        SpawnEnnemies(1);
        yield return new WaitUntil(killedFirstWave);
        
        tutoText.SetText("Maintenant, récoltez des ressources en détruisant des oeufs.");
        ShowResources();
        yield return new WaitUntil(resourcesDestroyed);
        
        tutoText.SetText("Vous gagnez une ressource verte ou jaune quand la jauge à droite se remplit à 100%. \r\n En détruisant tous les oeufs d'une planète, vous gagnez une ressource bleu.");
        yield return Helpers.getWait(2f);
        clickText.enabled = true;
        clickText.SetText("Tirer");
        yield return new WaitUntil(doClick);
        clickText.enabled = false;

        tutoText.SetText("Fouillez la planète pour découvrir un autel de pouvoir. \r\nPuis positionnez vous devant (dans le cercle).");
        ShowFirstAltar();
        yield return new WaitUntil(isAltarUsed);
        
        tutoText.SetText("Les ressources bleus servent à améliorer vos pouvoirs. \r\nLes jaunes et vertes, vos équipements.");
        yield return Helpers.getWait(2f);
        clickText.SetText("Tirer");
        clickText.enabled = true;
        yield return new WaitUntil(doClick);
        clickText.enabled = false;

        tutoText.SetText("Chaque planète possède des ressources d'un type spécifique.");
        yield return Helpers.getWait(2f);
        clickText.enabled = true;
        clickText.SetText("Tirer");
        yield return new WaitUntil(doClick);
        clickText.enabled = false;


        tutoText.SetText("D'autres ennemis !");
        SpawnEnnemies(3);
        yield return new WaitUntil(killedSecondWave);
        
        tutoText.SetText("Avez-vous remarqué ? Comme chaque planète est sphérique, en marchant dans le même sens, vous en ferez le tour.");
        yield return Helpers.getWait(2f);
        clickText.enabled = true;
        clickText.SetText("Tirer");
        yield return new WaitUntil(doClick);
        clickText.enabled = false;

        tutoText.SetText("La taille d'une planète varie, la couleur du sol vous indique à quelle distance du centre vous êtes. Le centre sera toujours plus clair.");
        yield return Helpers.getWait(2f);
        clickText.enabled = true;
        clickText.SetText("Tirer");
        yield return new WaitUntil(doClick);
        clickText.enabled = false;

        tutoText.SetText("Une fois toutes les stèles détruites, vous pouvez vous téléportez dans votre vaisseau. \r\nLa dernière stèle vient d'apparaitre, détruisez là !");
        ShowSecondStele();
        PlayerManager.isTuto = false;
        yield return new WaitUntil(isStele2Destroyed);
        
        tutoText.SetText("Le cercle de téléportation vient d'apparaitre au centre de la planète. \r\nQuand vous souhaitez partir, entrez dans le cercle de téléportation !");
        yield return Helpers.getWait(3f);
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

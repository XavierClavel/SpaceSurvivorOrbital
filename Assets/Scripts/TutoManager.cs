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
        tutoActive.SetActive(true);
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
        Resource.unregisterListener(this);
        MonsterStele.unregisterListener(this);
    }

    private IEnumerator Tuto()
    {
        tutoText.SetText("Bienvenue dans Cosmic Deserter !\r\nVotre planète a été détruite par une armée Alien. \r\nVous devez fuir de cette galaxie à tout prix ! \n Cliquez pour continuer");
        yield return Helpers.getWait(2f);
        yield return new WaitUntil(doClick);
        
        tutoText.SetText("Se déplacer : ZQSD \r\nTirer : Clic gauche");
        yield return Helpers.getWait(2f);
        yield return new WaitUntil(doClick);
        
        tutoText.SetText("Pour fuir d'une planète, vous devez détruire les stèles ennemis. \r\nLe nombre de stèles ennemis restantes est visible en bas à droite.");
        yield return Helpers.getWait(2f);
        yield return new WaitUntil(doClick);
        
        tutoText.SetText("Trouvez et détruisez une stèle !");
        ShowFirstStele();
        yield return new WaitUntil(isSteleDestroyed);
        
        tutoText.SetText("Bien joué ! Attention un ennemi ! Détruisez le !");
        SpawnEnnemies(1);
        yield return new WaitUntil(killedFirstWave);
        
        tutoText.SetText("Super ! Maintenant, récoltez des ressources en détruisant des oeufs.");
        ShowResources();
        yield return new WaitUntil(resourcesDestroyed);
        
        tutoText.SetText("Une ressource verte ou jaune se gagne en remplissant les jauges en haut à droite. \r\n En détruisant tous les oeufs d'une planète, vous gagnez une ressource bleu");
        yield return Helpers.getWait(2f);
        yield return new WaitUntil(doClick);
        
        tutoText.SetText("Fouillez la planète pour découvrir un autel de pouvoir. \r\nPuis positionnez vous devant (dans le cercle).");
        ShowFirstAltar();
        yield return new WaitUntil(isAltarUsed);
        
        tutoText.SetText("Les ressources bleu servent à améliorer vos pouvoirs. \r\nLes jaune et verte, vos équipements.");
        yield return Helpers.getWait(2f);
        yield return new WaitUntil(doClick);
        
        tutoText.SetText("Chaque planète possède des ressources d'un type spécifique.");
        yield return Helpers.getWait(2f);
        yield return new WaitUntil(doClick);
        
        tutoText.SetText("D'autres ennemis ! Faites leur la peau !!!");
        SpawnEnnemies(3);
        yield return new WaitUntil(killedSecondWave);
        
        tutoText.SetText("Avez-vous remarqué ? Comme chaque planète est sphérique, en marchant dans le même sens, vous en ferez le tour. Plus vous vous éloignez du centre, plus le sol sera sombre");
        yield return Helpers.getWait(2f);
        yield return new WaitUntil(doClick);
        
        tutoText.SetText("La dernière stèle vient d'apparaitre, détruisez là !");
        ShowSecondStele();
        PlayerManager.isTuto = false;
        yield return new WaitUntil(isStele2Destroyed);
        
        tutoText.SetText("Une fois les stèles détruites, vous pouvez vous téléportez dans votre vaisseau. \r\nLe cercle de téléportation apparait au centre de la planète");
        yield return Helpers.getWait(2f);
        yield return new WaitUntil(doClick);
        
        tutoText.SetText("Quand vous êtes prêt à partir, entre dans le cercle de téléportation !\r\nBon courage !");
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

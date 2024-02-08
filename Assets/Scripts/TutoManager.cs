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
    private bool click = false;
    

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

    private void Update()
    {
        click = Input.GetMouseButtonDown(0);
    }

    private IEnumerator Tuto()
    {
        tutoText.SetText("Bienvenue dans Cosmic Deserter !\r\nVotre patrie a �t� d�truite par une arm�e Alien. \r\nVous devez fuir de cette galaxie � tout prix !");
        yield return new WaitUntil(isCLicked);
        tutoText.SetText("Se d�placer : ZQSD \r\nTirer : Clic gauche");
        yield return new WaitUntil(isCLicked);
        tutoText.SetText("Pour fuir d'une plan�te, vous devez d�truire les st�les ennemis. \r\nLe nombre de st�les ennemis restantes est visible en bas � droite.");
        yield return new WaitUntil(isCLicked);
        tutoText.SetText("Trouvez et d�truisez une st�le !");
        yield return new WaitUntil(isSteleDestroyed);
        tutoText.SetText("Bien jou� ! Attention un ennemi ! D�truisez le !");
        yield return new WaitUntil(killedFirstWave);
        tutoText.SetText("Super ! Maintenant, r�coltez des ressources en d�truisant des oeufs.");
        yield return new WaitUntil(resourcesDestroyed);
        tutoText.SetText("Une ressource verte ou jaune se gagne en remplissant les jauges en haut � droite. \r\n En d�truisant tous les oeufs d'une plan�te, vous gagnez une ressource bleu");
        yield return new WaitUntil(isCLicked);
        tutoText.SetText("Fouillez la plan�te pour d�couvrir un autel de pouvoir. \r\nPuis positionnez vous devant (dans le cercle).");
        yield return new WaitUntil(isAltarUsed);
        tutoText.SetText("Les ressources bleu servent � am�liorer vos pouvoirs. \r\nLes jaune et verte, vos �quipements.");
        yield return new WaitUntil(isCLicked);
        tutoText.SetText("Chaque plan�te poss�de des ressources d'un type sp�cifique.");
        yield return new WaitUntil(isCLicked);
        tutoText.SetText("D'autres ennemis ! Faites leur la peau !!!");
        yield return new WaitUntil(killedSecondWave);
        tutoText.SetText("Avez-vous remarqu� ? Chaque plan�te est sph�rique ! En marchant dans le m�me sens, vous en ferez donc le tour.");
        yield return new WaitUntil(isCLicked);
        tutoText.SetText("La derni�re st�le vient d'apparaitre, d�truisez l� !");
        yield return new WaitUntil(isStele2Destroyed);
        tutoText.SetText("Une fois les st�les d�truites, vous pouvez vous t�l�portez dans votre vaisseau. \r\nLe cercle de t�l�portation apparait au centre de la plan�te");
        yield return new WaitUntil(isCLicked);
        tutoText.SetText("Quand vous �tes pr�t � partir, entre dans le cercle de t�l�portation !\r\nBon courage !");

    }
        tutoText.SetText("Super ! Maintenant, r�coltez des ressources en d�truisant des oeufs.");

    }

    public bool doClick() => click;
    
    public bool isSteleDestroyed() => steleDestroyed == 1;
    public bool killedFirstWave() => ennemiesKilled == 1;

    public bool isCLicked() => clicked == 1;
    public bool isSteleDestroyed() => steleDestroyed == 1;
    public bool isStele2Destroyed() => steleDestroyed == 2;
    public bool killedFirstWave() => ennemiesKilled == 1;
    public bool killedSecondWave() => ennemiesKilled == 4;
    public bool isAltarUsed() => altarUsed == 1;


    public bool resourcesDestroyed() => resourceDestroyed == 1;

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

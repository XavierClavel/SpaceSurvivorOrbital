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
    private int clicked = 0;
    

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
        tutoText.SetText("Bienvenue dans Cosmic Deserter !\r\nVotre patrie a été détruite par une armée Alien. \r\nVous devez fuir de cette galaxie à tout prix !");
        yield return new WaitUntil(isCLicked);
        tutoText.SetText("Se déplacer : ZQSD \r\nTirer : Clic gauche");
        yield return new WaitUntil(isCLicked);
        tutoText.SetText("Pour fuir d'une planète, vous devez détruire les stèles ennemis. \r\nLe nombre de stèles ennemis restantes est visible en bas à droite.");
        yield return new WaitUntil(isCLicked);
        tutoText.SetText("Trouvez et détruisez une stèle !");
        yield return new WaitUntil(isSteleDestroyed);
        tutoText.SetText("Bien joué ! Attention un ennemi ! Détruisez le !");
        yield return new WaitUntil(killedFirstWave);
        tutoText.SetText("Super ! Maintenant, récoltez des ressources en détruisant des oeufs.");
        yield return new WaitUntil(resourcesDestroyed);
        tutoText.SetText("Une ressource verte ou jaune se gagne en remplissant les jauges en haut à droite. \r\n En détruisant tous les oeufs d'une planète, vous gagnez une ressource bleu");
        yield return new WaitUntil(isCLicked);
        tutoText.SetText("Fouillez la planète pour découvrir un autel de pouvoir. \r\nPuis positionnez vous devant (dans le cercle).");
        yield return new WaitUntil(isAltarUsed);
        tutoText.SetText("Les ressources bleu servent à améliorer vos pouvoirs. \r\nLes jaune et verte, vos équipements.");
        yield return new WaitUntil(isCLicked);
        tutoText.SetText("Chaque planète possède des ressources d'un type spécifique.");
        yield return new WaitUntil(isCLicked);
        tutoText.SetText("D'autres ennemis ! Faites leur la peau !!!");
        yield return new WaitUntil(killedSecondWave);
        tutoText.SetText("Avez-vous remarqué ? Chaque planète est sphérique ! En marchant dans le même sens, vous en ferez donc le tour.");
        yield return new WaitUntil(isCLicked);
        tutoText.SetText("La dernière stèle vient d'apparaitre, détruisez là !");
        yield return new WaitUntil(isStele2Destroyed);
        tutoText.SetText("Une fois les stèles détruites, vous pouvez vous téléportez dans votre vaisseau. \r\nLe cercle de téléportation apparait au centre de la planète");
        yield return new WaitUntil(isCLicked);
        tutoText.SetText("Quand vous êtes prêt à partir, entre dans le cercle de téléportation !\r\nBon courage !");

    }

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
    public void onSteleDestroyed(MonsterStele stele) => steleDestroyed++;
}

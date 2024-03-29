using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = Vault.other.scriptableObjectMenu + "tilesBankManager", order = 1)]
public class TilesBankManager : ScriptableObject
{

    [Header("Empty")]
    public Tile empty1;
    public Tile empty2;

    [Header("Ressource")]
    public Tile ressource1;
    public Tile ressource2;
    public Tile ressource3;

    [Header("Altar")]
    public Tile altar1;

    [Header("Den")]
    public Tile den1;

    [Header("Other")]
    public Tile spaceship;
    public SpriteRenderer ground;

    public Color groundColor1;
    public Color groundColor2;
    public Color groundColor3;

    

    public void setTiles()
    {
        List<Tile> usedEmptyTiles = new List<Tile>();
        TilesBank bank = getBank();

        spaceship.setTileObjects(bank.spaceship);
        
        altar1.setTileObjects(bank.altar1);

        den1.setTileObjects(bank.den1);

        ressource1.setTileObjects(bank.ressource1);
        ressource2.setTileObjects(bank.ressource2);
        ressource3.setTileObjects(bank.ressource3);

        empty1.setTileObjects(bank.empty1);
        empty2.setTileObjects(bank.empty2);

        groundColor1 = bank.groundColor1;
        groundColor2 = bank.groundColor2;
        groundColor3 = bank.groundColor3;
        ground = bank.groundForm;
    }

    public List<Ennemy> GetEnnemies()
    {
        TilesBank bank = getBank();
        return bank.ennemies.Copy();
    }

    TilesBank getBank()
    {
        return ScriptableObjectManager.dictTypeToTilesBank[PlanetManager.getType()];
    }


}

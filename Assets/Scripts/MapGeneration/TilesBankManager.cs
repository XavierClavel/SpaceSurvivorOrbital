using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = Vault.other.scriptableObjectMenu + "tilesBankManager", order = 1)]
public class TilesBankManager : ScriptableObject
{
    [Header("Tiles Banks")]
    public TilesBank icePlanetBank;
    public TilesBank mushroomPlanetBank;
    public TilesBank junglePlanetBank;
    //public TilesBank stormPlanetBank;
    public TilesBank desertPlanetBank;

    [Header("Empty")]
    public Tile empty1;
    public Tile empty2;
    public Tile empty3;

    [Header("Green")]
    public Tile green1;
    public Tile green2;
    public Tile green3;

    [Header("Yellow")]
    public Tile yellow1;
    public Tile yellow2;
    public Tile yellow3;

    [Header("Altar")]
    public Tile altar1;
    public Tile altar2;
    public Tile altar3;

    [Header("Den")]
    public Tile den1;
    public Tile den2;
    public Tile den3;

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
        altar2.setTileObjects(bank.altar2);
        altar3.setTileObjects(bank.altar3);

        den1.setTileObjects(bank.den1);
        den2.setTileObjects(bank.den2);
        den3.setTileObjects(bank.den3);

        green1.setTileObjects(bank.green1);
        green2.setTileObjects(bank.green2);
        green3.setTileObjects(bank.green3);

        yellow1.setTileObjects(bank.yellow1);
        yellow2.setTileObjects(bank.yellow2);
        yellow3.setTileObjects(bank.yellow3);

        empty1.setTileObjects(bank.empty1);
        empty2.setTileObjects(bank.empty2);
        empty3.setTileObjects(bank.empty3);

        TileManager.instance.groundSprite.color = bank.groundColor1;
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
        switch (PlanetManager.getType())
        {
            case planetType.ice:
                return icePlanetBank;

            case planetType.mushroom:
                return mushroomPlanetBank;

            case planetType.desert:
                return desertPlanetBank;

            case planetType.jungle:
                return junglePlanetBank;

            //case planetType.storm:
            //    return stormPlanetBank;

            default:
                return mushroomPlanetBank;

        }

    }


}

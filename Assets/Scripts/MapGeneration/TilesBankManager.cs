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
    
    public Tile empty;

    [Header("Green")]
    public Tile greenLow;
    public Tile greenMid;
    public Tile greenStrong;

    [Header("Yellow")]
    public Tile yellowLow;
    public Tile yellowMid;
    public Tile yellowStrong;

    [Header("Extra")]
    public Tile autel;
    public Tile den;
    public Tile spaceship;
    

    public void setTiles()
    {
        List<Tile> usedEmptyTiles = new List<Tile>();
        TilesBank bank = getBank();

        spaceship.setTileObjects(bank.spaceship);
        autel.setTileObjects(bank.autel);
        den.setTileObjects(bank.den);
        
        greenLow.setTileObjects(bank.greenLow);
        greenMid.setTileObjects(bank.greenMid);
        greenStrong.setTileObjects(bank.greenStrong);

        yellowLow.setTileObjects(bank.yellowLow);
        yellowMid.setTileObjects(bank.yellowMid);
        yellowStrong.setTileObjects(bank.yellowStrong);

        empty.setTileObjects(bank.emptyTiles);

        TileManager.instance.groundSprite.color = bank.groundColor;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesBankManager : MonoBehaviour
{
    [Header("Tiles Banks")]
    [SerializeField] TilesBank icePlanetBank;
    [SerializeField] TilesBank mushroomPlanetBank;
    [SerializeField] TilesBank junglePlanetBank;
    [SerializeField] TilesBank stormPlanetBank;
    [SerializeField] TilesBank desertPlanetBank;

    [Header("Spaceship")]
    [SerializeField] Tile spaceship;

    [Header("Violet")]
    [SerializeField] Tile violet1;
    [SerializeField] Tile violet2;
    [SerializeField] Tile violet3;

    [Header("Green")]
    [SerializeField] Tile greenLow0;
    [SerializeField] Tile greenLow1;
    [SerializeField] Tile greenMid0;
    [SerializeField] Tile greenMid1;
    [SerializeField] Tile greenStrong0;
    [SerializeField] Tile greenStrong1;

    [Header("Yellow")]
    [SerializeField] Tile yellowLow0;
    [SerializeField] Tile yellowLow1;
    [SerializeField] Tile yellowMid0;
    [SerializeField] Tile yellowMid1;
    [SerializeField] Tile yellowStrong0;
    [SerializeField] Tile yellowStrong1;

    [Header("Bonus")]
    [SerializeField] Tile autel;

    [SerializeField] List<Tile> emptyTiles;

    public void getTiles()
    {
        List<Tile> usedEmptyTiles = new List<Tile>();
        TilesBank bank = getBank();

        spaceship.tileObject = bank.spaceship;

        violet1.tileObject = bank.violet1;
        violet2.tileObject = bank.violet2;
        violet3.tileObject = bank.violet3;

        greenLow0.tileObject = bank.greenLow0;
        greenLow1.tileObject = bank.greenLow1;
        greenMid0.tileObject = bank.greenMid0;
        greenMid1.tileObject = bank.greenMid1;
        greenStrong0.tileObject = bank.greenStrong0;
        greenStrong1.tileObject = bank.greenStrong1;

        yellowLow0.tileObject = bank.yellowLow0;
        yellowLow1.tileObject = bank.yellowLow1;
        yellowMid0.tileObject = bank.yellowMid0;
        yellowMid1.tileObject = bank.yellowMid1;
        yellowStrong0.tileObject = bank.yellowStrong0;
        yellowStrong1.tileObject = bank.yellowStrong1;

        autel.tileObject = bank.autel;

        for (int i = 0; i < bank.emptyTiles.Count; i++)
        {
            emptyTiles[i].tileObject = bank.emptyTiles[i];
            usedEmptyTiles.Add(emptyTiles[i]);
        }


        TileManager.instance.spaceship = spaceship;

        TileManager.instance.violet1 = violet1;
        TileManager.instance.violet2 = violet2;
        TileManager.instance.violet3 = violet3;

        TileManager.instance.greenLow0 = greenLow0;
        TileManager.instance.greenLow1 = greenLow1;
        TileManager.instance.greenMid0 = greenMid0;
        TileManager.instance.greenMid1 = greenMid1;
        TileManager.instance.greenStrong0 = greenStrong0;
        TileManager.instance.greenStrong1 = greenStrong1;

        TileManager.instance.yellowLow0 = yellowLow0;
        TileManager.instance.yellowLow1 = yellowLow1;
        TileManager.instance.yellowMid0 = yellowMid0;
        TileManager.instance.yellowMid1 = yellowMid1;
        TileManager.instance.yellowStrong0 = yellowStrong0;
        TileManager.instance.yellowStrong1 = yellowStrong1;

        TileManager.instance.autel = autel;

        TileManager.instance.tiles = usedEmptyTiles.Copy();

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

            case planetType.storm:
                return stormPlanetBank;

            default:
                return mushroomPlanetBank;

        }

    }


}

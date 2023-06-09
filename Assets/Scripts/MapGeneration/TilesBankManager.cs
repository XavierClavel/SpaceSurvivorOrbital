using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesBankManager : MonoBehaviour
{
    [Header("Tiles Banks")]
    [SerializeField] TilesBank bluePlanetBank;
    [SerializeField] TilesBank brownPlanetBank;
    [SerializeField] TilesBank redPlanetBank;

    [Header("Spaceship")]
    [SerializeField] Tile spaceship;

    [Header("Violet")]
    [SerializeField] Tile violet1;
    [SerializeField] Tile violet2;
    [SerializeField] Tile violet3;

    [Header("Green")]
    [SerializeField] Tile green1;
    [SerializeField] Tile green2;
    [SerializeField] Tile green3;

    [Header("Orange")]
    [SerializeField] Tile orange1;
    [SerializeField] Tile orange2;
    [SerializeField] Tile orange3;

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

        green1.tileObject = bank.green1;
        green2.tileObject = bank.green2;
        green3.tileObject = bank.green3;

        orange1.tileObject = bank.orange1;
        orange2.tileObject = bank.orange2;
        orange3.tileObject = bank.orange3;

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

        TileManager.instance.green1 = green1;
        TileManager.instance.green2 = green2;
        TileManager.instance.green3 = green3;

        TileManager.instance.orange1 = orange1;
        TileManager.instance.orange2 = orange2;
        TileManager.instance.orange3 = orange3;

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
            case planetType.blue:
                return bluePlanetBank;

            case planetType.red:
                return redPlanetBank;

            case planetType.brown:
                return brownPlanetBank;

            default:
                return bluePlanetBank;

        }

    }


}

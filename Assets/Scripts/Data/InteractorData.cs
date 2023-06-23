using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractorData
{
    public string name;
    public Vector2Int baseDamage = new Vector2Int(80, 120);
    public int attackSpeed = 10;
    public float range = 10;

    public float bulletReloadTime = 0.2f;
    public float magazineReloadTime = 1;

    public float criticalChance = 0.03f;
    public float criticalMultiplier = 1.5f;

    public int magazine = 6;
    public int projectiles = 1;
    public float spread = 10f;

    public int pierce = 0;
    public float speedWhileAiming = 0.7f;

    static Dictionary<int, int> mapper = new Dictionary<int, int>();

    static List<string> firstLine = new List<string> {
        "Name",
        "BaseDamage",
        "AttackSpeed",
        "Range",
        "Cooldown",
        "Pierce",
        "Projectiles",
        "Spread",
        "SpeedWhileAiming",
        "CriticalMultiplier",
        "Magazine",
        "MagazineReloadTime"
    };

    public GameObject getObject()
    {
        switch (name)
        {
            case "gun":
                return ObjectManager.instance.gun;
        }
        return null;
    }

    public InteractorData(List<string> s)
    {
        if (s.Count != 13) throw new System.ArgumentOutOfRangeException();
        name = s[mapper[0]];
        //baseDamage;
        attackSpeed = int.Parse(s[mapper[2]]);
        range = float.Parse(s[mapper[3]]);

        //CsvParser.dictUpgrades.Add(s[0], this);
    }

    public static void Initialize(List<string> values)
    {
        mapper = new Dictionary<int, int>();
        for (int i = 1; i < values.Count; i++)
        {
            mapper[i] = columnToKey(values[i]);
        }
    }

    public static int columnToKey(string columnName)
    {
        columnName = columnName.Trim();
        return firstLine.IndexOf(columnName);
    }

}

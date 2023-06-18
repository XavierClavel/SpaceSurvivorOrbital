using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeData
{
    public List<Effect> effects = new List<Effect>();
    public int costGreen;
    public int costOrange;

    static Dictionary<int, effectType> dictColumnToEffect;
    public static void Initialize(List<string> columnNames)
    {
        dictColumnToEffect = new Dictionary<int, effectType>();
        for (int i = 3; i < columnNames.Count; i++)
        {
            dictColumnToEffect[i] = columnNameToEffect(columnNames[i]);
        }
    }

    public UpgradeData(List<string> s)
    {
        costGreen = int.Parse(s[1]);
        costOrange = int.Parse(s[2]);
        for (int i = 3; i < s.Count; i++)
        {
            if (s[i] == "") continue;
            Process(s[i], dictColumnToEffect[i]);
        }


        CsvParser.dictUpgrades.Add(s[0], this);
    }



    void Process(string value, effectType effect)
    {
        operationType operation;
        if (value.Last() == '%')
        {
            if (value.First() == '+') operation = operationType.multiply;
            else operation = operationType.divide;
            value.RemoveFirst();
            value.RemoveLast();
        }
        else
        {
            if (value.First() == '+')
            {
                operation = operationType.add;
                value.RemoveFirst();
            }
            else if (value.First() == '-')
            {
                operation = operationType.substract;
                value.RemoveFirst();
            }
            else operation = operationType.assignation;
        }

        effects.Add(new Effect(effect, operation, value));
    }


    static effectType columnNameToEffect(string columnName)
    {
        columnName = columnName.Trim();
        switch (columnName)
        {
            case "maxHealth":
                return effectType.maxHealth;

            case "baseSpeed":
                return effectType.baseSpeed;

            case "damageResistance":
                return effectType.damageResistanceMultiplier;

            case "maxViolet":
                return effectType.CHARACTERMaxViolet;

            case "maxOrange":
                return effectType.maxOrange;

            case "maxGreen":
                return effectType.maxGreen;

            case "baseDamage":
                return effectType.WEAPONBaseDamage;

            case "attackSpeed":
                return effectType.WEAPONAttackSpeed;



            default:
                throw new System.ArgumentException($"\"{columnName}\" column is not recognized");
        }
    }


}

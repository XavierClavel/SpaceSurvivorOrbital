using System.Collections.Generic;
using UnityEngine;

public static class ResurrectionManager
{
    public static ResurrectionSource fairy = new ResurrectionSource();
    public static ResurrectionSource peaceMaker = new ResurrectionSource();

    private static readonly List<ResurrectionSource> list = new List<ResurrectionSource>
    {
        fairy,
        peaceMaker
    };

    public static void reset()
    {
        list.ForEach(it => it.reset());
    }

    public static bool consumeResurrection()
    {
        foreach (var it in list)
        {
            if (it.spend()) return true;
        }
        return false;
    }

    public static int getAmount()
    {
        int total = 0;
        list.ForEach(it =>
            total += it.getRemaining());
        return total;
    }
}

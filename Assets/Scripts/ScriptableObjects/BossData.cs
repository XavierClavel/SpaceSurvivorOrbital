
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossData", menuName = Vault.other.scriptableObjectMenu + "BossData", order = 0)]
public class BossData : ObjectHandler
{
    public List<Ennemy> miniBosses;
    public List<Ennemy> bosses;
}

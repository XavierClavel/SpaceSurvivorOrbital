using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FightZone : MonoBehaviour
{
    List<Ennemy> ennemies;
    int nbEnnemies;

    void Start()
    {
        ennemies = GetComponentsInChildren<Ennemy>().ToList();
        nbEnnemies = ennemies.Count();
        foreach (Ennemy ennemy in ennemies) {
            ennemy.InitializeFZ(this);
        }
        if (nbEnnemies != 0) GameManagement.nbPlanetsWEnnemies ++;
    }

    private void OnTriggerEnter(Collider other) {
        if (!PlayerController.instance.shieldUp) GameManagement.instance.ShowCrosshair();
        foreach (Ennemy ennemy in ennemies) {
            ennemy.FollowPlayer();
        }
    }

    private void OnTriggerExit(Collider other) {
        GameManagement.instance.HideCrosshair();
        foreach (Ennemy ennemy in ennemies) {
            ennemy.StopFollowingPlayer();
        }
    }

    public void EnnemyKilled(Ennemy ennemy) 
    {
        ennemies.Remove(ennemy);
        Destroy(ennemy.gameObject);
        if (ennemies.Count() == 0) {
            PlanetCleared();
        }
    }

    void PlanetCleared() {
        Debug.Log("FightZone cleared !!!!");
        GameManagement.nbPlanetsWEnnemies --;
    }
}

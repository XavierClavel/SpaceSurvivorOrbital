using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Planet : MonoBehaviour
{
    [HideInInspector] public Vector3 position;
    [HideInInspector] public float gravityRadius;
    [HideInInspector] public float size;
    [HideInInspector] public float mass = 10;
    List<Ennemy> ennemies;
    int nbEnnemies;

    private void Awake() {
        position = transform.position;
        gravityRadius = GetComponent<SphereCollider>().radius;
        size = GetComponentInChildren<SphereCollider>().radius;
    }

    void Start()
    {
        ennemies = GetComponentsInChildren<Ennemy>().ToList();
        nbEnnemies = ennemies.Count();
        foreach (Ennemy ennemy in ennemies) {
            ennemy.Initialize(this);
        }
        if (nbEnnemies != 0) GameManagement.nbPlanetsWEnnemies ++;
    }

    private void OnTriggerEnter(Collider other) {
        if (!PlayerController.instance.shieldUp) GameManagement.instance.ShowCrosshair();
        foreach (Ennemy ennemy in ennemies) {
            ennemy.FollowPlayer();
            EnnemyIndicator.instance.StartIndicatingEnnemy(ennemy.gameObject, this);
        }
    }

    private void OnTriggerExit(Collider other) {
        GameManagement.instance.HideCrosshair();
        foreach (Ennemy ennemy in ennemies) {
            ennemy.StopFollowingPlayer();
            EnnemyIndicator.instance.StopIndicatingEnnemy(ennemy.gameObject);
        }
    }

    public void EnnemyKilled(Ennemy ennemy) 
    {
        ennemies.Remove(ennemy);
        EnnemyIndicator.instance.StopIndicatingEnnemy(ennemy.gameObject);
        if (ennemies.Count() == 0) {
            PlanetCleared();
        }
    }

    void PlanetCleared() {
        Debug.Log("Planet cleared !!!!");
        GameManagement.nbPlanetsWEnnemies --;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MyBox;
using DG.Tweening;

public enum actionType {Translate, Rotate, TranslateAndRotate, other}

public class Planet : MonoBehaviour
{
    [HideInInspector] public Vector3 position;
    [HideInInspector] public float gravityRadius;
    [HideInInspector] public float size;
    [HideInInspector] public float mass = 10;
    List<Ennemy> ennemies;
    int nbEnnemies;
    [System.Serializable]
    public class Event 
    {
        
        public actionType action;
        [ConditionalField(nameof(action), false, actionType.Rotate, actionType.Translate, actionType.TranslateAndRotate)] public Transform objectTransform;
        [ConditionalField(nameof(action), false, actionType.Translate, actionType.TranslateAndRotate)] public Vector3 finalPosition;
        [ConditionalField(nameof(action), false, actionType.Rotate, actionType.TranslateAndRotate)] public Vector3 finalRotation;
        [ConditionalField(nameof(action), false, actionType.Rotate, actionType.Translate, actionType.TranslateAndRotate)] public float duration;
        };
    public List<Event> events;

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
        Debug.Log("ennemy killed");
        ennemies.Remove(ennemy);
        EnnemyIndicator.instance.StopIndicatingEnnemy(ennemy.gameObject);
        if (ennemies.Count() == 0) {
            PlanetCleared();
        }
        Destroy(ennemy.gameObject);
    }

    void PlanetCleared() {
        foreach (Event eventAction in events) {
            if (eventAction.action == actionType.Translate || eventAction.action == actionType.TranslateAndRotate) {
                eventAction.objectTransform.DOMove(eventAction.finalPosition, eventAction.duration);
            }
            if (eventAction.action == actionType.Rotate || eventAction.action == actionType.TranslateAndRotate) {
                eventAction.objectTransform.DORotate(eventAction.finalRotation, eventAction.duration);
            }
        }
        GameManagement.nbPlanetsWEnnemies --;
    }
}

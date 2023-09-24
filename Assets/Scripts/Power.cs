using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Power : Damager
{
    [SerializeField] private bool activateOnStart;
    
    protected override void Start() {
        base.Start();

        if (activateOnStart) {
            Use();
        } else {
            StartCoroutine(nameof(Cooldown));
        }
    }



}


/*
public enum powerType { none, creusetout, divineLightning, toxicCloud };

powerType type;
    PlayerData data;
    GameObject powerPrefab;

    public Power(powerType type)
    {
        this.type = type;
        //get data from a csv
    }

    public void Instantiate()
    {

    }
    */
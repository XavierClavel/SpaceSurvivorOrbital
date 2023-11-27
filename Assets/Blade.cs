using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    [SerializeField] private Sword sword;
    private void OnTriggerEnter2D(Collider2D other)
    {
        sword.onHit(other);
    }
}

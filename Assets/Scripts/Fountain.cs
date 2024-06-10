using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fountain : MonoBehaviour
{
    [SerializeField] ParticleSystem auraFountain;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ObjectManager.fountain = this;
            ObjectManager.DisplayFountainUI();
        }

    }
    public void DepleteFountain()
    {
        animator.enabled = true;
        GetComponent<Collider2D>().enabled = false;
        auraFountain.Stop();
    }

}

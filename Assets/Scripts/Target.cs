using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    enum color { blue, orange, red }
    [SerializeField] color targetState;
    [SerializeField] ParticleSystem explosion;
    Runner parentEnnemy;


    void Start()
    {
        parentEnnemy = GetComponentInParent<Runner>();
    }

    private void OnCollisionEnter(Collision other)
    {
        switch (targetState)
        {

            case color.blue:
                if (!other.gameObject.CompareTag("OrangeBullet"))
                {
                    Destruct();
                }
                break;

            case color.orange:
                if (!other.gameObject.CompareTag("BlueBullet"))
                {
                    Destruct();
                }
                break;

            case color.red:
                if (other.gameObject.CompareTag("RedBullet"))
                {
                    Destruct();
                }
                break;

        }
    }

    void Destruct()
    {
        ParticleSystem parSys = Instantiate(explosion, transform.position, Quaternion.LookRotation(transform.forward, transform.up));
        parSys.Play();
        Helpers.instance.WaitAndKill(0.5f, parSys.gameObject);
        Destroy(gameObject);
    }
}

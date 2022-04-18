using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody rb;
    [HideInInspector] public Vector3 planetPos;
    [SerializeField] ParticleSystem bulletParticle;
    public Vector3 axis;
    public float radius;
    public float rotationSpeed = 80.0f; 
    [HideInInspector] public bool inGravityField;
    float speed = 20f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (!inGravityField) rb.velocity = speed * transform.forward;
        else StartCoroutine(Orbit());
        StartCoroutine(DestroyTimer());
        
    }

    void OnCollisionEnter(Collision other) {
        ParticleSystem parSys = Instantiate(bulletParticle, transform.position, Quaternion.LookRotation(other.GetContact(0).normal, transform.up));
        parSys.Play();
        Helpers.instance.WaitAndKill(0.5f, parSys.gameObject);
        Destroy(gameObject);
    }

    IEnumerator Orbit()
    {
        WaitForEndOfFrame waitFrame = Helpers.GetWaitFrame;
        while (true)
        {
            transform.RotateAround(planetPos, axis, rotationSpeed * Time.deltaTime);
            yield return waitFrame;
        }
    }

    IEnumerator DestroyTimer()
    {
        yield return Helpers.GetWait(10f);
        Destroy(gameObject);
    }
}

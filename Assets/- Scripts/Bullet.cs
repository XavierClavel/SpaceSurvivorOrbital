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
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (!inGravityField) rb.velocity = speed * transform.forward;
    }

    private void Update() {
        if (inGravityField) transform.RotateAround(planetPos, axis, rotationSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other) {
        ParticleSystem parSys = Instantiate(bulletParticle, other.transform.position, Quaternion.LookRotation(other.GetContact(0).normal, transform.up));
        parSys.Play();
        Helpers.instance.WaitAndKill(0.5f, parSys.gameObject);
        Destroy(gameObject);
    }
}

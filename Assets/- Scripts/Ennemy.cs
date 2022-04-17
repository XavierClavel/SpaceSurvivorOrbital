using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class Ennemy : MonoBehaviour
{

    [SerializeField] GameObject bulletPrefab; 
    PlayerController player;
    Planet planet;
    Vector3 planetPos;
    float radius;
    [HideInInspector] public int nbTargets;
    bool inGravityField;
    [SerializeField] float mean = 4f;
    [SerializeField] float standardDeviation = 1f;
    bool hasProtection = false;
    FightZone fightZone;


    void Start()
    {
        List<Target> targets = GetComponentsInChildren<Target>().ToList();
        nbTargets = targets.Count();
        if (nbTargets > 0) hasProtection = true;
        player = PlayerController.instance;
    }

    IEnumerator Follow()
    {
        WaitForSeconds waitFixedDelta = Helpers.GetWait(Time.fixedDeltaTime);
        while (true) {
            Vector3 distance = player.transform.position - transform.position;
            Vector3 projectedDistance = Vector3.ProjectOnPlane(distance, transform.up);
            transform.DOLocalRotate(Quaternion.LookRotation(projectedDistance, transform.up).eulerAngles, Time.fixedDeltaTime);
            yield return waitFixedDelta;
        }
    }

    void Shoot()
    {
        Bullet bullet = Instantiate(bulletPrefab, transform.position + transform.forward, transform.rotation).GetComponentInChildren<Bullet>();
        bullet.axis = transform.right;
        bullet.planetPos = planetPos;
        bullet.radius = radius;
        bullet.inGravityField = inGravityField;
    }

    public void Initialize(Planet basePlanet) {
        planet = basePlanet;
        planetPos = planet.position;
        radius = (planetPos - transform.position).magnitude;
        inGravityField = true;
    }

    public void InitializeFZ(FightZone baseFightZone) {
        inGravityField = false;
        mean = 1.5f;
        standardDeviation = 0.5f;
        fightZone = baseFightZone;
    }

    IEnumerator WaitAndShoot() {
        while (true) {
            yield return new WaitForSeconds(mean + Random.Range(- standardDeviation, standardDeviation));
            Shoot();
        }
    }

    public void FollowPlayer() 
    {
        StartCoroutine("Follow");
        StartCoroutine("WaitAndShoot");
    }

    public void StopFollowingPlayer() 
    {
        StopAllCoroutines();
    }

    public void TargetHit() {
        nbTargets --;
        if (nbTargets == 0) hasProtection = false;
    }

    private void OnCollisionEnter(Collision other) {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.CompareTag("BlueBullet") && !hasProtection) {
            Death();
        }
    }

    void Death() {
        Debug.Log("death");
        if (inGravityField) planet.EnnemyKilled(this);
        else fightZone.EnnemyKilled(this);
    }

    
}

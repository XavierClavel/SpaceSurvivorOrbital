using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SphereBoss : MonoBehaviour
{
    enum bossState {following, erratic, shooting, followingLaser, transition}
    bossState state = bossState.following;
    bossState newState;
    Transform playerTransform;
    Vector3 distance;
    LineRenderer laser;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] GameObject redLight;
    [SerializeField] GameObject warmingParSys;
    [SerializeField] Material laserGlow;
    [SerializeField] GameObject groundImpactParSys;
    RaycastHit hit;
    bool laserActive = false;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = PlayerController.instance.gameObject.transform;
        laser = GetComponentInChildren<LineRenderer>();
        laser.gameObject.SetActive(false);
    }

    private void Update() {
        //if (Input.GetKeyDown(KeyCode.L)) {
          //  StartCoroutine(LaserWarmUp());
        //}
    }

    void FixedUpdate()
    {
        switch (state) {

            case bossState.following :
            Follow();
            break;

            case bossState.followingLaser :
            Follow();
            UpdateLaser();
            break;

            case bossState.shooting :
            Follow();
            break;

            case bossState.erratic :
            break;
            
            case bossState.transition :
            break;

        }
    }

    IEnumerator TransitionState()
    {
        while (true) {
            bossState newState = bossState.transition;

            switch (state) {

                case bossState.followingLaser :
                DeactivateLaser();
                break;

                case bossState.erratic :
                DeactivateLaser();
                break;
            }

            // buffer time here ?

            switch (newState) {
                case bossState.followingLaser :
                StartCoroutine(LaserWarmUp());
                break;

                case bossState.erratic :
                StartCoroutine(LaserWarmUp());
                break;
            }

            yield return Helpers.GetWait(20f);

        }
    }

    void Follow()
    {
        distance = playerTransform.position - transform.position;
        transform.DOLocalRotate(Quaternion.LookRotation(distance, transform.up).eulerAngles, Time.fixedDeltaTime);
    }

    void UpdateLaser()
    {
        Vector3 impactPos = CastRay();
        laser.SetPosition(1, impactPos);
        groundImpactParSys.transform.position = impactPos;
        groundImpactParSys.transform.rotation = Quaternion.LookRotation(hit.normal);
    }

    Vector3 CastRay()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, 50f, ~ignoreLayer);
        return hit.point;
    }

    void DeactivateLaser()
    {
        laser.gameObject.SetActive(false);
    }

    IEnumerator LaserWarmUp()
    {
        redLight.SetActive(true);
        warmingParSys.SetActive(true);
        yield return Helpers.GetWait(2f);
        
        redLight.SetActive(false);
        StartCoroutine(ActivateLaser());
    }

    IEnumerator ActivateLaser()
    {
        laser.gameObject.SetActive(true);
        laser.SetPosition(1, transform.position);

        float duration = 0.5f;
        float invDuration = 1f/duration;
        float ratio = 0f;
        float startTime = Time.time;
        WaitForEndOfFrame waitFrame = Helpers.GetWaitFrame;

        while (ratio < 1f) {
            ratio = (Time.time - startTime) * invDuration;
            laser.SetPosition(1, transform.position + ratio * distance);
            yield return waitFrame;
        }

        groundImpactParSys.SetActive(true);
        state = newState;   //transition to state with laser active is over
    }

}

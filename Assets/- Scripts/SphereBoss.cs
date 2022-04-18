using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class SphereBoss : MonoBehaviour
{
    public enum bossState {following, erratic, shooting, followingLaser, transition, laserAxis}
    public bossState state = bossState.laserAxis;
    bossState newState;
    Transform playerTransform;
    Vector3 distance;
    LineRenderer laser;
    [SerializeField] LayerMask raycastLayer;
    [SerializeField] GameObject redLight;
    [SerializeField] GameObject warmingParSys;
    [SerializeField] Material laserGlow;
    [SerializeField] GameObject groundImpactParSys;
    [SerializeField] List<AnimationCurve> curveList;
    AnimationCurve curveX;
    AnimationCurve curveY;
    AnimationCurve curveZ;
    RaycastHit hit;
    Rigidbody rb;
    bool laserActive = false;
    bool laserTouchesGround = false;
    Vector3 axis;
    float laserInitialWidth = 0.75f;
    float laserFinalWidth = 0.3f;
    bool previousGround = true;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletPrefab;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerTransform = PlayerController.instance.gameObject.transform;
        laser = GetComponentInChildren<LineRenderer>();
        laser.gameObject.SetActive(false);
        StartCoroutine(TransitionState());
    }


    IEnumerator TransitionState()
    {
        List<bossState> potentialNewStates = Enum.GetValues(typeof(bossState)).Cast<bossState>().ToList();
        //potentialNewStates.Remove(state);
        //state = potentialNewStates[UnityEngine.Random.Range(0, potentialNewStates.Count)];

        // buffer time here ?

        switch (state) {
            
            case bossState.followingLaser :
            rb.DORotate(Quaternion.LookRotation(playerTransform.position - transform.position).eulerAngles, 10f);
            yield return Helpers.GetWait(10f);

            StartCoroutine(ActivateLaser());
            break;

            case bossState.erratic :
            StartCoroutine(ActivateLaser());
            break;

            case bossState.laserAxis : 
            axis = PlayerController.instance.pathTransform.forward;
            transform.DOLookAt(transform.position + PlayerController.instance.pathTransform.up, 2f);
            yield return Helpers.GetWait(2f);

            StartCoroutine(ActivateLaser());
            break;

            case bossState.following :
            rb.DORotate(Quaternion.LookRotation(playerTransform.position - transform.position).eulerAngles, 10f);
            yield return Helpers.GetWait(10f);
            break;

            case bossState.shooting :
            rb.DORotate(Quaternion.LookRotation(playerTransform.position - transform.position).eulerAngles, 10f);
            yield return Helpers.GetWait(10f);
            StartCoroutine("WaitAndShoot");
            StartCoroutine("Follow");
            break;
        }
        
        yield return null;
    }

    IEnumerator WaitAndShoot()
    {
        GameManagement.instance.ShowCrosshair();
        WaitForSeconds waitWindow = Helpers.GetWait(1f);
        for (int i = 0; i <20; i++) {
            yield return waitWindow;
            Shoot();
        }
        StartCoroutine(TransitionState());
        GameManagement.instance.ShowCrosshair();
    }

    IEnumerator Follow()
    {
        while(true) {

        float duration = 0.5f;
        float invDuration = 1f/duration;
        WaitForSeconds waitWindow = Helpers.GetWait(duration);

        Vector3 finalPos = Quaternion.LookRotation(playerTransform.position).eulerAngles; //need to specify upward
        transform.DOLocalRotate(finalPos, duration, RotateMode.Fast).SetEase(Ease.Linear);

        yield return waitWindow;
        }

    }

    void Shoot()
    {
        Bullet bullet = Instantiate(bulletPrefab, firePoint.position, transform.rotation).GetComponentInChildren<Bullet>();
        bullet.inGravityField = false;
    }

    void RandomizeCurves()
    {
        int maxIndex = curveList.Count;
        curveX = curveList[UnityEngine.Random.Range(0, maxIndex)];
        curveY = curveList[UnityEngine.Random.Range(0, maxIndex)];
        curveZ= curveList[UnityEngine.Random.Range(0, maxIndex)];
    }

    IEnumerator LaserUpdate()
    {
        WaitForEndOfFrame waitFrame = Helpers.GetWaitFrame;
        while (true) {
            UpdateLaser();
            yield return waitFrame;
        }
    }

    void UpdateLaser()
    {
        Vector3 impactPos = CastRay();
        laser.SetPosition(1, impactPos);
        if (laserTouchesGround != previousGround) {
            groundImpactParSys.SetActive(laserTouchesGround);
            redLight.SetActive(laserTouchesGround);
        }
        if (laserTouchesGround) {
            groundImpactParSys.transform.position = impactPos;
            groundImpactParSys.transform.rotation = Quaternion.LookRotation(hit.normal);
            redLight.transform.position = impactPos - transform.forward * 0.5f;
        }
        previousGround = laserTouchesGround;
    }

    IEnumerator FollowLaser()
    {
        StartCoroutine("LaserUpdate");
        float duration = 20f;
        float timeStep = 0.2f;
        WaitForSeconds waitForSeconds = Helpers.GetWait(timeStep);
        float startTime = Time.time;
        while (Time.time - startTime < duration) {
            transform.DORotateQuaternion(Quaternion.LookRotation(playerTransform.position - transform.position), timeStep).SetEase(Ease.Linear);
            yield return waitForSeconds;
        }
        DeactivateLaser();

    }

    IEnumerator ErraticMovement()
    {
        RandomizeCurves();
        StartCoroutine("LaserUpdate");
        float duration = 5f;
        WaitForSeconds waitForSeconds = Helpers.GetWait(0.1f);
        float startTime = Time.time;
        float invDuration = 1f/duration;
        float ratio = 0f;
        while (ratio < 1f) {
            ratio = (Time.time - startTime) * invDuration;
            rb.angularVelocity = 10f *  new Vector3(curveX.Evaluate(ratio), curveY.Evaluate(ratio), 0f);
            yield return waitForSeconds;
        }
        rb.angularVelocity = Vector3.zero;
        DeactivateLaser();

    }

    IEnumerator LaserAxis()
    {
        
        StartCoroutine("LaserUpdate");
        transform.DORotate(360f * axis, 5f, RotateMode.WorldAxisAdd);
        yield return Helpers.GetWait(5f);
        rb.angularVelocity = Vector3.zero;
        DeactivateLaser();

    }

    Vector3 CastRay()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        laserTouchesGround = Physics.Raycast(ray, out hit, 50f, raycastLayer);
        return laserTouchesGround ? hit.point : transform.position + transform.forward * 100f;
    }

    void DeactivateLaser()
    {
        warmingParSys.SetActive(false);
        groundImpactParSys.SetActive(false);

        StopCoroutine("LaserUpdate");
        StartCoroutine(LaserCoolDown());
    }

    IEnumerator LaserCoolDown()
    {
        //Tweening for animating the laser fading out
        redLight.transform.DOScale(Vector3.zero, 0.5f);
        DOTween.To(() => laser.startWidth, x => laser.startWidth = x, 0f, 0.5f);
        DOTween.To(() => laser.endWidth, x => laser.endWidth = x, 0f, 0.5f);

        //Wait until tweening if over
        yield return Helpers.GetWait(0.6f);

        //Deactivate the objects
        laser.gameObject.SetActive(false);
        redLight.SetActive(false);

        //Set back their original values
        laser.startWidth = laserInitialWidth;
        laser.endWidth = laserFinalWidth;
        redLight.transform.localScale = Vector3.one;

        //End state and start transition
        StartCoroutine(TransitionState());
    }

    IEnumerator ActivateLaser()
    {
        redLight.SetActive(true);
        warmingParSys.SetActive(true);
        redLight.transform.position = transform.forward *5f;
        yield return Helpers.GetWait(2f);

        laser.gameObject.SetActive(true);
        laser.SetPosition(1, transform.position);

        float duration = 0.5f;
        float invDuration = 1f/duration;
        float ratio = 0f;
        float startTime = Time.time;
        WaitForEndOfFrame waitFrame = Helpers.GetWaitFrame;
        distance = CastRay();

        while (ratio < 1f) {
            ratio = (Time.time - startTime) * invDuration;
            laser.SetPosition(1, transform.position + ratio * distance);
            redLight.transform.position = ratio * distance;
            yield return waitFrame;
        }

        redLight.SetActive(laserTouchesGround);
        groundImpactParSys.SetActive(laserTouchesGround);

        switch (state) {
            case bossState.erratic :
                StartCoroutine(ErraticMovement());
                break;

            case bossState.followingLaser :
                StartCoroutine(FollowLaser());
                break;

            case bossState.laserAxis :
                StartCoroutine(LaserAxis());
                break;
        }
    }

}

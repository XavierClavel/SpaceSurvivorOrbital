using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    Transform playerTransform;
    int planetSize;
    Vector3 localPosition;
    [SerializeField] Transform xRotator;
    [SerializeField] Transform yRotator;
    [SerializeField] Transform playerProjector;
    float radarRadius = 1.5f;
    float planetRadius;
    float latitude;
    float longitude;
    public bool isActive = false;

    private void Start()
    {
        if (!isActive) { this.gameObject.SetActive(false); }
        else if (isActive) { this.gameObject.SetActive(true); }

        playerTransform = PlayerController.instance.transform;
        planetSize = (PlanetManager.hasData() ? PlanetManager.getSize() : 9) * TileManager.instance.tileSize.x;
        planetRadius = (float)planetSize * 0.5f;
    }

    private void Update()
    {
        //localPosition = playerTransform.position.mod(planetSize);
        //transform.eulerAngles = new Vector3(getXRotation(), 0f, getZRotation());
        //Debug.Log(getYPosition());
        //playerProjector.updateY(getYPosition());
        playerProjector.LookAt(transform, transform.up);

        Vector3 cartesian = new Vector3(playerTransform.position.x, 0, playerTransform.position.y);
        //Vector3 spherical = ToSpherical(cartesian) / (float)planetSize * 360f;
        //Vector3 offset = new Vector3(90f, 0f, 29f);

        Vector3 spherical = cartesian * 360f / planetSize;
        Debug.Log(spherical);

        longitude = playerTransform.position.x * 360f / (float)planetSize;
        //var longitude = Mathf.Rad2Deg * Mathf.Atan2(playerTransform.position.y, playerTransform.position.x); // radians
        latitude = ((playerTransform.position.y + planetRadius).mod(planetSize) - planetRadius + planetRadius) * 180f / planetSize;
        latitude = Mathf.Cos(Mathf.Deg2Rad * latitude) * 90f;
        Debug.Log(latitude);

        //var q = latitude * longitude;
        //xRotator.eulerAngles = latitude * Vector3.right;
        //yRotator.localEulerAngles = longitude * Vector3.up;
        transform.eulerAngles = new Vector3(latitude, longitude);
        //transform.eulerAngles = new Vector3(spherical.z, spherical.x, 0f);
        //Debug.Log(transform.eulerAngles);
        //Debug.Log(localPosition);
    }

    float getYPosition()
    {
        return (playerTransform.position.y.mod(planetSize) - (float)planetSize * 0.5f) * radarRadius / planetRadius;
    }

    float getXRotation()
    {
        latitude = ((playerTransform.position.y + planetRadius).mod(planetSize) - planetRadius) * 180f / planetSize;
        //Debug.Log(playerTransform.position.y);
        //Debug.Log(latitude);
        //Debug.Log(Mathf.Cos(Mathf.Deg2Rad * latitude));
        float angle = Mathf.Clamp(latitude, -60f, 60f);
        return 90f - angle;
        //return Mathf.Clamp(playerTransform.position.y)
    }

    float getZRotation()
    {
        return -29f - (playerTransform.position.x * 360f / (float)planetSize); //* Mathf.Cos(Mathf.Deg2Rad * latitude);
    }


    Vector3 ToSpherical(Vector3 aCartesian)
    {
        return ToSpherical(aCartesian, Quaternion.identity);
    }
    Vector3 ToSpherical(Vector3 aCartesian, Quaternion aSpace)
    {
        var q = Quaternion.Inverse(aSpace);
        var len = aCartesian.magnitude;
        var v = q * aCartesian / len;
        Vector3 result;
        result.x = Mathf.Atan2(v.z, v.x);
        result.y = Mathf.Asin(v.y);
        result.z = len;
        return result;
    }
    Vector3 ToCartesian(Vector3 aSpherical)
    {
        return ToCartesian(aSpherical, Quaternion.identity);
    }
    Vector3 ToCartesian(Vector3 aSpherical, Quaternion aSpace)
    {
        Vector3 result;
        float c = Mathf.Cos(aSpherical.y);
        result.x = Mathf.Cos(aSpherical.x) * c;
        result.y = Mathf.Sin(aSpherical.y);
        result.z = Mathf.Sin(aSpherical.x) * c;
        result *= aSpherical.z;
        result = aSpace * result;
        return result;
    }
}

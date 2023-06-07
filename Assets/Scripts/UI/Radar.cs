using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    Transform playerTransform;
    int planetCircumference;
    Vector3 localPosition;
    [SerializeField] Transform xRotator;
    [SerializeField] Transform yRotator;
    [SerializeField] Transform playerProjector;
    float radarRadius = 1.5f;
    float planetRadius;
    float latitude;
    float longitude;

    float scaleFactor;
    float scaleFactorRad;

    private void Start()
    {
        playerTransform = PlayerController.instance.transform;
        planetCircumference = (PlanetManager.hasData() ? PlanetManager.getSize() : 9) * TileManager.instance.tileSize.x;
        planetRadius = (float)planetCircumference * 0.5f;
        scaleFactor = 360f / (float)planetCircumference;
        scaleFactorRad = Mathf.Deg2Rad * scaleFactor;
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

        Vector3 spherical = cartesian * 360f / planetCircumference;
        //Debug.Log(spherical);

        float y = (playerTransform.position.y + planetRadius).mod(planetCircumference) - planetRadius;
        //Debug.Log(y);
        longitude = playerTransform.position.x * scaleFactor;
        //var longitude = Mathf.Rad2Deg * Mathf.Atan2(playerTransform.position.y, playerTransform.position.x); // radians
        //latitude = 0.5f * ((playerTransform.position.y + planetRadius).mod(planetCircumference) - planetRadius) * 180f / planetCircumference;
        latitude = -playerTransform.position.y * scaleFactor;
        //latitude = -Mathf.Sin(Mathf.Deg2Rad * latitude * 0.50f) * 90f;


        //latitude = Mathf.Rad2Deg * 2 * Mathf.Atan(Mathf.Exp(Mathf.Deg2Rad * y * scaleFactor)) - 90f;
        //latitude = gudermannianDeg(y);
        //latitude *= -1f;

        //Debug.Log(gudermannianDeg(y));
        //Debug.Log(latitude);
        //float clampedLatitude = -Mathf.Clamp(latitude.mod(360), -60f, 60f);

        //var q = latitude * longitude;
        //if (Mathf.Abs(latitude) > 90) longitude += 180f;
        xRotator.eulerAngles = latitude * Vector3.right;
        yRotator.localEulerAngles = longitude * Vector3.up;

        //transform.eulerAngles = new Vector3(latitude, longitude);
        //transform.eulerAngles = new Vector3(spherical.z, spherical.x, 0f);
        //Debug.Log(transform.eulerAngles);
        //Debug.Log(localPosition);
    }
    /**
    <summary>Returns the latitude in radians corresponding to the given height</summary>
    **/
    float gudermannianRad(float value)
    {
        return (float)Mathf.Atan(Helpers.Sinh(value * scaleFactorRad));
    }

    /**
    <summary>Returns the latitude in degrees corresponding to the given height</summary>
    **/
    float gudermannianDeg(float value)
    {
        return Mathf.Rad2Deg * Mathf.Log(Mathf.Tan((Mathf.PI / 4) + (value * scaleFactorRad / 2)), Mathf.Exp(1));
        //return Mathf.Rad2Deg * (float)Mathf.Atan(Helpers.Sinh(value * scaleFactorRad));

    }


    float getYPosition()
    {
        return (playerTransform.position.y.mod(planetCircumference) - (float)planetCircumference * 0.5f) * radarRadius / planetRadius;
    }

    float getXRotation()
    {
        latitude = ((playerTransform.position.y + planetRadius).mod(planetCircumference) - planetRadius) * 180f / planetCircumference;
        //Debug.Log(playerTransform.position.y);
        //Debug.Log(latitude);
        //Debug.Log(Mathf.Cos(Mathf.Deg2Rad * latitude));
        float angle = Mathf.Clamp(latitude, -60f, 60f);
        return 90f - angle;
        //return Mathf.Clamp(playerTransform.position.y)
    }

    float getZRotation()
    {
        return -29f - (playerTransform.position.x * 360f / (float)planetCircumference); //* Mathf.Cos(Mathf.Deg2Rad * latitude);
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

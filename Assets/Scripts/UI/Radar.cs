using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    Transform playerTransform;
    int planetSize;
    Vector3 localPosition;
    private void Start()
    {
        playerTransform = PlayerController.instance.transform;
        planetSize = (PlanetManager.hasData() ? PlanetManager.getSize() : 9) * TileManager.instance.tileSize.x;
    }

    private void Update()
    {
        //localPosition = playerTransform.position.mod(planetSize);
        transform.eulerAngles = new Vector3(90f, getYRotation());
        //Debug.Log(localPosition);
    }

    float getYRotation()
    {
        return playerTransform.position.x * 360f / (float)planetSize;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    protected Transform cameraTransform;
    private Vector3 originalCameraPosition;
    public Camera mainCamera;

    [Header("EnnemyShake")]
    public float shakeDuration = 0.1f;
    public float shakeIntensity = 1f;
    public float negativeRange = -0.1f;
    public float positiveRange = 0.1f;

    public void Update()
    {
        cameraTransform = mainCamera.transform;
        originalCameraPosition = cameraTransform.localPosition;
    }

    public IEnumerator ShakeCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            float randomX = Random.Range(negativeRange, positiveRange) * shakeIntensity;
            float randomY = Random.Range(negativeRange, positiveRange) * shakeIntensity;
            Vector3 randomPoint = originalCameraPosition + new Vector3(randomX, randomY, 0f);
            cameraTransform.localPosition = randomPoint;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        cameraTransform.localPosition = originalCameraPosition;

    }
}

using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ShakeManager : MonoBehaviour
{
    private static ShakeManager instance;
    private CinemachineVirtualCamera cinemachineCamera;
    private CinemachineBasicMultiChannelPerlin camNoise;
    private float shakeDuration = 0.1f;
    private float shakeIntensity = 0.1f;
    private bool isShaking = false;
    private bool overrideShake = false;
    private bool hasCinemarchineCamera;
    public void Setup()
    {
        instance = this;
        try
        {
            cinemachineCamera = Camera.main.gameObject.GetComponent<CinemachineVirtualCamera>();
            camNoise = cinemachineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            hasCinemarchineCamera = true;
        }
        catch
        {
            hasCinemarchineCamera = false;
        }
        
    }
    
    

    public static void StartShake(float shakeIntensity)
    {
        if (!instance.hasCinemarchineCamera) return;
        instance.StopCoroutine(nameof(ShakeCoroutine));
        instance.overrideShake = true;
        instance.camNoise.m_AmplitudeGain = shakeIntensity;
    }

    public static void StopShake()
    {
        if (!instance.hasCinemarchineCamera) return;
        if (!instance.overrideShake) return;
        instance.overrideShake = false;
        instance.camNoise.m_AmplitudeGain = 0f;
    }

    public static void Shake(float shakeIntensity, float shakeDuration)
    {
        if (!instance.hasCinemarchineCamera) return;
        if (instance.overrideShake) return;
        if (instance.isShaking &&  instance.shakeIntensity >= shakeIntensity)
        {
            return;
        }
        instance.shakeIntensity = shakeIntensity;
        instance.shakeDuration = shakeDuration;
        instance.StopCoroutine(nameof(ShakeCoroutine));
        instance.StartCoroutine(nameof(ShakeCoroutine));
    }

    private IEnumerator ShakeCoroutine()
    {
        isShaking = true;
        camNoise.m_AmplitudeGain = shakeIntensity;
        yield return Helpers.getWait(shakeDuration);
        camNoise.m_AmplitudeGain = 0f;
        isShaking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (cinemachineCamera == null) return;
        cinemachineCamera.transform.rotation = Quaternion.identity;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Helpers : MonoBehaviour
{
    static readonly Dictionary<float, WaitForSeconds> waitDictionary = new Dictionary<float, WaitForSeconds>();
    static readonly Dictionary<float, WaitForSecondsRealtime> waitDictionaryRealtime = new Dictionary<float, WaitForSecondsRealtime>();
    public static readonly WaitForEndOfFrame GetWaitFrame = new WaitForEndOfFrame();
    public static Helpers instance;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    public static WaitForSeconds GetWait(float time)
    {
        if (waitDictionary.TryGetValue(time, out WaitForSeconds wait)) return wait;
        waitDictionary[time] = new WaitForSeconds(time);
        return waitDictionary[time];
    }

    public static WaitForSecondsRealtime GetWaitRealtime(float time)
    {
        if (waitDictionaryRealtime.TryGetValue(time, out WaitForSecondsRealtime wait)) return wait;
        waitDictionaryRealtime[time] = new WaitForSecondsRealtime(time);
        return waitDictionaryRealtime[time];
    }

    public void WaitAndKill(float time, GameObject objectToDestroy) {
        StartCoroutine(WaitThenKill(time, objectToDestroy));
    }

    IEnumerator WaitThenKill(float time, GameObject objectToDestroy) {
        yield return GetWait(time);
        Destroy(objectToDestroy);
    }
    IEnumerator test()
    {
        yield return null;
    }

    public void LerpQuaternion(Transform objectTransform, Quaternion initialPos, Quaternion finalPos, float duration) {
        StartCoroutine(LerpQuaternionCoroutine(objectTransform, initialPos, finalPos, duration));
    }

    IEnumerator LerpQuaternionCoroutine(Transform objectTransform, Quaternion initialPos, Quaternion finalPos, float duration) {
        float invDuration = 1f/duration;
        float ratio = 0f;
        float startTime = Time.time;
        while (ratio < 1f) {
            ratio = (Time.time - startTime) * invDuration;
            objectTransform.rotation = Quaternion.Slerp(initialPos, finalPos, ratio);
            yield return GetWaitFrame;
        }
    }

}

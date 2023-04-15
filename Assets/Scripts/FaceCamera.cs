using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    [SerializeField]
    Transform spriteTransform;
    [SerializeField] Canvas canvas;

    //cached values
    Transform cameraTransform;
    PlayerController player;
    Vector3 distance;
    Vector3 up;
    Vector3 correctedUp;
    Vector3 projectedDistance;
    float dotProduct;
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerController.instance;
        cameraTransform = Camera.main.transform;
        canvas.worldCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        distance = player.transform.position - transform.position;
        up = transform.position.normalized;
        projectedDistance = Vector3.ProjectOnPlane(distance, up).normalized;
        correctedUp = up;
        dotProduct = Mathf.Clamp01(Vector3.Dot(projectedDistance, player.localTransform.forward));


        transform.rotation = Quaternion.LookRotation(projectedDistance, up);
        spriteTransform.LookAt(cameraTransform.position - cameraTransform.up * dotProduct * 20f -
        cameraTransform.forward * dotProduct * 20f
        , correctedUp);

        //spriteTransform.LookAt(cameraTransform, correctedUp);
    }
}

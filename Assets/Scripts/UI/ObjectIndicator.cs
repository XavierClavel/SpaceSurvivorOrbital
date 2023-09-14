using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectIndicator : MonoBehaviour
{
    [HideInInspector] public Transform target;
    [SerializeField] Image image;


    float posx;
    float signx;

    float posy;
    float signy;

    int halfScreenx;
    int halfScreeny;
    RectTransform rectTransform;
    new Camera camera;

    private void Awake()
    {
        ObjectManager.spaceshipIndicator = this;
        gameObject.SetActive(false);
    }

    private void Start()
    {
        halfScreenx = (int)(Camera.main.pixelWidth * 0.5);
        halfScreeny = (int)(Camera.main.pixelHeight * 0.5);
        rectTransform = GetComponent<RectTransform>();

        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPos = camera.WorldToScreenPoint(target.position);
        Vector2 direction = screenPos - new Vector3(halfScreenx, halfScreeny, 0);
        float angle = Vector2.SignedAngle(Vector2.up, direction);
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        if (Mathf.Abs(direction.x) < halfScreenx && Mathf.Abs(direction.y) < halfScreeny)
        {
            image.enabled = false;
            return;
        }
        image.enabled = true;

        posx = Mathf.Clamp(direction.x, -900, 900);
        posy = Mathf.Clamp(direction.y, -480, 480);

        //TODO : check behavior on larger resolutions

        rectTransform.anchoredPosition = new Vector2(posx, posy);
    }
}

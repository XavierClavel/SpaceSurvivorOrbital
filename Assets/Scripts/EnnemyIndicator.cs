using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyIndicator : MonoBehaviour
{
    Vector3 targetPosOnScreen;
    [SerializeField] GameObject indicatorPrefabClosest;
    [SerializeField] GameObject indicatorPrefabFarthest;
    public static EnnemyIndicator instance;
    [SerializeField] Transform t_player;
    static Coroutine coroutine;
    static Dictionary<GameObject, Coroutine> dictionnaryObjectToCoroutine;
    static Dictionary<GameObject, GameObject> dictionnaryObjectToIndicatorClosest;
    static Dictionary<GameObject, GameObject> dictionnaryObjectToIndicatorFarthest;
    Planet planet;
    float ennemyOffset = 0.5f;
    public LayerMask ignoredLayer;

    Vector3 camPlanetAxis;
    float camDistance;
    float alpha;
    Vector3 pointOnCamPlanetAxisWhereEnemyAxisCrossesOrthogonally;
    float horizonHeight;
    Vector3 enemyAxisOriginInWorldCoordinates;
    Vector3 orthogonalEnemyAxis;
    Vector3 sayHello;
    Vector2 posOnScreen;
    Vector2 screenOffset = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
    [SerializeField] Canvas canvas;
    float scaleFactor;
    float angle;
    Transform playerTransform;
    Vector2 pixelLocation;
    Vector2 dir;
    float teta;


    void Awake()
    {
        instance = this;
        dictionnaryObjectToIndicatorClosest = new Dictionary<GameObject, GameObject>();
        dictionnaryObjectToIndicatorFarthest = new Dictionary<GameObject, GameObject>();
        dictionnaryObjectToCoroutine = new Dictionary<GameObject, Coroutine>();
        scaleFactor = 1f/canvas.scaleFactor;
    }

    private void Start() {
        playerTransform = PlayerController.instance.gameObject.transform;
    }

    public void StartIndicatingEnnemy(GameObject ennemy, Planet localPlanet) {
        planet = localPlanet;

        dictionnaryObjectToIndicatorClosest[ennemy] = Instantiate(indicatorPrefabClosest);
        GameObject indicatorClosest = dictionnaryObjectToIndicatorClosest[ennemy];
        indicatorClosest.transform.SetParent(canvas.transform);

        RectTransform prefabTransform = indicatorPrefabClosest.GetComponent<RectTransform>();
        RectTransform objectTransform = indicatorClosest.GetComponent<RectTransform>();
        objectTransform.position = prefabTransform.position;
        objectTransform.localScale = prefabTransform.localScale;

        dictionnaryObjectToIndicatorFarthest[ennemy] = Instantiate(indicatorPrefabFarthest);
        GameObject indicatorFarthest = dictionnaryObjectToIndicatorFarthest[ennemy];
        indicatorFarthest.transform.SetParent(canvas.transform);

        prefabTransform = indicatorPrefabFarthest.GetComponent<RectTransform>();
        objectTransform = indicatorFarthest.GetComponent<RectTransform>();
        objectTransform.position = prefabTransform.position;
        objectTransform.localScale = prefabTransform.localScale;

        coroutine = instance.StartCoroutine("IndicateEnnemy", ennemy);
        dictionnaryObjectToCoroutine[ennemy] = coroutine;
    }

    public void StopIndicatingEnnemy(GameObject ennemy) {
        coroutine = dictionnaryObjectToCoroutine[ennemy];
        instance.StopCoroutine(coroutine);
        dictionnaryObjectToCoroutine.Remove(ennemy);

        GameObject indicator = dictionnaryObjectToIndicatorClosest[ennemy];
        Destroy(indicator);
        dictionnaryObjectToIndicatorClosest.Remove(ennemy);

        indicator = dictionnaryObjectToIndicatorFarthest[ennemy];
        Destroy(indicator);
        dictionnaryObjectToIndicatorFarthest.Remove(ennemy);

    }

    IEnumerator IndicateEnnemy(GameObject ennemy)
    {
        Transform camTransform = Camera.main.gameObject.transform;
        GameObject indicatorClosest = dictionnaryObjectToIndicatorClosest[ennemy];
        GameObject indicatorFarthest = dictionnaryObjectToIndicatorFarthest[ennemy];
        Transform t_indicator = indicatorClosest.transform;
        Vector3 ennemyPos = ennemy.transform.position + ennemyOffset * ennemy.transform.up;
        float radius = planet.transform.localScale.x * 0.5f;
        RectTransform closestTransform = indicatorClosest.GetComponent<RectTransform>();
        RectTransform farthestTransform = indicatorFarthest.GetComponent<RectTransform>();

        WaitForEndOfFrame waitFrame = Helpers.GetWaitFrame;

        while (true) {
            targetPosOnScreen = Camera.main.WorldToScreenPoint(ennemyPos);

            if (onScreen(targetPosOnScreen, ennemyPos, ennemy, camTransform.position)) {
                indicatorClosest.SetActive(false);
                indicatorFarthest.SetActive(false);
                yield return waitFrame;
                continue;
            }

        // Find the tangent intersection point on the planet in a fake 2D space, where the planet is at (0,0) and the camera is aligned left to the planet (-camDistance, 0)
        camPlanetAxis =  (planet.transform.position - camTransform.position);
        camDistance = camPlanetAxis.magnitude;

        Vector2 planetScreenPos = Camera.main.WorldToScreenPoint(planet.transform.position);

        // a line from the enemy position that lies orthogonally on the cam-planet vector intersects at:
        pointOnCamPlanetAxisWhereEnemyAxisCrossesOrthogonally  = Vector3.Project((ennemy.transform.position - camTransform.position), camPlanetAxis); // result is relative to cam position
        // define the vector from the cam-planet axis to the enemy and extend it so it reaches the sayHello point
        enemyAxisOriginInWorldCoordinates = pointOnCamPlanetAxisWhereEnemyAxisCrossesOrthogonally + camTransform.position;
        orthogonalEnemyAxis = ennemy.transform.position - enemyAxisOriginInWorldCoordinates;
        
        teta = Mathf.Asin(radius / camDistance);



        //Set farthest indicator position and rotation
        indicatorFarthest.SetActive(true);

        alpha = -  teta;// the angle between the cam-to-planet line and cam to intersection line (tangent).
        
        // this is the distance from the cam-planet axis that the indicator must be placed at to appear on the horizon
        horizonHeight = Mathf.Tan(alpha) * pointOnCamPlanetAxisWhereEnemyAxisCrossesOrthogonally.magnitude;
        
        sayHello = orthogonalEnemyAxis.normalized * horizonHeight + enemyAxisOriginInWorldCoordinates;
        
        // now comes the easy part, project this onto the camera
        posOnScreen = Camera.main.WorldToScreenPoint(sayHello);
        pixelLocation = scaleFactor *posOnScreen - screenOffset;

        dir =  posOnScreen - planetScreenPos;
        angle = Vector2.SignedAngle(Vector2.up, dir);
        farthestTransform.anchoredPosition = scaleFactor *posOnScreen - screenOffset;;
        farthestTransform.localRotation = Quaternion.Euler((180f + angle) * Vector3.forward);


        //Set closest indicator position and rotation

        if (TooClose(ennemyPos, camTransform.position)) {
            indicatorClosest.SetActive(false);
            yield return waitFrame;
            continue;
        }

        indicatorClosest.SetActive(true);
            
        alpha = teta; // the angle between the cam-to-planet line and cam to intersection line (tangent).

        // this is the distance from the cam-planet axis that the indicator must be placed at to appear on the horizon
        horizonHeight = Mathf.Tan(alpha) * pointOnCamPlanetAxisWhereEnemyAxisCrossesOrthogonally.magnitude;
        
        sayHello = orthogonalEnemyAxis.normalized * horizonHeight + enemyAxisOriginInWorldCoordinates;
        
        // now comes the easy part, project this onto the camera
        posOnScreen = Camera.main.WorldToScreenPoint(sayHello);
        pixelLocation = scaleFactor *posOnScreen - screenOffset;
        
        closestTransform.anchoredPosition = pixelLocation;
        dir =  posOnScreen - planetScreenPos;
        angle = Vector2.SignedAngle(Vector2.up, dir);
        closestTransform.localRotation = Quaternion.Euler(angle * Vector3.forward);

        yield return waitFrame;
        }

    }
    bool onScreen(Vector2 input, Vector3 ennemyPos, GameObject ennemy, Vector3 pos){
        bool canBeOnScreen = !(input.x > Screen.width || input.x < 0 || input.y > Screen.height || input.y < 0);
        if (!canBeOnScreen) return false;
        else {
            Ray ray = new Ray(t_player.position, ennemyPos - t_player.position);
		    RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 20f, ~ignoredLayer)) {
                return hit.transform.gameObject == ennemy;}
            else return false;
        }
    }

    bool TooClose(Vector3 ennemyPos, Vector3 pos) {
        return (ennemyPos - pos).sqrMagnitude < 50; 
    }

}

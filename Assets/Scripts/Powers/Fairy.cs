using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using static Vault;

public class Fairy : Power
{
    public Transform player; 
    public float moveSpeed = 4.0f; 
    public float circleRadius = 5.0f; 
    private float angle = 0.0f;

    public Bullet bulletPrefab;

    public Transform firePoint; 
    public LayerMask enemyLayer; 

    private List<Transform> targets = new List<Transform>();

    private Transform detectedEnemy; 

    static protected WaitForSeconds wait;

    public GameObject fairy;
    public GameObject fairy2;
    private bool second = true;
    private bool third = true;

    public Animator animator;

    protected override void Start()
    {
        
        
        stats = DataManager.dictPowers["Fairy"].interactor;
        player = PlayerController.instance.transform;
        StartCoroutine(nameof(Reload));

        this.transform.position = player.position;

        if (stats.pierce == 4) { animator.SetBool("BigFairy", true); }
        if (stats.magazine >= 1 && second) { Instantiate(fairy, transform.position, Quaternion.identity); second = false; }
        if (stats.magazine == 2 && third) { Instantiate(fairy2, transform.position, Quaternion.identity); third = false; }
    }

    private void Update()
    {
        Vector3 offset = new Vector3(Mathf.Cos(angle) * circleRadius, Mathf.Sin(angle) * circleRadius, 0);
        Vector3 targetPosition = player.position + offset;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        angle += moveSpeed * Time.deltaTime;
        if (angle >= 360.0f)
        {
            angle = 0.0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ennemy"))
        {
            targets.Add(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ennemy"))
        {
            targets.Remove(other.transform);
        }
    }

    void Shoot(Transform detectedEnemy)
    {
        //soundManager.PlaySfx(transform, sfx.ennemyShoots);
        Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.gameObject.SetActive(true);

        bullet.FireFairy(stats.attackSpeed, stats.range, stats.pierce, detectedEnemy, new HitInfo(stats));
    }

    IEnumerator Reload()
    {
        while (true)
        {
            yield return Helpers.GetWait(stats.cooldown);
            if (targets.Count > 0) Shoot(targets.getRandom());
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using static Vault;

public class Fairy : MonoBehaviour
{
    private Transform player; 
    public float moveSpeed = 4.0f; 
    public float circleRadius = 5.0f; 
    private float angle = 0.0f;

    public Bullet bulletPrefab;


    private List<Transform> targets = new List<Transform>();

    private Transform detectedEnemy; 

    public Animator animator;
    private interactorStats stats;
    private Vector2 offsetRange;

    public void Setup(interactorStats stats, Vector2 offsetRange ,bool isBig = false)
    {
        this.stats = stats;
        this.offsetRange = offsetRange;
        player = PlayerController.instance.transform;
        transform.position = player.position;
        StartCoroutine(nameof(Reload));

        if (isBig)
        {
            animator.SetBool("BigFairy", true);
        }
    }

    private void Update()
    {
        Vector3 circleOffset = new Vector3(Mathf.Cos(angle) * circleRadius, Mathf.Sin(angle) * circleRadius, 0);
        Vector3 rangeOffset = new Vector3(offsetRange.x,offsetRange.y, 0);
        Vector3 offset = circleOffset + rangeOffset;

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

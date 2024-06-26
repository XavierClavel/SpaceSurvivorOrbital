using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : Ennemy
{
    [SerializeField] private float chargeDelay = 1f; // D�lai avant de foncer
    [SerializeField] private float chargeSpeed = 10f; // Vitesse de charge
    [SerializeField] private float chargeDuration = 1f; // Dur�e de la charge
    [SerializeField] private LineRenderer chargeIndicator; // Indicateur de charge
    [SerializeField] private CircleCollider2D detectionZone; // Zone de d�tection

    private Vector2 targetPosition; // Position cible du joueur
    public bool isCharging = false; // Indique si l'ennemi est en train de charger
    public bool isStopped = false; // Indique si l'ennemi est arr�t�
    private bool playerDetected = false; // Indique si le joueur est d�tect�

    protected override void Start()
    {
        base.Start();
        chargeIndicator.enabled = false; // D�sactive l'indicateur de charge au d�but
        detectionZone.isTrigger = true; // Assure que le collider est en mode Trigger
    }

    protected override void FixedUpdate()
    {
        if (knockback) return; // Ne fait rien si en knockback ou joueur non d�tect�
        base.FixedUpdate();

        if (!isCharging && !isStopped)
        {
          Move(directionToPlayer);
        }
    }

    private IEnumerator Charge()
    {
        targetPosition = player.transform.position; // Enregistre la position du joueur
        ShowChargeIndicator(targetPosition); // Affiche l'indicateur de charge
        yield return new WaitForSeconds(chargeDelay); // Attend avant de foncer
        chargeIndicator.enabled = false; // D�sactive l'indicateur de charge

        isCharging = true;
        float chargeTime = 0f;

        while (chargeTime < chargeDuration)
        {
            chargeTime += Time.fixedDeltaTime;
            Vector2 chargeDirection = (targetPosition - (Vector2)transform.position).normalized;
            Move(chargeDirection, chargeSpeed);
            yield return new WaitForFixedUpdate();
        }

        isCharging = false;
        isStopped = false; // R�initialise l'�tat d'arr�t pour permettre la poursuite
    }

    private void ShowChargeIndicator(Vector2 target)
    {
        chargeIndicator.enabled = true;
        chargeIndicator.positionCount = 2;
        chargeIndicator.SetPosition(0, transform.position);
        chargeIndicator.SetPosition(1, target);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(Vault.tag.Player))
        {
            PlayerController.Hurt(baseDamage);
            ApplyKnockback(1500);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Vault.tag.Player))
        {
            playerDetected = true;
            if (!isCharging && !isStopped)
            {
                isStopped = true;
                StartCoroutine(Charge());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(Vault.tag.Player))
        {
            playerDetected = false;
        }
    }
}

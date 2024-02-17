using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D collider;
    private static readonly int Open = Animator.StringToHash("open");

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag(Vault.tag.Player)) return;
        collider.enabled = false;
        animator.SetTrigger(Open);
        List<ArtefactHandler> artefactsRemaining = ScriptableObjectManager.dictKeyToArtefactHandler.Values.Difference(PlayerManager.artefacts);
        ArtefactHandler artefact = artefactsRemaining.getRandom();
        PlayerManager.AcquireArtefact(artefact);
        ArtefactPanel.Display(artefact);
    }
}

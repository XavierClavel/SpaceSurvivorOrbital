using UnityEngine;

[CreateAssetMenu(fileName = "CharacterHandler", menuName = Vault.other.scriptableObjectMenu + "CharacterHandler", order = 0)]
public class CharacterHandler : ObjectHandler
{
    [SerializeField]  public GameObject character;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Animator animator;
    public void Activate()
    {
        spriteRenderer = character.GetComponent<SpriteRenderer>();
        animator = character.GetComponent<Animator>();
    }
}
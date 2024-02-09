using UnityEngine;

[CreateAssetMenu(fileName = "CharacterHandler", menuName = Vault.other.scriptableObjectMenu + "CharacterHandler", order = 0)]
public class CharacterHandler : HidableObjectHandler
{
    [SerializeField] private RuntimeAnimatorController animatorController;
    public RuntimeAnimatorController getAnimatorController() => animatorController;

}
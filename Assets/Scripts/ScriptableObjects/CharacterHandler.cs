using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CharacterHandler", menuName = Vault.other.scriptableObjectMenu + "CharacterHandler", order = 0)]
public class CharacterHandler : HidableObjectHandler
{
    [SerializeField] private AnimatorController animatorController;
    public AnimatorController getAnimatorController() => animatorController;

}
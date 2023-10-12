using UnityEngine;

[CreateAssetMenu(fileName = "CharacterHandler", menuName = Vault.other.scriptableObjectMenu + "CharacterHandler", order = 0)]
public class CharacterHandler : ObjectHandler
{
    [SerializeField] private string power1;
    [SerializeField] private string power2;

    //public List<string> getPowers() {return ;}

}
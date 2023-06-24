using UnityEngine;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public enum character
{
    Pistolero
}

public enum interactor
{
    None,
    Gun,
    Laser
}

public class CsvParser : MonoBehaviour
{
    [SerializeField] ObjectReferencer objectReferencer;
    [SerializeField] TextAsset characterData;
    [SerializeField] TextAsset interactorData;
    [SerializeField] TextAsset localizationData;
    [SerializeField] TextAsset upgradesData;
    delegate void Formatter(List<string> s);
    public static Dictionary<character, CharacterData> dictCharacters = new Dictionary<character, CharacterData>();
    public static Dictionary<interactor, InteractorData> dictInteractors = new Dictionary<interactor, InteractorData>();
    public static Dictionary<string, LocalizedString> dictLocalization = new Dictionary<string, LocalizedString>();
    public static Dictionary<string, UpgradeData> dictUpgrades = new Dictionary<string, UpgradeData>();
    [SerializeField] character selectedCharacter = character.Pistolero;
    [SerializeField] interactor selectedInteractor = interactor.Laser;

    private void Awake()
    {
        if (dictCharacters.Count != 0) return;
        loadText(characterData, x => new CharacterData(x), x => CharacterData.Initialize(x));
        loadText(interactorData, x => new InteractorData(x), x => InteractorData.Initialize(x));
        loadText(localizationData, x => new LocalizedString(x), x => LocalizedString.Initialize(x));
        loadText(upgradesData, x => new UpgradeData(x), x => UpgradeData.Initialize(x));

        PlayerManager.setCharacter(dictCharacters[selectedCharacter]);
        PlayerManager.setInteractor(dictInteractors[selectedInteractor], objectReferencer.getInteractor(selectedInteractor));
    }

    void loadText(TextAsset csv, Formatter formatter, Formatter initializer = null, int offset = 0)
    {

        List<string> stringArray = csv.text.Split('\n').ToList();
        if (initializer != null) initializer(stringArray[offset].Split(';').ToList());
        stringArray.RemoveAt(offset);

        foreach (string s in stringArray)
        {
            formatter(s.Split(';').ToList());
        }
    }

}


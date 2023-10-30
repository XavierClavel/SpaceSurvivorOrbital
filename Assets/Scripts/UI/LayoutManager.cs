using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LayoutManager : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject prefabEmpty;
    [SerializeField] private GameObject prefabShield;
    List<GameObject> objectList = new List<GameObject>();
    private List<GameObject> emptyObjectsList = new List<GameObject>();
    private List<GameObject> shieldObjectsList = new List<GameObject>();
    private int index;
    private int maxAmount;
    private bool _isEmptyNull;
    private int shieldRemaining = 0;

    public void Setup(int maxAmount)
    {
        _isEmptyNull = prefabEmpty == null;
        
        this.maxAmount = maxAmount;
        index = maxAmount;
        
        InstantiateItems(prefab, objectList, this.maxAmount, true);
        if (!_isEmptyNull) InstantiateItems(prefabEmpty, emptyObjectsList, this.maxAmount, false);

    }

    public void SetupShields(int shieldAmount)
    {
        shieldRemaining = shieldAmount;
        InstantiateItems(prefabShield, shieldObjectsList, shieldAmount, true);
    }

    void InstantiateItems(GameObject prefab, List<GameObject> list, int amount, bool isActive)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject go = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            go.transform.SetParent(transform, false);
            list.Add(go);
            go.SetActive(isActive);
        }
    }

    public void DecreaseAmount(int amount = 1)
    {
        if (shieldRemaining > 0)
        {
            DecreaseShield();
            amount--;
            if (amount > 0) DecreaseAmount(amount);
            return;
        }
        int newIndex = Helpers.FloorInt(index - amount, 0);
        for (int i = index; i > newIndex; i--)
        {
            objectList[i - 1].SetActive(false);
            if (!_isEmptyNull) emptyObjectsList[i-1].SetActive(true);
        }
        index = newIndex;
    }

    public void DecreaseShield()
    {
        shieldRemaining--;
        shieldObjectsList[shieldRemaining].SetActive(false);
    }

    public void IncreaseAmount(int amount = 1)
    {
        int newIndex = Helpers.CeilInt(index + amount, maxAmount);
        for (int i = index; i < newIndex; i++)
        {
            objectList[i].SetActive(true);
            if (!_isEmptyNull) emptyObjectsList[i].SetActive(false);
        }
        index = newIndex;
    }

    public void SetAmount(int amount)
    {
        if (amount == index) return;
        if (amount < index) DecreaseAmount(index - amount);
        else IncreaseAmount(amount - index);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutManager : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] private GameObject prefabEmpty;
    List<GameObject> objectList = new List<GameObject>();
    private List<GameObject> emptyObjectsList = new List<GameObject>();
    int index;
    int maxAmount;
    private bool _isprefabNull;

    public void Setup(int maxAmount)
    {
        _isprefabNull = prefabEmpty == null;
        this.maxAmount = maxAmount;
        for (int i = 0; i < maxAmount; i++)
        {
            GameObject go = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            go.transform.SetParent(transform, false);
            objectList.Add(go);
            
        }
        index = maxAmount;
        if (_isprefabNull) return;
        for (int i = 0; i < maxAmount; i++)
        {
            GameObject emptyGo = Instantiate(prefabEmpty, Vector3.zero, Quaternion.identity);
            emptyGo.transform.SetParent(transform, false);
            emptyObjectsList.Add(emptyGo);
            emptyGo.SetActive(false);
        }

    }

    public void DecreaseAmount(int amount = 1)
    {
        int newIndex = Helpers.FloorInt(index - amount, 0);
        for (int i = index; i > newIndex; i--)
        {
            objectList[i - 1].SetActive(false);
            if (!_isprefabNull) emptyObjectsList[i-1].SetActive(true);
        }
        index = newIndex;
    }

    public void IncreaseAmount(int amount = 1)
    {
        int newIndex = Helpers.CeilInt(index + amount, maxAmount);
        for (int i = index; i < newIndex; i++)
        {
            objectList[i].SetActive(true);
            if (!_isprefabNull) emptyObjectsList[i].SetActive(false);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscreteBarHandler : MonoBehaviour
{

    [HideInInspector] public int maxAmount = 0;
    [HideInInspector] public int currentAmount = 0;
    [SerializeField] GameObject emptyDisplay;
    [SerializeField] GameObject fullDisplay;
    [SerializeField] GameObject layout;
    List<GameObject> emptyList = new List<GameObject>();
    List<GameObject> fullList = new List<GameObject>();


    public void Initialize()
    {
        for (int i = 0; i < maxAmount; i++)
        {
            GameObject instance = Instantiate(fullDisplay);
            instance.transform.SetParent(layout.transform);
            fullList.Add(instance);
        }
        for (int i = 0; i < maxAmount; i++)
        {
            GameObject instance = Instantiate(emptyDisplay);
            instance.transform.SetParent(layout.transform);
            emptyList.Add(instance);
        }

        SetActiveAmount(currentAmount, fullList);
        SetActiveAmount(maxAmount - currentAmount, emptyList);


    }

    void SetActiveAmount(int amount, List<GameObject> list)
    {
        for (int i = 0; i < maxAmount; i++)
        {
            if (i < amount)
            {
                list[i].SetActive(true);
            }
            else list[i].SetActive(false);
        }
    }

}

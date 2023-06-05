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

    /*
        public void SetMaxAmount(int amount, bool initialize = false)
        {
            if (initialize) maxAmount = 0;
            if (maxAmount == amount) return;
            if (maxAmount < amount)
            {
                for (int i = 0; i < amount - maxAmount; i++)
                {
                    GameObject instance = Instantiate(emptyDisplay);
                    instance.transform.SetParent(layout.transform);
                }
            }
            if (maxAmount > amount)
            {
                for (int i = maxAmount - 1; i >= amount; i--)
                {
                    Destroy(layout.transform.GetChild(i).gameObject);
                }
            }
            maxAmount = amount;
        }
        */

    public void SetAmount(int amount)
    {
        if (currentAmount == amount) return;
        int difference = currentAmount - amount;
        if (currentAmount > amount)
        {
            for (int i = amount; i < currentAmount; i++)
            {
                Destroy(layout.transform.GetChild(i).gameObject);
            }
            for (int i = 0; i < difference; i++)
            {
                GameObject instance = Instantiate(emptyDisplay);
                instance.transform.SetParent(layout.transform);
            }
        }
        if (currentAmount < amount)
        {
            for (int i = maxAmount - 1; i >= maxAmount - difference; i--)
            {
                Destroy(layout.transform.GetChild(i).gameObject);
            }
            for (int i = 0; i < difference; i++)
            {

            }
        }
        currentAmount = amount;
    }

    public void Initialize()
    {
        for (int i = 0; i < maxAmount; i++)
        {
            GameObject instance = Instantiate(fullDisplay);
            instance.transform.SetParent(layout.transform);
            //instance.GetComponent<RectTransform>().localScale = new Vector3(50, 50, 50);
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

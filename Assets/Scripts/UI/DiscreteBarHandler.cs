using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DiscreteBarHandler : MonoBehaviour
{

    [HideInInspector] public int maxAmount = 0;
    [HideInInspector] public int currentAmount = 0;
    [SerializeField] GameObject emptyDisplay;
    [SerializeField] GameObject fullDisplay;
    [SerializeField] GameObject layout;
    List<GameObject> emptyList = new List<GameObject>();
    List<GameObject> fullList = new List<GameObject>();
    private UnityEvent onFull = new UnityEvent();
    private UnityEvent onEmpty = new UnityEvent();
    private bool isFull;
    private bool isEmpty;

    public DiscreteBarHandler addOnFullAction(UnityAction action)
    {
        onFull.AddListener(action);
        return this;
    }
    
    public DiscreteBarHandler addOnEmptyAction(UnityAction action)
    {
        onEmpty.AddListener(action);
        return this;
    }


    public void Initialize(int maxAmount, int currentAmount)
    {
        this.maxAmount = maxAmount;
        this.currentAmount = currentAmount;
        
        for (int i = 0; i < maxAmount; i++)
        {
            GameObject instance = Instantiate(fullDisplay, layout.transform, true);
            instance.transform.localScale = Vector3.one;
            fullList.Add(instance);
        }
        for (int i = 0; i < maxAmount; i++)
        {
            GameObject instance = Instantiate(emptyDisplay, layout.transform, true);
            instance.transform.localScale = Vector3.one;
            emptyList.Add(instance);
        }

        SetActiveAmount(currentAmount, fullList);
        SetActiveAmount(maxAmount - currentAmount, emptyList);
        
        checkStatus();
    }

    public void IncreaseAmount(int increaseAmount = 1)
    {
        if (isFull) return;
        int newAmount = Helpers.CeilInt(currentAmount + increaseAmount, maxAmount);
        for (int i = currentAmount; i < newAmount; i++)
        {
            fullList[i].SetActive(true);
            emptyList[i].SetActive(false);
        }
        currentAmount = newAmount;
        checkStatus();
    }
    
    public void DecreaseAmount(int amount = 1)
    {
        if (isEmpty) return;
        int newIndex = Helpers.FloorInt(currentAmount + amount, 0);
        for (int i = currentAmount - 1; i >= newIndex; i--)
        {
            fullList[i].SetActive(false);
            emptyList[i].SetActive(true);
        }
        currentAmount = newIndex;
        checkStatus();
    }

    private void SetActiveAmount(int amount, List<GameObject> list)
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

    private void checkStatus()
    {
        isEmpty = false;
        isFull = false;
        if (currentAmount == 0)
        {
            isEmpty = true;
            onEmpty.Invoke();
        }

        if (currentAmount == maxAmount)
        {
            isFull = true;
            onFull.Invoke();
        }
        
    }

}

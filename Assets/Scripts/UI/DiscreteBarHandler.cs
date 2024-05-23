using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DiscreteBarHandler : MonoBehaviour
{

    [HideInInspector] public int maxAmount = 0;
    [HideInInspector] public int currentAmount = 0;
    public TextMeshProUGUI currentText;

    [SerializeField] GameObject emptyDisplay;
    [SerializeField] GameObject fullDisplay;
    [SerializeField] GameObject layout;
    List<GameObject> emptyList = new List<GameObject>();
    List<GameObject> fullList = new List<GameObject>();
    private UnityEvent onFullAction = new UnityEvent();
    private UnityEvent onEmptyAction = new UnityEvent();
    private bool isFull;
    private bool isEmpty;

    public DiscreteBarHandler addOnFullAction(UnityAction action)
    {
        onFullAction.AddListener(action);
        return this;
    }
    
    public DiscreteBarHandler addOnEmptyAction(UnityAction action)
    {
        onEmptyAction.AddListener(action);
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

        setActiveAmount(currentAmount, fullList);
        setActiveAmount(maxAmount - currentAmount, emptyList);
        
        checkStatus();
    }

    public void setAmount(int newAmount)
    {
        if (newAmount == currentAmount) return;
        else if (newAmount > currentAmount) increaseAmount(newAmount - currentAmount);
        else if (newAmount < currentAmount) decreaseAmount(currentAmount - newAmount);
    }

    public void increaseAmount(int increaseAmount = 1)
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
    
    public void decreaseAmount(int amount = 1)
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

    private void setActiveAmount(int amount, List<GameObject> list)
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
            onEmptyAction.Invoke();
        }

        if (currentAmount == maxAmount)
        {
            isFull = true;
            onFullAction.Invoke();
        }
        
    }

    //TODO : move out of Update method
    public void Update()
    {
        currentText.text = currentAmount.ToString();
    }

}

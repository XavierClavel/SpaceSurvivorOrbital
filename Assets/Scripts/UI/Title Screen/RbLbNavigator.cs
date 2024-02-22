using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class RbLbObject
{
    public Button LB;
    public Button RB;

    public RbLbObject(Button lb, Button rb)
    {
        this.LB = lb;
        this.RB = rb;
    }
}

public class RbLbNavigator : MonoBehaviour
{
    [SerializeField] private List<RbLbObject> list = new List<RbLbObject>();
    private int currentIndex = 0;
    private InputMaster inputMaster;
    public static RbLbNavigator instance;

    public void addRbLbObject(RbLbObject o) => list.Add(o);
    public void setIndex(int i) => currentIndex = i;

    private void Awake()
    {
        instance = this;
        inputMaster = new InputMaster();
        inputMaster.Enable();
        inputMaster.UI.RB.performed += ctx => onRB();
        inputMaster.UI.LB.performed += ctx => onLB();
    }

    private void OnDestroy()
    {
        inputMaster.Disable();
    }

    public void Enable()
    {
        inputMaster.Enable();
    }

    public void Disable()
    {
        inputMaster.Disable();
    }

    private void onRB()
    {
        list[currentIndex].RB?.onClick.Invoke();
        if (currentIndex != list.maxIndex()) currentIndex++;
    }

    private void onLB()
    {
        list[currentIndex].LB?.onClick.Invoke();
        if (currentIndex != 0) currentIndex--;
    }
}

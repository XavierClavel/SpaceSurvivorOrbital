using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class Book<T> : MonoBehaviour where T: Component
{
    private int currentPage = 0;
    [SerializeField] protected List<T> pages;

    private void Awake()
    {
        for (int i = 0; i < pages.Count; i++)
        {
            setState(pages[i], i == 0);
        }
    }

    public void NextPage()
    {
        if (currentPage == pages.Count - 1) return;
        deactivate(pages[currentPage]);
        currentPage++;
        activate(pages[currentPage]);
    }
    
    public void PreviousPage()
    {
        if (currentPage == 0) return;
        deactivate(pages[currentPage]);
        currentPage--;
        activate(pages[currentPage]);
    }

    public void setActivePage(int i)
    {
        deactivate(pages[currentPage]);
        currentPage = i;
        activate(pages[currentPage]);
    }

    protected virtual void setState(T item, bool active)
    {
        if (active) activate(item);
        else deactivate(item);
    }

    protected virtual void activate(T item)
    {
        item.gameObject.SetActive(true);
    }

    protected virtual void deactivate(T item)
    {
        item.gameObject.SetActive(false);
    }
}

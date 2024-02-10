using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    private int currentPage = 0;
    [SerializeField] private List<GameObject> pages;

    private void Awake()
    {
        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].SetActive(i == 0);
        }
    }

    public void NextPage()
    {
        if (currentPage == pages.Count-1) return;
        pages[currentPage].gameObject.SetActive(false);
        currentPage++;
        pages[currentPage].gameObject.SetActive(true);
    }
    
    public void PreviousPage()
    {
        if (currentPage == 0) return;
        pages[currentPage].gameObject.SetActive(false);
        currentPage--;
        pages[currentPage].gameObject.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.InputSystem;
using DG.Tweening;

public class SkillTree : MonoBehaviour
{
    EventSystem eventSystem;
    public static SkillTree instance;
    [SerializeField] ScrollRect scrollRect;
    RectTransform scrollRectTransform;
    RectTransform contentPanel;
    GameObject previousSelected;
    InputMaster inputActions;



    private void Awake()
    {
        /*
        eventSystem = EventSystem.current;

        inputActions = new InputMaster();
        inputActions.UI.Navigate.started += ctx => StartCoroutine(nameof(OnNavigate));
        inputActions.UI.Navigate.canceled += ctx => StopCoroutine(nameof(OnNavigate));
        inputActions.UI.Navigate.performed += xtc => UpdateDisplay();
        inputActions.Enable();

        scrollRectTransform = scrollRect.GetComponent<RectTransform>();
        contentPanel = scrollRect.content;
        */

        instance = this;

        if (!PlayerManager.isPlayingWithGamepad) Cursor.visible = true;

    }


    IEnumerator OnNavigate()
    {
        while (true)
        {
            UpdateDisplay();
            yield return null;
        }
    }

    void UpdateDisplay()
    {
        if (eventSystem.currentSelectedGameObject == previousSelected) return;

        previousSelected = eventSystem.currentSelectedGameObject;
        RectTransform selectedRectTransform = eventSystem.currentSelectedGameObject.GetComponent<RectTransform>();
        RectTransform parentRectTransform = selectedRectTransform.parent.GetComponent<RectTransform>(); ;
        // The position of the selected UI element is the absolute anchor position,
        // ie. the local position within the scroll rect + its height if we're
        // scrolling down. If we're scrolling up it's just the absolute anchor position.
        //float selectedPositionY = Mathf.Abs(selectedRectTransform.anchoredPosition.y) + selectedRectTransform.rect.height;
        float selectedPositionY = selectedRectTransform.anchoredPosition.y + selectedRectTransform.rect.height;
        selectedPositionY += parentRectTransform.anchoredPosition.y;
        // The upper bound of the scroll view is the anchor position of the content we're scrolling.
        float scrollViewBottom = contentPanel.anchoredPosition.y;
        // The lower bound is the anchor position + the height of the scroll rect.
        float scrollViewTop = contentPanel.anchoredPosition.y + scrollRectTransform.rect.height;

        //selectedPositionY *= -1f;


        // If the selected position is below the current lower bound of the scroll view we scroll down.
        if (selectedPositionY > scrollViewTop)
        {
            float newY = selectedPositionY - scrollRectTransform.rect.height - 800f;
            //contentPanel.anchoredPosition = new Vector2(contentPanel.anchoredPosition.x, newY);
            contentPanel.DOAnchorPos(new Vector2(contentPanel.anchoredPosition.x, newY), 0.5f);
        }
        // If the selected position is above the current upper bound of the scroll view we scroll up.
        else if (selectedPositionY < scrollViewBottom)
        {
            //contentPanel.DOAnchorPos(new Vector2(contentPanel.anchoredPosition.x, Mathf.Abs(selectedRectTransform.anchoredPosition.y)), 0.5f);
            contentPanel.DOAnchorPos(new Vector2(contentPanel.anchoredPosition.x, -selectedPositionY - 500f), 0.5f);
        }
    }


    private void OnDisable()
    {
        //inputActions.Disable();
    }
}

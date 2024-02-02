using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class DraggableHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public Draggable hoverDraggable;
    [HideInInspector] public Draggable selectedDraggable;
    public RectTransform rectTransform;
    
    public void OnPointerEnter(PointerEventData e)
    {
        Draggable draggable = getSelectedDraggable();
        if (draggable == null) return;
        hoverDraggable = draggable;
        hoverDraggable.hoverDraggableHolder = this;
        onPointerEnter();
    }

    protected abstract Draggable getSelectedDraggable();
    
    public void OnPointerExit(PointerEventData e)
    {
        if (hoverDraggable == null) return;
        onPointerExit();
        hoverDraggable.hoverDraggableHolder = null;
        hoverDraggable = null;
    }

    protected virtual void onPointerEnter()
    {
        
    }

    protected virtual void onPointerExit()
    {
        
    }
    
    public void UseDraggable()
    {
        onUseDraggable();
        if (selectedDraggable == null) return;
        Destroy(selectedDraggable.gameObject);
        selectedDraggable = null;
    }

    public virtual void onUseDraggable()
    {
        
    }
    
    public bool isFree(Draggable draggable)
    {
        return selectedDraggable == null || selectedDraggable == draggable;
    }

}

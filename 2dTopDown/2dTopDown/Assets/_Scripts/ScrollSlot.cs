using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollSlot : MonoBehaviour, IPointerClickHandler
{
    public ScrollElement ScrollElement;

    internal void AddToSlot(ScrollElement scrollElement)
    {
        if (ScrollElement != null)
        {
            ScrollElement.RemoveFromSlot();
        }
        ScrollElement = scrollElement;
    }

    public void OnPointerClick(PointerEventData eventData)
    {     
        if (ScrollElement)
            ScrollElement.RemoveFromSlot();
    }

    internal void RemoveFromSlot()
    {
        ScrollElement = null;
    }
}

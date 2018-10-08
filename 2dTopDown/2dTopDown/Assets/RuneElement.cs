using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RuneElement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Rune Rune;
    public Image Image;

    private Transform _avalibleListParent;

    private RuneSlot _runeslot;

    public void Init(Rune rune)
    {
        _avalibleListParent = transform.parent;
        Rune = rune;
        Image.sprite = Rune.Image;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(_avalibleListParent.parent.parent);
        Image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = new Vector2(transform.position.x + eventData.delta.x, transform.position.y + eventData.delta.y);
        if (eventData.pointerEnter && eventData.pointerEnter.name == "RuneSlot")
        {
            RuneSlot rSlot = eventData.pointerEnter.gameObject.GetComponent<RuneSlot>();
            if (rSlot && !rSlot.Locked)
            {
                _runeslot = rSlot;
            }
        }
        else if (eventData.pointerEnter && eventData.pointerEnter.name == "RuneImage")
        {
            RuneElement rRlement = eventData.pointerEnter.GetComponentInParent<RuneElement>();
            RuneSlot rSlot = null;
            if (rRlement)
                rSlot = rRlement.GetRuneSlot();

            if (rSlot && !rSlot.Locked)
            {
                _runeslot = rSlot;
            }
        }
        else
        {
            _runeslot = null;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Image.raycastTarget = true;
        if (_runeslot == null)
        {
            transform.SetParent(_avalibleListParent);
        }
        else
        {
            _runeslot.AddToSlot(this);
            transform.SetParent(_runeslot.transform);
        }
    }

    public void RemoveFromSlot()
    {
        transform.SetParent(_avalibleListParent);
        _runeslot = null;
    }

    private RuneSlot GetRuneSlot()
    {
        return _runeslot;
    }
}

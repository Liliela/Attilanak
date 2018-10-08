﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RuneElement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public RuneDescriptor Rune;
    public Image Image;

    private Transform _startParent;

    private RuneSlot _newRuneSlot;
    private RuneSlot _runeslot;

    public void Init(RuneDescriptor rune)
    {
        _startParent = transform.parent;
        Rune = rune;
        Image.sprite = Rune.Image;
    }   

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        transform.SetParent(_startParent.parent.parent);
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
                _newRuneSlot = rSlot;
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
                _newRuneSlot = rSlot;
            }
        }
        else
        {
            _newRuneSlot = null;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        Image.raycastTarget = true;
        if (_newRuneSlot)
        {
            if (_runeslot)
                _runeslot.RemoveFromSlot();
            transform.SetParent(_newRuneSlot.transform);
            _newRuneSlot.AddToSlot(this);
            _runeslot = _newRuneSlot;
            _newRuneSlot = null;
        }
        else
        {
            if (_runeslot)
                _runeslot.RemoveFromSlot();
            _runeslot = null;
            transform.SetParent(_startParent);
        }
    }

    public void RemoveFromSlot()
    {
        if (_runeslot)
            _runeslot.RemoveFromSlot();
        _runeslot = null;
        transform.SetParent(_startParent);
    }

    private RuneSlot GetRuneSlot()
    {
        return _runeslot;
    }
}
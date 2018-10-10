using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RuneElement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    public RuneDescriptor Rune;
    public Image Image;

    private Transform _startParent;

    private RuneSlot _newRuneSlot;
    private RuneSlot _runeslot;
    private MagicMaker _magicMaker;

    public void Init(RuneDescriptor rune, MagicMaker magicMaker)
    {
        _magicMaker = magicMaker;
        _startParent = transform.parent;
        Rune = rune;
        Image.sprite = Rune.Image;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
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

    public void PutInSlot(RuneSlot newSlot)
    {
        if (_runeslot)
            _runeslot.RemoveFromSlot();
        transform.SetParent(newSlot.transform);
        newSlot.AddToSlot(this);
        _runeslot = newSlot;
    }

    private RuneSlot GetRuneSlot()
    {
        return _runeslot;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_runeslot == null)
            _magicMaker.PutInFirstAvalibleSlot(this);
        else
            RemoveFromSlot();
    }
}

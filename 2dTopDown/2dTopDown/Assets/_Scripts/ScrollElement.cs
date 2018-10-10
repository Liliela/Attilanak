using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollElement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Image Image;
    public SpellDescriptor SpellDescriptor;

    private ScrollSlot _newScrollSlot;
    private ScrollSlot _scrollSlot;
    private Transform _startParent;

    private bool _onDrag;

    public void Init(SpellDescriptor spellD)
    {
        SpellDescriptor = spellD;
        Image.sprite = spellD.Image;
        _startParent = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_scrollSlot && !_scrollSlot.ValidForChangeSlotSpell()) return;

        _onDrag = true;
        transform.SetParent(_startParent.parent.parent.parent);
        transform.SetAsLastSibling();
        Image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_onDrag) return;

        transform.position = new Vector2(transform.position.x + eventData.delta.x, transform.position.y + eventData.delta.y);
        if (eventData.pointerEnter && eventData.pointerEnter.name == "ScrollSlot")
        {
            ScrollSlot sSlot = eventData.pointerEnter.gameObject.GetComponent<ScrollSlot>();
            if (sSlot)
            {
                _newScrollSlot = sSlot;
            }
        }
        else if (eventData.pointerEnter && eventData.pointerEnter.name == "ScrollImage")
        {
            ScrollElement sRlement = eventData.pointerEnter.GetComponentInParent<ScrollElement>();
            ScrollSlot sSlot = null;
            if (sRlement)
                sSlot = sRlement.GetScrollSlot();

            if (sSlot)
            {
                _newScrollSlot = sSlot;
            }
        }
        else
        {
            _newScrollSlot = null;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!_onDrag) return;

        if (_newScrollSlot && _newScrollSlot.ValidForChangeSlotSpell())
        {
            Image.raycastTarget = true;
            if (_newScrollSlot)
            {
                if (_scrollSlot)
                    _scrollSlot.RemoveFromSlot();
                transform.SetParent(_newScrollSlot.transform);
                transform.SetAsFirstSibling();
                _newScrollSlot.AddToSlot(this);
                _scrollSlot = _newScrollSlot;
                _newScrollSlot = null;
            }
            else
            {
                if (_scrollSlot)
                    _scrollSlot.RemoveFromSlot();
                _scrollSlot = null;
                transform.SetParent(_startParent);
            }
        }
        else
        {
            if (_scrollSlot)
                _scrollSlot.RemoveFromSlot();
            Image.raycastTarget = true;
            _scrollSlot = null;
            transform.SetParent(_startParent);
        }

        _onDrag = false;
    }

    private ScrollSlot GetScrollSlot()
    {
        return _scrollSlot;
    }

    public void RemoveFromSlot()
    {
        if (_scrollSlot)
            _scrollSlot.RemoveFromSlot();
        _scrollSlot = null;
        transform.SetParent(_startParent);
    }
}

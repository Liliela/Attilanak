using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollSlot : MonoBehaviour, IPointerClickHandler
{
    public ScrollElement ScrollElement;
    private bool _onCooldown;
    private bool _onWarm;
    public Image CooldownImage;
    private float _cdTimer;
    public string InputName;

    private void Awake()
    {
        CooldownImage.fillAmount = 0f;
    }

    private void Update()
    {
        if (_onCooldown)
        {
            _cdTimer += Time.deltaTime;
            if (_cdTimer > ScrollElement.SpellDescriptor.Cooldown)
            {
                _cdTimer = 0;
                _onCooldown = false;
                CooldownImage.fillAmount = 0f;
            }
            else
            {
                CooldownImage.fillAmount = 1 - (_cdTimer / ScrollElement.SpellDescriptor.Cooldown);               
            }
        }
    }

    public bool CanWarmSlot()
    {
        if (_onCooldown || !ScrollElement) return false;
        return true;
    }

    internal void WarmSlot()
    {
        _onWarm = true;
    }

    public bool CanFireSlot()
    {
        if (!_onWarm || !ScrollElement) return false;
        return true;
    }

    internal void FireSlot()
    {
        _onCooldown = true;
        _onWarm = false;
    }

    internal void AddToSlot(ScrollElement scrollElement)
    {
        if (ScrollElement != null)
        {
            ScrollElement.RemoveFromSlot();
        }
        ScrollElement = scrollElement;
    }

    public void OnPointerClick(PointerEventData eventdata)
    {
        //if (scrollelement)
        //    scrollelement.removefromslot();
    }

    public void RemoveFromSlot()
    {
        ScrollElement = null;
    }

    public bool ValidForChangeSlotSpell()
    {
        if (_onCooldown)
            return false;

        return true;
    }
}

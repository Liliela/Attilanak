using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotUser : MonoBehaviour
{
    public List<ScrollSlot> Slots;
    private SpellCaster _spellCaster;
    private Targeter _targeter;

    private void Awake()
    {
        _targeter = GetComponent<Targeter>();
        _spellCaster = GetComponent<SpellCaster>();
    }

    void Update()
    {
        UpdateInput();
    }

    private void UpdateInput()
    {
        foreach (var slot in Slots)
        {
            if (Input.GetButtonDown(slot.InputName))
            {
                if (slot.CanWarmSlot())
                {
                    if (_spellCaster.TryWarmSpell(slot.ScrollElement.SpellDescriptor))
                    {
                        slot.WarmSlot();
                    }
                }
            }
        }

        foreach (var slot in Slots)
        {
            if (Input.GetButtonUp(slot.InputName))
            {
                if (slot.CanFireSlot())
                {

                    if (_spellCaster.TryFireSpell(slot.ScrollElement.SpellDescriptor))
                    {
                        slot.FireSlot();
                    }
                }
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MagicMaker : SkriptableReferenceHolder
{
    public GameObject RunePrefab;
    public Transform AvalibleRunesParent;
    public List<RuneSlot> RuneSlots;
    private List<RuneDescriptor> _myRunes = new List<RuneDescriptor>();
    private List<SpellDescriptor> _mySpells = new List<SpellDescriptor>();

    private List<RuneElement> _instancedRunes = new List<RuneElement>();

    public GameObject ScrollPrefab;
    public Transform SpellBookParent;

    public ScrollReserchSlot ScrollReserchSlot;

    public override void Awake()
    {
        base.Awake();
        Init();
        UpdateAvalibleRunes();
    }

    private void Init()
    {
        ScrollPrefab.SetActive(false);
        RunePrefab.SetActive(false);
        ScrollReserchSlot.Init(this);
        foreach (var runeRef in Runes)
        {
            RuneDescriptor rune = Instantiate(runeRef);
            _myRunes.Add(rune);
        }
        foreach (var spellRef in Spells)
        {
            SpellDescriptor spell = Instantiate(spellRef);
            _mySpells.Add(spell);
        }
        foreach (var rSlot in RuneSlots)
        {
            rSlot.Init(this);
        }
    }

    public void UpdateRunes()
    {
        SpellDescriptor spellD = GetCorrectSpell();
        if (spellD == null)
        {
            ScrollReserchSlot.ClearSlot();
            Debug.LogWarning("NoCorrectRune");
        }
        else
        {
            CrateSpell(spellD);
        }
    }

    private void CrateSpell(SpellDescriptor spellD)
    {
        ScrollReserchSlot.UpdateSlot(spellD);
    }

    public void LearnSpell(SpellDescriptor spellD)
    {
        ScrollReserchSlot.ClearSlot();
        GameObject go = Instantiate(ScrollPrefab, SpellBookParent);
        go.SetActive(true);
        ScrollElement se = go.GetComponent<ScrollElement>();
        if (se)
        {
            spellD.Learned = true;
            se.Init(spellD);
        }
    }

    private SpellDescriptor GetCorrectSpell()
    {
        List<RuneType> runeTypes = new List<RuneType>();
        foreach (var rS in RuneSlots)
        {
            if (rS.RuneElement)
            {
                runeTypes.Add(rS.RuneElement.Rune.RuneType);
            }
        }

        foreach (var spell in _mySpells)
        {
            if (runeTypes.Count() == spell.RuneTypes.Count())
                if (runeTypes.Intersect(spell.RuneTypes).Count() == runeTypes.Count())
                {
                    return spell;
                };
        }

        return null;
    }

    private void UpdateAvalibleRunes()
    {
        foreach (var runeElement in _instancedRunes)
        {
            Destroy(runeElement.gameObject);
        }
        _instancedRunes.Clear();
        foreach (var rune in _myRunes)
        {
            if (rune.Unlocked)
            {
                GameObject go = Instantiate(RunePrefab, AvalibleRunesParent);
                go.SetActive(true);
                RuneElement re = go.GetComponent<RuneElement>();
                re.Init(rune, this);
                _instancedRunes.Add(re);
            }
        }
    }

    public void PutInFirstAvalibleSlot(RuneElement runeElement)
    {
        foreach (var slot in RuneSlots)
        {
            if (slot.RuneElement == null && !slot.Locked)
            {
                runeElement.PutInSlot(slot);
                return;
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMaker : MonoBehaviour
{
    public List<Rune> RunesReferences;
    public GameObject RunePrefab;
    public Transform AvalibleRunesParent;

    public List<RuneSlot> RuneSlots;

    private List<Rune> _myRunes = new List<Rune>();
    private List<RuneElement> _instancedRunes = new List<RuneElement>();

    private void Awake()
    {
        RunePrefab.SetActive(false);
        Init();
        UpdateAvalibleRunes();
    }

    private void Init()
    {
        foreach (var runeRef in RunesReferences)
        {
            Rune rune = ScriptableObject.CreateInstance<Rune>();
            rune.Image = runeRef.Image;
            rune.RuneType = runeRef.RuneType;
            rune.Unlocked = runeRef.Unlocked;
            _myRunes.Add(rune);
        }
        foreach (var rSlot in RuneSlots)
        {
            rSlot.Init(this);
        }
    }

    public void UpdateRunes()
    {
      
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
                re.Init(rune);
                _instancedRunes.Add(re);
            }
        }
    }
}

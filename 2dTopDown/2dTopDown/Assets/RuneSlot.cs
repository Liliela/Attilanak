using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuneSlot : MonoBehaviour
{
    public RuneElement RuneElement;
    public Image Image;
    public Sprite LockedSprite;
    public Sprite UnlockedSprite;

    public bool Locked = false;
    private MagicMaker _magicMaker;

    internal void Init(MagicMaker magicMaker)
    {
        _magicMaker = magicMaker;
        SetLocked(Locked);
    }

    public void SetLocked(bool value)
    {
        Locked = value;
        if (Locked)
        {
            Image.sprite = LockedSprite;
        }
        else
        {
            Image.sprite = UnlockedSprite;
        }
    }

    public void AddToSlot(RuneElement rune)
    {
        if (RuneElement != null)
        {
            RuneElement.RemoveFromSlot();
        }
        RuneElement = rune;
        _magicMaker.UpdateRunes();
    }
}

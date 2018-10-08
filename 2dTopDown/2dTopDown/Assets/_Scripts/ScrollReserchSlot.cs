using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollReserchSlot : MonoBehaviour
{
    public Image Image;
    public bool WaitForLearn;
    private SpellDescriptor _spell;
    private MagicMaker _maker;
    private Button _button;
    private Sprite _originalSprite;

    public void UpdateSlot(SpellDescriptor spell)
    {
        _spell = spell;
        WaitForLearn = true;
        Image.sprite = spell.Image;

        _button.interactable = !_spell.Learned;
    }

    public void ClearSlot()
    {
        _button.interactable = false;
        _spell = null;
        WaitForLearn = false;
        Image.sprite = _originalSprite;
    }

    public void LearnSpell()
    {
        _maker.LearnSpell(_spell);
    }

    internal void Init(MagicMaker magicMaker)
    {
        _originalSprite = Image.sprite;
           _maker = magicMaker;
        _button = GetComponent<Button>();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterEffects : MonoBehaviour
{
    private List<Animator> _effects = new List<Animator>();

    private void Awake()
    {
        _effects = GetComponentsInChildren<Animator>().ToList();
    }

    public void ActivateOneTimeEffect(string effectName)
    {
        Animator anim = _effects.Find(x => x.gameObject.name == effectName);
        if (anim)
        {
            anim.SetTrigger("Activate");
        }
    }
}

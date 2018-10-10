using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    private Targeter _targeter;

    private void Awake()
    {
        _targeter = GetComponent<Targeter>();
    }

    public bool TryWarmSpell(SpellDescriptor spellDescriptor)
    {
        switch (spellDescriptor.SpellType)
        {
            case SpellType.Target:
                break;
            case SpellType.AOE:
                break;
            case SpellType.Projectile:
                break;
            case SpellType.Global:
                break;
            default:
                break;
        }
        return true;
    }

    public bool TryFireSpell(SpellDescriptor spellDescriptor)
    {
        switch (spellDescriptor.SpellType)
        {
            case SpellType.Target:
                return CastTargetSpell(spellDescriptor);
            case SpellType.AOE:
                break;
            case SpellType.Projectile:
                break;
            case SpellType.Global:
                break;
            default:
                break;
        }
        return true;
    }


    private bool CastTargetSpell(SpellDescriptor spellDescriptor)
    {
        //checkDistance
        if (!_targeter.CurrentFocusedCharacter || Vector2.Distance(transform.position, _targeter.CurrentFocusedCharacter.transform.position) > spellDescriptor.Range)
        {
            return false;
        }

        SpellAffector sa = _targeter.CurrentFocusedCharacter.GetComponent<SpellAffector>();
        if (sa)
        {
            Debug.Log("SendRpc " + spellDescriptor.Name);
            sa.photonView.RPC("RPC_ApplyTargetSpell", PhotonTargets.AllBuffered, spellDescriptor.Name);
        }
        else
        {
            return false;
        }

        return true;
    }
}

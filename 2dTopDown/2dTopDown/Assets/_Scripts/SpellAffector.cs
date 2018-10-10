using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAffector : Photon.MonoBehaviour
{
    private SkriptableReferenceHolder _scriptableRefHolder;
    private GeneralStatistics _stat;
    private CharacterEffects _cEffects;

    private void Awake()
    {
        _scriptableRefHolder = GetComponent<SkriptableReferenceHolder>();
        _stat = GetComponent<GeneralStatistics>();
        _cEffects = GetComponentInChildren<CharacterEffects>();
    }

    [PunRPC]
    public void RPC_ApplyTargetSpell(string spellName)
    {
        SpellDescriptor sd = _scriptableRefHolder.AvalibleSpells.Find(x => x.Name == spellName);
        AffactByTargetSpell(sd);
    }

    public void AffactByTargetSpell(SpellDescriptor sd)
    {
        if (sd.Damage != 0)
        {
            _stat.ChangeHealth(-sd.Damage, sd.EnergyType);
        }
        foreach (var buff in sd.ApplyBuff)
        {

        }
        _cEffects.ActivateOneTimeEffect(sd.EffectName);
    }
}

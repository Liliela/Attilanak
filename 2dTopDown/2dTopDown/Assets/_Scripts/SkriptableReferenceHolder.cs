using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkriptableReferenceHolder : MonoBehaviour
{
    public List<RuneDescriptor> RunesReferences;
    public List<SpellDescriptor> AvalibleSpells;

    public virtual void Awake()
    {
        Object[] runes = Resources.LoadAll("Runes", typeof(RuneDescriptor));
        Object[] spells = Resources.LoadAll("Spells", typeof(SpellDescriptor));

        foreach (var item in runes)
        {
            RunesReferences.Add((RuneDescriptor)item);
        }
        foreach (var item in spells)
        {
            AvalibleSpells.Add((SpellDescriptor)item);
        }
    }
}

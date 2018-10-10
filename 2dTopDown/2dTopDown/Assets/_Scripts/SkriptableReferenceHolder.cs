using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkriptableReferenceHolder : MonoBehaviour
{
    public List<RuneDescriptor> Runes;
    public List<SpellDescriptor> Spells;
    public List<BuffDescriptor> Buffs;

    public virtual void Awake()
    {
        Object[] runes = Resources.LoadAll("Runes", typeof(RuneDescriptor));
        Object[] spells = Resources.LoadAll("Spells", typeof(SpellDescriptor));
        Object[] buffs = Resources.LoadAll("Buffs", typeof(BuffDescriptor));

        foreach (var item in runes)
        {
            Runes.Add((RuneDescriptor)item);
        }
        foreach (var item in spells)
        {
            Spells.Add((SpellDescriptor)item);
        }
        foreach (var item in buffs)
        {
            Buffs.Add((BuffDescriptor)item);
        }
    }
}

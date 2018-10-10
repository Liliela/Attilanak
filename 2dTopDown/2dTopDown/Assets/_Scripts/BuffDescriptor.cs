using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Buff")]
public class BuffDescriptor : ScriptableObject
{
    public string Name;
    public Sprite Sprite;
    public BuffCategory BuffType;
    public BuffStackingType BuffStackingType;
    public float Duration;
    public BuffChangeType BuffChangeType;
    public float Change;
}

public enum BuffCategory
{
    PositiveEnergyWave,
    NegativeEnergyWave,
    ManaRegen,
    DamageChange,
    Bleed,
    Poison,
    Burn,
    Freeze,
    MoveChange,
    Inmobile,
    Stun,
}

public enum BuffStackingType
{
    Refresh,
    StackInTime,
    //StackInEffectAndRefresh,
}

public enum BuffChangeType
{
    StatPercentChange,
    StatChange,
}
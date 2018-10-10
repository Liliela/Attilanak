using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ScriptableObject/Spell")]
public class SpellDescriptor : ScriptableObject
{
    public string Name;
    public string EffectName;
    public List<RuneType> RuneTypes;
    public Sprite Image;
    public bool Learned;

    public SpellType SpellType;
    public EnergyType EnergyType;
    public float Cooldown;
    public float Damage;
    public float Range;
    public float Radius;

    public List<BuffDescriptor> ApplyBuff;
}

public enum SpellType
{
    Target,
    AOE,
    Projectile,
    Global,
}

public enum EnergyType
{
    None = 0,
    Fire = 5,
    Air = 10,
    Earth = 20,
    Water = 30,
    Negative = 40,
    Positive = 50,
    Physical = 60,
    TrueDamage = 70,
}
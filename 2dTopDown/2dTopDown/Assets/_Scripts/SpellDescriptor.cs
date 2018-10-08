using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ScriptableObject/Spell")]
public class SpellDescriptor : ScriptableObject
{
    public string Name;
    public List<RuneType> RuneTypes;
    public Sprite Image;
    public bool Learned;
}

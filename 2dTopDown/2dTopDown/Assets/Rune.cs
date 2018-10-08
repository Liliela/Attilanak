using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(menuName = "ScriptableObject/Rune")]
public class Rune : ScriptableObject
{
    public Sprite Image;
    public RuneType RuneType;
    public bool Unlocked = false;   
}

public enum RuneType
{
    Fire = 0,
    Earth = 10,
    Wind = 20,
    Water = 10,
    Life = 10,
    Death = 10,
}
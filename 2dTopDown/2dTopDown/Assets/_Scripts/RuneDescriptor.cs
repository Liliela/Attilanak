using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(menuName = "ScriptableObject/Rune")]
public class RuneDescriptor : ScriptableObject
{
    public string Name;
    public Sprite Image;
    public RuneType RuneType;
    public bool Unlocked = false;
}

public enum RuneType
{
    Fire = 0,
    Earth = 10,
    Air = 20,
    Water = 30,
    Life = 40,
    Death = 50,
    Force = 60,
    Time = 70,
    Space = 80,
}
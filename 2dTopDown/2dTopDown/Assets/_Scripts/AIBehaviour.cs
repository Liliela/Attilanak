using System.Collections.Generic;
using UnityEngine;

public class AIBehaviour : MonoBehaviour
{
    public List<AIBehaviour> ConnectedBehaviors;
    public bool Active;

    public virtual void Awake()
    {
        enabled = false;
    }

    public virtual void EnterBehaviour()
    {
        Active = true;
        enabled = true;
        foreach (var beh in ConnectedBehaviors)
        {
            beh.EnterBehaviour();
        }
    }

    public virtual void ExitBehaviour()
    {
        Active = false;
        enabled = false;
        foreach (var beh in ConnectedBehaviors)
        {
            beh.ExitBehaviour();
        }
    }
}
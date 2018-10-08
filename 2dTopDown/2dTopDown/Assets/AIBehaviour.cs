using System.Collections.Generic;
using UnityEngine;

public class AIBehaviour : MonoBehaviour
{
    public List<AIBehaviour> ConnectedBehaviors;

    public virtual void Awake()
    {
        enabled = false;
    }

    public virtual void EnterBehaviour()
    {
        enabled = true;
        foreach (var beh in ConnectedBehaviors)
        {
            beh.EnterBehaviour();
        }
    }

    public virtual void ExitBehaviour()
    {
        enabled = false;
        foreach (var beh in ConnectedBehaviors)
        {
            beh.ExitBehaviour();
        }
    }
}
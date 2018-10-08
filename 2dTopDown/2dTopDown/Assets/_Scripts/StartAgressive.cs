using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAgressive : AIBehaviour
{
    public override void Awake()
    {
        base.Awake();
    }

    public override void EnterBehaviour()
    {
        base.EnterBehaviour();
        BehaviourController bc = GetComponent<BehaviourController>();
        if (bc)
        {
            bc.ChangeState(AIState.Agessive);
        }
    }

    public override void ExitBehaviour()
    {
        base.ExitBehaviour();
    }
}


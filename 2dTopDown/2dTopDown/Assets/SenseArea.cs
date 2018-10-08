using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenseArea : AIBehaviour
{
    private BehaviourController _bh;
    private EnemySensor _sensor;

    private void Awake()
    {
        _bh = GetComponent<BehaviourController>();
        _sensor = GetComponentInParent<EnemySensor>();
    }

    private void Update()
    {
        if (_sensor.SensedPlayers.Count > 0)
        {
            _bh.ChangeState(AIState.Sense);
        }
    }

    public override void EnterBehaviour()
    {
        base.EnterBehaviour();
      
    }

    public override void ExitBehaviour()
    {
        base.ExitBehaviour();       
    }
}

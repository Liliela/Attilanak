using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : GeneralStatistics
{
    private BehaviourController _bh;
    private Spawner _spawner;

    protected override void Awake()
    {
        base.Awake();
        if (PhotonNetwork.isMasterClient)
            if (photonView.instantiationData != null)
            {
                _spawner = GameObject.Find(photonView.instantiationData[0].ToString()).GetComponent<Spawner>();
            }

        _bh = GetComponent<BehaviourController>();
    }

    public override void Death()
    {
        base.Death();
        _bh.Death();
        Destroy(gameObject, 3f);

        if (_spawner != null)
        {
            _spawner.RemoveEnemy(this);
        }
    }
}


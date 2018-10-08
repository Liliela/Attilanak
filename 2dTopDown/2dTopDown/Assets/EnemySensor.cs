using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySensor : MonoBehaviour
{
    public List<PhotonPlayerController> SensedPlayers;
    private BehaviourController _bh;

    private void Awake()
    {
        _bh = GetComponentInParent<BehaviourController>();
    }

    public void AddPlayer(PhotonPlayerController player)
    {
        if (!SensedPlayers.Contains(player))
        {
            SensedPlayers.Add(player);
        }
        if (SensedPlayers.Count > 0)
        {
            _bh.ChangeState(AIState.Sense);
        }
    }

    public void RemovePlayer(PhotonPlayerController player)
    {
        if (SensedPlayers.Contains(player))
        {
            SensedPlayers.Remove(player);
        }
        if (SensedPlayers.Count == 0)
        {
            _bh.ChangeState(AIState.Calm);
        }
    }
}

//[Serializable]
//public class SenseData
//{
//    PhotonPlayerController Player;
//    public bool Sensed;
//}

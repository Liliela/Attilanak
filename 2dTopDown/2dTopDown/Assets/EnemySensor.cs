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
    }

    public void RemovePlayer(PhotonPlayerController player)
    {
        if (SensedPlayers.Contains(player))
        {
            SensedPlayers.Remove(player);          
        }         
    }
}

//[Serializable]
//public class SenseData
//{
//    PhotonPlayerController Player;
//    public bool Sensed;
//}

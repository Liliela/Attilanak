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
        if (!SensedPlayers.Contains(player) && !player.GetComponent<GeneralStatistics>().Dead)
        {
            SensedPlayers.Add(player);
            player.GetComponent<GeneralStatistics>().AddMonsterSense(this);
        }
    }

    public void RemovePlayer(PhotonPlayerController player)
    {
        if (SensedPlayers.Contains(player))
        {
            player.GetComponent<GeneralStatistics>().RemoveMonsterSenser(this);
            SensedPlayers.Remove(player);
        }
    }

    public Transform GetTarget(TargetingType targetingType)
    {
        Transform Target = null;
        switch (targetingType)
        {
            case TargetingType.FirstSee:
                Target = SensedPlayers[0].transform;
                break;
            case TargetingType.Closest:
                float dist = float.PositiveInfinity;
                foreach (var sensed in SensedPlayers)
                {
                    float actDis = Vector2.Distance(sensed.transform.position, transform.position);
                    if (actDis < dist)
                    {
                        dist = actDis;
                        Target = sensed.transform;
                    }
                }
                break;
            case TargetingType.LowestHp:
                float hp = float.PositiveInfinity;

                foreach (var sensed in SensedPlayers)
                {
                    float actHp = sensed.GetComponent<GeneralStatistics>().HealthActual;
                    if (actHp < hp)
                    {
                        hp = actHp;
                        Target = sensed.transform;
                    }
                }
                break;
            case TargetingType.HighestHp:
                float hph = 0;

                foreach (var sensed in SensedPlayers)
                {
                    float actHp = sensed.GetComponent<GeneralStatistics>().HealthActual;
                    if (actHp > hph)
                    {
                        hp = actHp;
                        Target = sensed.transform;
                    }
                }
                break;
            case TargetingType.Slowest:
                float ms = float.PositiveInfinity;

                foreach (var sensed in SensedPlayers)
                {
                    float actms = sensed.GetComponent<GeneralStatistics>().MoveSpeed;
                    if (actms < ms)
                    {
                        hp = actms;
                        Target = sensed.transform;
                    }
                }
                break;
            default:
                break;
        }
        return Target;
    }
}

public enum TargetingType
{
    FirstSee,
    Closest,
    LowestHp,
    HighestHp,
    Slowest,
    //AgroMeter,
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorZone : MonoBehaviour
{
    private EnemySensor _enemy;
    public SensorZoneType SensorZoneType;
    private GeneralStatistics _stats;

    private void Awake()
    {
        _enemy = GetComponentInParent<EnemySensor>();
        _stats = GetComponentInParent<GeneralStatistics>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!PhotonNetwork.isMasterClient || _stats.Dead) return;

        switch (SensorZoneType)
        {
            case SensorZoneType.CloseAgro:
                if (collision.tag == "Player")
                {
                    _enemy.AddPlayer(collision.gameObject.GetComponentInParent<PhotonPlayerController>());
                }
                break;
            case SensorZoneType.Sense:
                if (collision.tag == "Player")
                {
                    _enemy.AddPlayer(collision.gameObject.GetComponentInParent<PhotonPlayerController>());
                }
                break;
            case SensorZoneType.Escape:
                break;
            default:
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!PhotonNetwork.isMasterClient || _stats.Dead) return;

        switch (SensorZoneType)
        {
            case SensorZoneType.CloseAgro:
                break;
            case SensorZoneType.Sense:
                break;
            case SensorZoneType.Escape:
                if (collision.tag == "Player")
                {
                    _enemy.RemovePlayer(collision.gameObject.GetComponentInParent<PhotonPlayerController>());
                }
                break;
            default:
                break;
        }
    }
}

public enum SensorZoneType
{
    CloseAgro,
    Sense,
    Escape,
}

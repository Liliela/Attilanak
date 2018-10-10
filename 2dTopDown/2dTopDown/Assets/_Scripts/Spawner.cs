using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Photon.MonoBehaviour
{
    public GameObject Prefab;
    public float spawnTime;
    public int NumberOfSpawnInWave;
    public int MaxSpawn;
    public List<Transform> SpawnLocations;
    public bool AddOriginToWanderAi = true;
    public List<EnemyController> Enemys;
    private int _locIndex;

    public void Awake()
    {
        Invoke("Spawn", spawnTime);
        _locIndex = UnityEngine.Random.Range(0, SpawnLocations.Count - 1);
    }

    private void Spawn()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            return;
        }

        object[] data = new object[1];
        data[0] = gameObject.name;

        Debug.Log("Spawn");
        if (SpawnLocations.Count == 0) return;

        for (int i = 0; i < NumberOfSpawnInWave; i++)
        {
            if (Enemys.Count >= MaxSpawn)
            {
                Invoke("Spawn", spawnTime);
                return;
            }

            Transform loc = SpawnLocations[_locIndex];
            _locIndex++;
            if (_locIndex > SpawnLocations.Count - 1)
            {
                _locIndex = 0;
            }

            GameObject go = PhotonNetwork.Instantiate("Monsters/" + Prefab.name, loc.position, loc.rotation, 0, data);

            EnemyController entitiy = go.GetComponent<EnemyController>();
            Enemys.Add(entitiy);

            if (AddOriginToWanderAi)
            {
                WanderArea wai = go.GetComponent<WanderArea>();
                if (wai != null)
                {
                    wai.OriginTransform = transform;
                }
            }
        }

        Invoke("Spawn", spawnTime);
    }

    public void RemoveEnemy(EnemyController enemy)
    {
        Enemys.Remove(enemy.GetComponent<EnemyController>());
    }

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var loc in SpawnLocations)
        {
            Gizmos.DrawLine(transform.position, loc.position);
        }
    }
}

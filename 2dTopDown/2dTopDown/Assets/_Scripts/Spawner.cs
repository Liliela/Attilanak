using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Photon.MonoBehaviour
{
    public GameObject Prefab;

    public float spawnTime;

    public int NumberOfSpawnInWave;
    public List<EnemyController> Enemys;

    public int MaxSpawn;
    private List<Transform> _spawnLocations = new List<Transform>();
    public bool AddOriginToWanderAi = true;

    public void Awake()
    {
        Invoke("Spawn", spawnTime);
        foreach (Transform child in transform)
            _spawnLocations.Add(child);
        if (_spawnLocations.Count == 0)
        {
            _spawnLocations.Add(transform);
        }
    }

    private void Spawn()
    {
        object[] data = new object[1];
        data[0] = gameObject.name;

        if (!PhotonNetwork.isMasterClient)
        {
            return;
        }
        Debug.Log("Spawn");
        if (_spawnLocations.Count == 0) return;

        for (int i = 0; i < NumberOfSpawnInWave; i++)
        {
            if (Enemys.Count >= MaxSpawn)
            {
                Invoke("Spawn", spawnTime);
                return;
            }

            Transform loc = _spawnLocations[UnityEngine.Random.Range(0, _spawnLocations.Count - 1)];

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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public string monsterPoolKey = "Enemy";
    public Transform[] spawnPoints;
    public float respawnDelay = 5f;

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private bool allEnemiesDead = false;

    private void Start()
    {
        SpawnAllEnemy();
    }

    private void Update()
    {
        if(!allEnemiesDead)
        {
            allEnemiesDead = CheckAllEnemiesDead();

            if(allEnemiesDead)
            {
                Invoke(nameof(RespawnAllEnemies), respawnDelay);
            }
        }
    }


    private void SpawnAllEnemy()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject monster = ObjectPool.Instance.SpawnFromPool(monsterPoolKey, spawnPoint.position, spawnPoint.rotation);
            spawnedEnemies.Add(monster);
        }
    }

    private bool CheckAllEnemiesDead()
    {
        for(int i = spawnedEnemies.Count - 1; i >= 0; i--)
        {
            if (spawnedEnemies[i] == null || !spawnedEnemies[i].activeInHierarchy)
            {
                spawnedEnemies.RemoveAt(i);
            }
        }

        return spawnedEnemies.Count == 0;
    }

    private void RespawnAllEnemies()
    {
        allEnemiesDead = false;
        SpawnAllEnemy();
    }
}

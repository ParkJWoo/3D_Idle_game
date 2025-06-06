using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public string enemyPoolKey = "Enemy";
    public Transform[] spawnPoints;
    public float respawnDelay = 5f;

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private bool isRespawning = false;

    private void Start()
    {
        SpawnAllEnemies();
    }

    private void Update()
    {
        if(!isRespawning && AllEnemiesAreDead())
        {
            isRespawning = true;
            Invoke(nameof(RespawnAllEnemies), respawnDelay);
        }
    }

    private void SpawnAllEnemies()
    {
        spawnedEnemies.Clear();

        foreach(Transform spawnPoint in spawnPoints)
        {
            GameObject enemy = ObjectPool.Instance.SpawnFromPool(enemyPoolKey, spawnPoint.position, spawnPoint.rotation);

            if(enemy != null)
            {
                if(enemy.TryGetComponent(out Character character))
                {
                    character.currentHP = character.maxHP;
                }

                spawnedEnemies.Add(enemy);
            }

            else
            {
                Debug.LogWarning("Ǯ���� ���͸� ������ �� �����ϴ�. Ǯ�� ũ�⸦ �÷��� �մϴ�.");
            }
        }
    }

    private bool AllEnemiesAreDead()
    {
        foreach(GameObject enemy in spawnedEnemies)
        {
            if(enemy != null && enemy.activeInHierarchy)
            {
                return false;
            }
        }

        return true;
    }

    private void RespawnAllEnemies()
    {
        isRespawning = false;
        SpawnAllEnemies();
    }
}

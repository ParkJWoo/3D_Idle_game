using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public int enemiesPerWave = 10;
    public Transform[] spawnPoints;
    public string enemyPoolTag = "Enemy";
    public float respawnDelay = 3f;

    private int currentAliveEnemies;

    private void Start()
    {
        StartCoroutine(StartWave());
    }

    private IEnumerator StartWave()
    {
        Debug.Log($"[WaveManager] 웨이브 시작: {enemiesPerWave}명 소환");

        currentAliveEnemies = 0;

        for(int i = 0; i < enemiesPerWave; i++)
        {
            Vector3 spawnPos = GetSpawnPosition(i);
            GameObject enemy = ObjectPool.Instance.SpawnFromPool(enemyPoolTag, spawnPos, Quaternion.identity);

            if(enemy == null)
            {
                Debug.LogWarning($"[WaveManager] 풀에 '{enemyPoolTag}' 태그의 적이 부족합니다!");
                continue;
            }

            //  체력 초기화
            if(enemy.TryGetComponent(out Character character))
            {
                character.ResetCharacter();
            }

            //  FSM 초기화
            if(enemy.TryGetComponent(out EnemyFSM fsm))
            {
                fsm.ResetState();
            }

            //  웨이브 매니저 전달
            if(enemy.TryGetComponent(out Enemy enemyComp))
            {
                enemyComp.waveManager = this;
            }

            currentAliveEnemies++;

            yield return new WaitForSeconds(0.1f);
        }

        Debug.Log($"[WaveManager] 현재 웨이브 적 수: {currentAliveEnemies}");

    }

    private Vector3 GetSpawnPosition(int index)
    {
        if(spawnPoints == null || spawnPoints.Length == 0)
        {
            return Vector3.zero;
        }

        return spawnPoints[index % spawnPoints.Length].position;
    }

    public void OnEnemyDied()
    {
        currentAliveEnemies--;

        if(currentAliveEnemies <= 0)
        {
            Debug.Log("[WaveManager] 웨이브 전멸 → 다음 웨이브 대기 중...");
            StartCoroutine(RespawnAfterDelay(respawnDelay));
        }
    }

    private IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        StartCoroutine(StartWave());
    }
}

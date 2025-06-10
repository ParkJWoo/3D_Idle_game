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

    //  웨이브 시작 메서드
    private IEnumerator StartWave()
    {
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

            //  웨이브 내 적들의 수를 지정한 수만큼 올려줌 → 10마리의 적을 세팅했다면, 10마리의 적이 웨이브에 나오게금 하기 위함.
            currentAliveEnemies++;

            yield return new WaitForSeconds(0.1f);
        }

    }

    //  지정한 스폰 포인트에 적들을 스폰해주는 메서드
    private Vector3 GetSpawnPosition(int index)
    {
        if(spawnPoints == null || spawnPoints.Length == 0)
        {
            return Vector3.zero;
        }

        return spawnPoints[index % spawnPoints.Length].position;
    }

    //  적들이 죽었을 때, 웨이브 내 설정한 적들의 수를 차감 → 웨이브 내 적의 수가 0 이하일 경우 RespawnAfterDelay 실행
    public void OnEnemyDied()
    {
        currentAliveEnemies--;

        if(currentAliveEnemies <= 0)
        {
            StartCoroutine(RespawnAfterDelay(respawnDelay));
        }
    }

    //  적들을 리스폰해주는 메서드
    private IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        StartCoroutine(StartWave());
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Enemy : Character
{
    [Header("Enemy Rewards")]
    public int rewardGold = 100;
    public int rewardExp = 10;

    [Header("Drop Settings")]
    [SerializeField] private List<GameObject> dropPrefabs;  // 드랍 가능한 아이템 프리팹 목록

    public WaveManager waveManager;

    public override void Die()
    {
        base.Die();

        //  보상 지급
        CharacterManager.Instance?.AddExp(rewardExp);
        CharacterManager.Instance?.AddGold(rewardGold);

        StartCoroutine(ReleaseToPoolAfterDelay(1.5f));
    }

    private IEnumerator ReleaseToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ObjectPool.Instance.ReturnToPool("Enemy", gameObject);

        //  적이 완전히 풀로 돌아간 다음에 웨이브 매니저에게 알림
        if (waveManager != null)
        {
            waveManager.OnEnemyDied();
        }
    }
}

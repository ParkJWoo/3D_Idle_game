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

    public WaveManager waveManager;

    public override void Die()
    {
        base.Die();

        //  보상 지급
        CharacterManager.Instance?.AddExp(rewardExp);
        CharacterManager.Instance?.AddGold(rewardGold);

        StartCoroutine(ReleaseToPoolAfterDelay(1.5f));
    }

    //  WaveManager에게 적이 죽었음을 알려주는 메서드 → WaveManager에서 웨이브 내 적이 죽었다는 것을 확인한 후 리스폰하게 하기 위함
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

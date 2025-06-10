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

        //  ���� ����
        CharacterManager.Instance?.AddExp(rewardExp);
        CharacterManager.Instance?.AddGold(rewardGold);

        StartCoroutine(ReleaseToPoolAfterDelay(1.5f));
    }

    //  WaveManager���� ���� �׾����� �˷��ִ� �޼��� �� WaveManager���� ���̺� �� ���� �׾��ٴ� ���� Ȯ���� �� �������ϰ� �ϱ� ����
    private IEnumerator ReleaseToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ObjectPool.Instance.ReturnToPool("Enemy", gameObject);

        //  ���� ������ Ǯ�� ���ư� ������ ���̺� �Ŵ������� �˸�
        if (waveManager != null)
        {
            waveManager.OnEnemyDied();
        }
    }
}

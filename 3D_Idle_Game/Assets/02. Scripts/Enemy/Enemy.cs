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
    [SerializeField] private List<GameObject> dropPrefabs;  // ��� ������ ������ ������ ���

    public WaveManager waveManager;

    public override void Die()
    {
        base.Die();

        //  ���� ����
        CharacterManager.Instance?.AddExp(rewardExp);
        CharacterManager.Instance?.AddGold(rewardGold);

        StartCoroutine(ReleaseToPoolAfterDelay(1.5f));
    }

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

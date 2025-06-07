using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : Character
{
    [Header("Enemy Rewards")]
    public int rewardGold = 100;
    public int rewardExp = 10;


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
    }
}

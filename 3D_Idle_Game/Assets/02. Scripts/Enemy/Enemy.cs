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

    public override void Die()
    {
        //DropRandomItems();
        
        base.Die();

        //  보상 지급
        CharacterManager.Instance?.AddExp(rewardExp);
        CharacterManager.Instance?.AddGold(rewardGold);


        StartCoroutine(ReleaseToPoolAfterDelay(1.5f));
    }

    private void DropRandomItems()
    {
        if (dropPrefabs == null || dropPrefabs.Count == 0) return;

        int index = Random.Range(0, dropPrefabs.Count);
        GameObject selectedPrefab = dropPrefabs[index];

        Instantiate(selectedPrefab, transform.position + Vector3.up * 1f, Quaternion.identity);
    }


    private IEnumerator ReleaseToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ObjectPool.Instance.ReturnToPool("Enemy", gameObject);
    }
}

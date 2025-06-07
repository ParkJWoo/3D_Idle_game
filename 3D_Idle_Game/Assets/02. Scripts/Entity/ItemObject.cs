using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemData itemData;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            bool added = Inventory.Instance.AddItem(itemData);

            if(added)
            {
                Debug.Log($"{itemData.displayName} 아이템을 인벤토리에 추가했습니다.");
                Destroy(gameObject);
            }

            else
            {
                Debug.LogWarning($"{itemData.displayName} 아이템 추가 실패 (인벤토리 공간 부족)");
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemData itemData;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (itemData == null)
        {
            Debug.LogError("ItemObject의 itemData가 null입니다!");
            return;
        }

        if (Inventory.Instance == null)
        {
            Debug.LogError("Inventory.Instance가 null입니다!");
            return;
        }

        bool added = Inventory.Instance.AddItem(itemData);

        if (added)
        {
            Debug.Log($"{itemData.displayName} 아이템 획득 성공!");
            Destroy(gameObject);
        }
    }
}

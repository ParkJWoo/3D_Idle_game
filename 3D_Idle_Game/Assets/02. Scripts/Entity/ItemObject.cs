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
            Debug.LogError("ItemObject�� itemData�� null�Դϴ�!");
            return;
        }

        if (Inventory.Instance == null)
        {
            Debug.LogError("Inventory.Instance�� null�Դϴ�!");
            return;
        }

        bool added = Inventory.Instance.AddItem(itemData);

        if (added)
        {
            Debug.Log($"{itemData.displayName} ������ ȹ�� ����!");
            Destroy(gameObject);
        }
    }
}

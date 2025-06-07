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
                Debug.Log($"{itemData.displayName} �������� �κ��丮�� �߰��߽��ϴ�.");
                Destroy(gameObject);
            }

            else
            {
                Debug.LogWarning($"{itemData.displayName} ������ �߰� ���� (�κ��丮 ���� ����)");
            }
        }
    }
}

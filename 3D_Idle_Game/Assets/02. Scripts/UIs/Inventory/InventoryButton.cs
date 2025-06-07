using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryButton : MonoBehaviour
{
    [Header("�κ��丮 �г�")]
    [SerializeField] private GameObject inventoryPanel;

    public void ToggleInventory()
    {
        if(inventoryPanel == null)
        {
            Debug.LogWarning("�κ��丮 �г��� �Ҵ���� �ʾҽ��ϴ�");
            return;
        }

        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }
}

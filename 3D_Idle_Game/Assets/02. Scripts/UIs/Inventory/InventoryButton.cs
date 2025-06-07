using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryButton : MonoBehaviour
{
    [Header("인벤토리 패널")]
    [SerializeField] private GameObject inventoryPanel;

    public void ToggleInventory()
    {
        if(inventoryPanel == null)
        {
            Debug.LogWarning("인벤토리 패널이 할당되지 않았습니다");
            return;
        }

        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }
}

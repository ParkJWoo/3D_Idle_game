using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    [Header("Data Table")]
    public ItemDataTable itemDataTable;

    [Header("Slot Settings")]
    public Transform slotParent;
    public GameObject slotPrefab;
    public int maxSlots = 3;
    public List<ItemSlot> slots = new();

    [Header("UI Panel")]
    public GameObject inventoryPanel;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDesc;
    public Button useButton;
    public Button cancelButton;

    private ItemSlot selectedSlot;

    private void Awake()
    {
        //  Instance 싱글톤 제거! 필요 시 GameManager 등에서 참조
        if(itemDataTable == null)
        {
            Debug.LogError("[Inventory] itemDataTable(SO)가 할당되지 않았습니다!"); ;
        }
    }

    private void Start()
    {
        slots = new List<ItemSlot>(slotParent.GetComponentsInChildren<ItemSlot>());
        ClearDetailPanel();

        useButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(true);
    }

    public bool AddItemByID(string itemID)
    {
        var data = GetItemData(itemID);

        if (data == null)
        {
            Debug.LogError($"[Inventory] 아이템 ID : {itemID}에 해당하는 아이템이 없습니다!");
            return false;
        }

        return AddItem(data);
    }

    public bool AddItem(ItemData data)
    {
        if(data.canStack)
        {
            foreach(var slot in slots)
            {
                if(slot.itemData == data && slot.quantity < data.maxStackAmount)
                {
                    slot.AddQuantity(1);
                    return true;
                }
            }
        }

        foreach(var slot in slots)
        {
            if(slot.itemData == null)
            {
                slot.SetItem(data);
                return true;
            }
        }

        Debug.LogWarning("[Inventory] 인벤토리가 가득찼습니다!");

        return false;
    }

    //  SO로부터 아이템을 조회하는 메서드
    public ItemData GetItemData(string itemID)
    {
        if(itemDataTable == null)
        {
            return null;
        }

        foreach(var item in itemDataTable.items)
        {
            if(item.itemID == itemID)
            {
                return item;
            }
        }

        return null;
    }

    //  플레이어가 아이템 슬롯을 선택했을 때, 하단 설명 UI에 텍스트를 띄워주는 메서드
    public void SelectSlot(ItemSlot slot)
    {
        selectedSlot = slot;

        itemName.text = slot.itemData.displayName;
        itemDesc.text = slot.itemData.description;
    }

    //  [사용] 버튼 이벤트 메서드
    public void UseButton()
    {
        if(selectedSlot == null || selectedSlot.itemData == null)
        {
            return;
        }

        if(selectedSlot.itemData.itemType != ItemType.Consumable)
        {
            Debug.LogWarning("[Inventory] 소비 아이템이 아닙니다!");
            return;
        }

        foreach(var effect in selectedSlot.itemData.consumableEffects)
        {
            switch(effect.type)
            {
                case ConsumableType.Health:
                    PlayerStat.Instance.Heal((int)effect.value);
                    break;

                case ConsumableType.Mana:
                    PlayerStat.Instance.ManaHeal((int)effect.value);
                    break;

                case ConsumableType.AttackPower:
                    PlayerStat.Instance.TemporaryAttackPowerBuff((int)effect.value, 30f);
                    break;
            }
        }

        RemoveSelectedItem();
    }

    //  아이템 사용 후 인벤토리에서 제거
    private void RemoveSelectedItem()
    {
        selectedSlot.SubtractQuantity(1);

        if(selectedSlot.quantity <= 0)
        {
            selectedSlot.Clear();
            selectedSlot = null;
        }

        else
        {
            selectedSlot.UpdateSlotUI();
        }

        ClearDetailPanel();
    }

    //  하단 UI 초기화 메서드
    private void ClearDetailPanel()
    {
        itemName.text = "";
        itemDesc.text = "";
    }

    //  [닫기] 버튼 이벤트 메서드
    public void CancelInventory()
    {
        inventoryPanel.SetActive(false);
    }
}

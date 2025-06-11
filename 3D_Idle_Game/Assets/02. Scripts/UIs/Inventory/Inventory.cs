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
        //  Instance �̱��� ����! �ʿ� �� GameManager ��� ����
        if(itemDataTable == null)
        {
            Debug.LogError("[Inventory] itemDataTable(SO)�� �Ҵ���� �ʾҽ��ϴ�!"); ;
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
            Debug.LogError($"[Inventory] ������ ID : {itemID}�� �ش��ϴ� �������� �����ϴ�!");
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

        Debug.LogWarning("[Inventory] �κ��丮�� ����á���ϴ�!");

        return false;
    }

    //  SO�κ��� �������� ��ȸ�ϴ� �޼���
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

    //  �÷��̾ ������ ������ �������� ��, �ϴ� ���� UI�� �ؽ�Ʈ�� ����ִ� �޼���
    public void SelectSlot(ItemSlot slot)
    {
        selectedSlot = slot;

        itemName.text = slot.itemData.displayName;
        itemDesc.text = slot.itemData.description;
    }

    //  [���] ��ư �̺�Ʈ �޼���
    public void UseButton()
    {
        if(selectedSlot == null || selectedSlot.itemData == null)
        {
            return;
        }

        if(selectedSlot.itemData.itemType != ItemType.Consumable)
        {
            Debug.LogWarning("[Inventory] �Һ� �������� �ƴմϴ�!");
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

    //  ������ ��� �� �κ��丮���� ����
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

    //  �ϴ� UI �ʱ�ȭ �޼���
    private void ClearDetailPanel()
    {
        itemName.text = "";
        itemDesc.text = "";
    }

    //  [�ݱ�] ��ư �̺�Ʈ �޼���
    public void CancelInventory()
    {
        inventoryPanel.SetActive(false);
    }
}

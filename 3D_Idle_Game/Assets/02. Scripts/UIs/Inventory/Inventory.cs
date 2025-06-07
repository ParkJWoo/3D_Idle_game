using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    [Header("Slot Setting")]
    public Transform slotParent;
    public GameObject slotPrefab;
    public int maxSlots = 15;
    public List<ItemSlot> slots = new();

    [Header("UI Panel")]
    public GameObject inventoryPanel;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDesc;
    public Button useButton;
    public Button equipButton;
    public Button dropButton;

    private ItemSlot selectedSlot;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //for (int i = 0; i < maxSlots; i++)
        //{
        //    GameObject go = Instantiate(slotPrefab, slotParent);
        //    ItemSlot slot = go.GetComponent<ItemSlot>();
        //    slot.index = i;
        //    slots.Add(slot);
        //    slot.Clear();
        //}

        ClearDetailPanel();
    }

    public bool AddItem(ItemData data)
    {
        if (data.canStack)
        {
            foreach (var slot in slots)
            {
                if (slot.itemData == data && slot.quantity < data.maxStackAmount)
                {
                    slot.quantity++;
                    slot.UpdateSlotUI();
                    return true;
                }
            }
        }

        foreach (var slot in slots)
        {
            if (slot.itemData == null)
            {
                slot.SetItem(data);
                return true;
            }
        }

        Debug.Log("인벤토리가 가득 찼습니다.");
        return false;
    }

    public void SelectSlot(ItemSlot slot)
    {
        selectedSlot = slot;

        itemName.text = slot.itemData.displayName;
        itemDesc.text = slot.itemData.description;

        useButton.gameObject.SetActive(slot.itemData.itemType == ItemType.Consumable);
        equipButton.gameObject.SetActive(slot.itemData.itemType == ItemType.Equipable && !slot.isEquipped);
        dropButton.gameObject.SetActive(true);
    }

    public void UseSelectedItem()
    {
        if (selectedSlot == null || selectedSlot.itemData == null) return;

        foreach (var con in selectedSlot.itemData.consumableEffects)
        {
            switch (con.type)
            {
                case ConsumableType.Health:
                    PlayerStat.Instance.Heal((int)con.value);
                    break;
                    // 다른 타입이 있다면 여기에 추가
            }
        }

        RemoveSelectedItem();
    }

    public void EquipSelectedItem()
    {
        if (selectedSlot == null || selectedSlot.itemData == null || selectedSlot.isEquipped) return;

        PlayerStat.Instance.ApplyModifiers(selectedSlot.itemData.statModifiers, selectedSlot.itemData.displayName);
        selectedSlot.isEquipped = true;
        selectedSlot.UpdateOutline();
    }

    public void DropSelectedItem()
    {
        if (selectedSlot == null || selectedSlot.itemData == null) return;

        RemoveSelectedItem();
    }

    private void RemoveSelectedItem()
    {
        selectedSlot.quantity--;
        if (selectedSlot.quantity <= 0)
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

    private void ClearDetailPanel()
    {
        itemName.text = "";
        itemDesc.text = "";
        useButton.gameObject.SetActive(false);
        equipButton.gameObject.SetActive(false);
        dropButton.gameObject.SetActive(false);
    }
}

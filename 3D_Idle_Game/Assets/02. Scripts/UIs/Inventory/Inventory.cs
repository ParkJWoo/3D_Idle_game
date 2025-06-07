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
    public Button unEquipButton;
    public Button cancelButton;

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
        slots = new List<ItemSlot>(slotParent.GetComponentsInChildren<ItemSlot>());

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

        useButton.gameObject.SetActive(false);
        equipButton.gameObject.SetActive(false);
        unEquipButton.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(true);

        if (slot.itemData.itemType == ItemType.Consumable)
        {
            useButton.gameObject.SetActive(true); // 이게 핵심
        }
        else if (slot.itemData.itemType == ItemType.Equipable)
        {
            if (slot.isEquipped)
                unEquipButton.gameObject.SetActive(true);
            else
                equipButton.gameObject.SetActive(true);
        }
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
                case ConsumableType.Mana:
                    PlayerStat.Instance.ManaHeal((int)con.value);
                    break;
                case ConsumableType.AttackPower:
                    // 햄버거 버프: 공격력 20 증가, 30초 지속
                    PlayerStat.Instance.TemporaryAttackPowerBuff((int)con.value, 30f);
                    break;
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
        cancelButton.gameObject.SetActive(false);
    }

    public void UseButton()
    {
        if (selectedSlot == null || selectedSlot.itemData == null)
        {
            Debug.LogWarning("[Inventory] 소비 아이템 사용 실패: 선택된 슬롯이 비어 있습니다.");
            return;
        }

        if (selectedSlot.itemData.itemType != ItemType.Consumable)
        {
            Debug.LogWarning("[Inventory] 소비 아이템이 아닙니다.");
            return;
        }

        // 소비형 효과 적용
        foreach (var effect in selectedSlot.itemData.consumableEffects)
        {
            switch (effect.type)
            {
                case ConsumableType.Health:
                    PlayerStat.Instance.Heal((int)effect.value);
                    break;
                case ConsumableType.Mana:
                    PlayerStat.Instance.ManaHeal((int)effect.value);
                    break;
                case ConsumableType.AttackPower:
                    PlayerStat.Instance.TemporaryAttackPowerBuff((int)effect.value, 30f);  // 30초 버프
                    break;
            }
        }

        RemoveSelectedItem();
    }

    public void EquipButton()
    {
        if (selectedSlot == null || selectedSlot.itemData == null) return;
        if (selectedSlot.itemData.itemType != ItemType.Equipable || selectedSlot.isEquipped) return;

        PlayerStat.Instance.ApplyModifiers(selectedSlot.itemData.statModifiers, selectedSlot.itemData.displayName);
        selectedSlot.isEquipped = true;
        selectedSlot.UpdateOutline();
        UpdateButtonState();
    }

    public void UnEquipButton()
    {
        if (selectedSlot == null || selectedSlot.itemData == null) return;
        if (!selectedSlot.isEquipped) return;

        PlayerStat.Instance.RemoveModifiers(selectedSlot.itemData.statModifiers, selectedSlot.itemData.displayName);
        selectedSlot.isEquipped = false;
        selectedSlot.UpdateOutline();
        UpdateButtonState();
    }

    public void CancelInventory()
    {
        inventoryPanel.SetActive(false);
    }

    private void UpdateButtonState()
    {
        if (selectedSlot == null || selectedSlot.itemData == null)
        {
            useButton.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(false);
            unEquipButton.gameObject.SetActive(false);
            return;
        }

        if (selectedSlot.itemData.itemType == ItemType.Consumable)
        {
            useButton.gameObject.SetActive(true);
            equipButton.gameObject.SetActive(false);
            unEquipButton.gameObject.SetActive(false);
        }
        else if (selectedSlot.itemData.itemType == ItemType.Equipable)
        {
            useButton.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(!selectedSlot.isEquipped);
            unEquipButton.gameObject.SetActive(selectedSlot.isEquipped);
        }
    }
}

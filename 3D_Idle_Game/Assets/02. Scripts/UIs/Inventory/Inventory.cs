using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

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
        if(Instance == null)
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

        useButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(true);
    }

    //  ������ ȹ�� ��, �κ��丮�� ���� ���� ó���ϴ� �޼���
    public bool AddItem(ItemData data)
    {
        if(data.canStack)
        {
            foreach(var slot in slots)
            {
                if(slot.itemData == data && slot.quantity < data.maxStackAmount)
                {
                    slot.quantity++;
                    slot.UpdateSlotUI();
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

        Debug.LogWarning("[Inventory] �κ��丮�� ����á���ϴ�");
        return false;
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
        if (selectedSlot == null || selectedSlot.itemData == null)
        {
            return;
        }

        if (selectedSlot.itemData.itemType != ItemType.Consumable)
        {
            Debug.LogWarning("[Inventory] �Һ� �������� �ƴմϴ�");
            return;
        }

        foreach (var effect in selectedSlot.itemData.consumableEffects)
        {
            switch (effect.type)                                        //  �Һ� �������� ������ ���� ���� �ش��ϴ� �޼��带 ȣ���Ͽ� ����.
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

        //  ����� �������� �κ��丮 ���� ������ ������ ���ҽ�Ŵ.
        RemoveSelectedItem();
    }
    
    //  ������ ������ ���� ��, �ٸ� �������� �������� ��, �ش� �������� ������ �ϴ� UI�� �ٲ㼭 ����ϱ� ���� �޼���
    private void RemoveSelectedItem()
    {
        selectedSlot.quantity--;

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

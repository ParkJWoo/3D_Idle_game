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

    //  아이템 획득 시, 인벤토리에 들어가는 것을 처리하는 메서드
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

        Debug.LogWarning("[Inventory] 인벤토리가 가득찼읍니다");
        return false;
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
        if (selectedSlot == null || selectedSlot.itemData == null)
        {
            return;
        }

        if (selectedSlot.itemData.itemType != ItemType.Consumable)
        {
            Debug.LogWarning("[Inventory] 소비 아이템이 아닙니다");
            return;
        }

        foreach (var effect in selectedSlot.itemData.consumableEffects)
        {
            switch (effect.type)                                        //  소비 아이템의 유형에 따라 각각 해당하는 메서드를 호출하여 적용.
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

        //  사용한 아이템은 인벤토리 슬롯 내에서 수량을 감소시킴.
        RemoveSelectedItem();
    }
    
    //  기존에 아이템 선택 후, 다른 아이템을 선택했을 때, 해당 아이템의 정보를 하단 UI에 바꿔서 출력하기 위한 메서드
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

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [Header("Slot Data")]
    public int index;
    public ItemData itemData { get; private set; }
    public int quantity { get; private set; }
    public bool isEquipped { get; private set; }

    [Header("UI")]
    public Image icon;
    public TextMeshProUGUI quantityText;
    public Outline outline;

    //  인벤토리 참조용
    [HideInInspector]
    public Inventory inventory;

    //  아이템 세팅 
    public void SetItem(ItemData data, int amount = 1)
    {
        itemData = data;
        quantity = amount;
        icon.sprite = data.icon;
        icon.enabled = true;
        isEquipped = false;
        UpdateSlotUI();
    }

    //  슬롯 UI 갱신
    public void UpdateSlotUI()
    {
        icon.enabled = (itemData != null);
        quantityText.text = (itemData != null && itemData.canStack && quantity > 1) ? quantity.ToString() : "";
        UpdateOutline();
    }

    //  슬롯 테두리 갱신
    public void UpdateOutline()
    {
        if (outline != null)
        {
            outline.enabled = isEquipped;
        }
    }

    //  슬롯 초기화
    public void Clear()
    {
        itemData = null;
        quantity = 0;
        isEquipped = false;
        icon.enabled = false;
        quantityText.text = "";
        UpdateOutline();
    }

    //  인벤토리에 슬롯 선택 알림
    public void OnClick()
    {
        if (inventory != null)
        {
            inventory.SelectSlot(this);
        }

        else
        {
            //  만약 인스펙터에 주입 안 되었다면 부모에서 자동 세팅을 시도한다.
            inventory = GetComponentInParent<Inventory>();

            if(inventory != null)
            {
                inventory.SelectSlot(this);
            }

            else
            {
                Debug.LogError("[ItemSlot] Inventory 참조가 없습니다!");
            }
        }
    }

    //  슬롯 내부에서 수량 감소 등 관리 필요 시 메서드 제공
    public void AddQuantity(int delta)
    {
        quantity += delta;

        if(quantity < 0)
        {
            quantity = 0;
        }

        UpdateSlotUI();
    }

    public void SubtractQuantity(int value = 1)
    {
        quantity -= value;

        if(quantity < 0)
        {
            quantity = 0;
        }

        UpdateSlotUI();
    }

    //  착용 여부 갱신
    public void SetEquipped(bool equipped)
    {
        isEquipped = equipped;
        UpdateOutline();
    }
}

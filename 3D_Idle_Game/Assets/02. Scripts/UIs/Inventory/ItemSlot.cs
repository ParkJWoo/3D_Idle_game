using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public int index;
    public ItemData itemData;
    public int quantity;
    public bool isEquipped;

    [Header("UI")]
    public Image icon;
    public TextMeshProUGUI quantityText;
    public Outline outline;

    //  아이템 세팅 메서드
    public void SetItem(ItemData data, int amount = 1)
    {
        itemData = data;
        quantity = amount;
        icon.sprite = data.icon;
        icon.enabled = true;
        UpdateSlotUI();
    }

    //  슬롯의 변경되는 것들을 갱신해주는 메서드
    public void UpdateSlotUI()
    {
        icon.enabled = (itemData != null);
        quantityText.text = (itemData != null && itemData.canStack && quantity > 1) ? quantity.ToString() : "";
        UpdateOutline();
    }

    //  슬롯 테두리 갱신 메서드
    public void UpdateOutline()
    {
        if (outline != null)
            outline.enabled = isEquipped;
    }

    //  초기화 메서드
    public void Clear()
    {
        itemData = null;
        quantity = 0;
        isEquipped = false;
        icon.enabled = false;
        quantityText.text = "";
        UpdateOutline();
    }

    //  슬롯 선택 시 인벤토리에 해당 슬롯이 선택되었음을 알리는 메서드
    public void OnClick()
    {
        Inventory.Instance.SelectSlot(this);
    }
}

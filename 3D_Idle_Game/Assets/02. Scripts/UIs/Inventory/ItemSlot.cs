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

    public void SetItem(ItemData data, int amount = 1)
    {
        itemData = data;
        quantity = amount;
        icon.sprite = data.icon;
        icon.enabled = true;
        UpdateSlotUI();
    }

    public void UpdateSlotUI()
    {
        icon.enabled = (itemData != null);
        quantityText.text = (itemData != null && itemData.canStack && quantity > 1) ? quantity.ToString() : "";
        UpdateOutline();
    }

    public void UpdateOutline()
    {
        if (outline != null)
            outline.enabled = isEquipped;
    }

    public void Clear()
    {
        itemData = null;
        quantity = 0;
        isEquipped = false;
        icon.enabled = false;
        quantityText.text = "";
        UpdateOutline();
    }

    public void OnClick()
    {
        Inventory.Instance.SelectSlot(this);
    }
}

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

    //  �κ��丮 ������
    [HideInInspector]
    public Inventory inventory;

    //  ������ ���� 
    public void SetItem(ItemData data, int amount = 1)
    {
        itemData = data;
        quantity = amount;
        icon.sprite = data.icon;
        icon.enabled = true;
        isEquipped = false;
        UpdateSlotUI();
    }

    //  ���� UI ����
    public void UpdateSlotUI()
    {
        icon.enabled = (itemData != null);
        quantityText.text = (itemData != null && itemData.canStack && quantity > 1) ? quantity.ToString() : "";
        UpdateOutline();
    }

    //  ���� �׵θ� ����
    public void UpdateOutline()
    {
        if (outline != null)
        {
            outline.enabled = isEquipped;
        }
    }

    //  ���� �ʱ�ȭ
    public void Clear()
    {
        itemData = null;
        quantity = 0;
        isEquipped = false;
        icon.enabled = false;
        quantityText.text = "";
        UpdateOutline();
    }

    //  �κ��丮�� ���� ���� �˸�
    public void OnClick()
    {
        if (inventory != null)
        {
            inventory.SelectSlot(this);
        }

        else
        {
            //  ���� �ν����Ϳ� ���� �� �Ǿ��ٸ� �θ𿡼� �ڵ� ������ �õ��Ѵ�.
            inventory = GetComponentInParent<Inventory>();

            if(inventory != null)
            {
                inventory.SelectSlot(this);
            }

            else
            {
                Debug.LogError("[ItemSlot] Inventory ������ �����ϴ�!");
            }
        }
    }

    //  ���� ���ο��� ���� ���� �� ���� �ʿ� �� �޼��� ����
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

    //  ���� ���� ����
    public void SetEquipped(bool equipped)
    {
        isEquipped = equipped;
        UpdateOutline();
    }
}

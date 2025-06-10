using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [Header("UI Referances")]
    public GameObject shopPanel;
    public Transform slotParent;
    public GameObject slotPrefabs;

    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescText;
    public Button purchaseButton;

    private ShopItemSlot selectSlot;
    public List<ItemData> itemsForSale = new List<ItemData>();
    public List<int> itemPrices = new List<int>();

    private void Start()
    {
        shopPanel.SetActive(false);
    }

    //  [����] ��ư �̺�Ʈ �޼���
    public void OpenShop()
    {
        shopPanel.SetActive(true);
        SetupShop();
    }

    //  [�ݱ�] ��ư �̺�Ʈ �޼���
    public void CloseShop()
    {
        shopPanel.SetActive(false);
        ClearSelection();
    }

    //  ���� �� ���� �ʱ�ȭ �޼���
    private void SetupShop()
    {
        //  ���� �ʱ�ȭ
        for (int i = 0; i < slotParent.childCount; i++)
        {
            Transform slotTransform = slotParent.GetChild(i);
            ShopItemSlot slot = slotTransform.GetComponent<ShopItemSlot>();

            if(i < itemsForSale.Count)
            {
                slot.Initialize(itemsForSale[i], itemPrices[i], this);
            }

            else
            {
                slot.gameObject.SetActive(false);
            }
        }
    }

    //  ���� UI �� ���Ե� �� �ϳ��� �������� ��, �ϴ� ���� UI�� �ش� �������� ������ ���� ���� �޼���
    public void SelectItem(ShopItemSlot slot)
    {
        selectSlot = slot;

        if(itemNameText != null)
        {
            itemNameText.text = slot.itemData.displayName;
        }

        if(itemDescText != null)
        {
            itemDescText.text = slot.itemData.description;
        }

        if (slot.icon != null)
        {
            slot.icon.sprite = slot.itemData.icon;
        }

        purchaseButton.interactable = true;
    }

    //  [����] ��ư �̺�Ʈ �޼���
    public void PurchaseItem()
    {
        if(selectSlot == null)
        {
            return;
        }

        int price = selectSlot.price;

        if (CharacterManager.Instance.SpendGold(price))
        {
            Inventory.Instance.AddItem(selectSlot.itemData);
        }

        else
        {
            Debug.LogWarning("[����] ��� ����!");
        }

        ClearSelection();
    }

    //  ���� ���� �ʱ�ȭ �޼���
    private void ClearSelection()
    {
        selectSlot = null;
        itemNameText.text = "";
        itemDescText.text = "";
        purchaseButton.interactable = false;
    }
}

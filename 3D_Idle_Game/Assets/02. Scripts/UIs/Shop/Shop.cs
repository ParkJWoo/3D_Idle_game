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

    [Header("Shop Inventory (SO 가능)")]
    public List<ItemData> itemsForSale = new List<ItemData>();
    public List<int> itemPrices = new List<int>();

    [Header("연동 컴포넌트")]
    public Inventory playerInventory;       //  인스펙터에서 할당할 것!

    private void Start()
    {
        shopPanel.SetActive(false);

        //  필요 시 슬롯 자동 생성
        if(slotParent.childCount < itemsForSale.Count)
        {
            for(int i = slotParent.childCount; i < itemsForSale.Count; i++)
            {
                Instantiate(slotPrefabs, slotParent);
            }
        }
    }

    //  [상점] 버튼 이벤트 메서드
    public void OpenShop()
    {
        shopPanel.SetActive(true);
        SetupShop();
    }

    //  [닫기] 버튼 이벤트 메서드
    public void CloseShop()
    {
        shopPanel.SetActive(false);
        ClearSelection();
    }

    //  상점 내 슬롯 초기화 메서드
    private void SetupShop()
    {
        //  슬롯 초기화
        for (int i = 0; i < slotParent.childCount; i++)
        {
            Transform slotTransform = slotParent.GetChild(i);
            ShopItemSlot slot = slotTransform.GetComponent<ShopItemSlot>();

            if(i < itemsForSale.Count)
            {
                slot.Initialize(itemsForSale[i], itemPrices[i], this);
                slot.gameObject.SetActive(true);
            }

            else
            {
                slot.gameObject.SetActive(false);
            }
        }
    }

    //  상점 UI 내 슬롯들 중 하나를 선택했을 때, 하단 정보 UI에 해당 아이템의 정보를 띄우기 위한 메서드
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

    //  [구매] 버튼 이벤트 메서드
    public void PurchaseItem()
    {
        if (selectSlot == null || selectSlot.itemData == null)
        {
            return;
        }

        int price = selectSlot.price;

        if(CharacterManager.Instance.SpendGold(price))
        {
            if(playerInventory == null)
            {
                Debug.LogError("[Shop] 플레이어 인벤토리가 연결되어 있지 않습니다!");
                return;
            }

            bool added = playerInventory.AddItem(selectSlot.itemData);

            if(!added)
            {
                Debug.LogWarning("[Shop] 인벤토리가 가득찼습니다!");
            }
        }

        else
        {
            Debug.LogWarning("[Shop] 골드 부족!");
        }

        ClearSelection();
    }

    //  상점 슬롯 초기화 메서드
    private void ClearSelection()
    {
        selectSlot = null;
        itemNameText.text = "";
        itemDescText.text = "";
        purchaseButton.interactable = false;
    }
}

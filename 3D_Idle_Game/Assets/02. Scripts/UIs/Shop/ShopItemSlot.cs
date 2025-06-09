using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;


public class ShopItemSlot : MonoBehaviour
{
    public Image icon;
    public ItemData itemData;
    public int price;

    private Shop shop;

    public void Initialize(ItemData data, int itemPrice, Shop shopRef)
    {
        itemData = data;
        price = itemPrice;
        shop = shopRef;

        if (icon != null && itemData.icon != null)
        {
            icon.sprite = itemData.icon;
            icon.enabled = true; 
        }
        else
        {
            Debug.LogWarning($"[ShopItemSlot] 아이콘 설정 실패! 아이템 이름: {itemData?.displayName}");
        }
    }

    public void OnClick()
    {
        if (shop != null)
        {
            shop.SelectItem(this);
        }
    }
}

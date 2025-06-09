using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton : MonoBehaviour
{
    [SerializeField] private Shop shop;

    public void OnClickShopButton()
    {
        if(shop != null)
        {
            shop.OpenShop();
        }

        else
        {
            Debug.LogWarning("[ShopButton] Shop 컴포넌트가 연결되지 않았습니다.");
        }
    }
}

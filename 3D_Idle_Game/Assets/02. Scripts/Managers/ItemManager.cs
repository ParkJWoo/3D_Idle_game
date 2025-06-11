using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public ItemDataTable itemTable;

    public ItemData GetItemByName(string name)
    {
        foreach(var item in itemTable.items)
        {
            if(item.displayName == name)
            {
                return item;
            }
        }

        Debug.LogWarning($"[ItemManager] 이름이 {name}인 아이템이 없습니다!");

        return null; 
    }
}

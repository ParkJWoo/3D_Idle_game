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

        Debug.LogWarning($"[ItemManager] �̸��� {name}�� �������� �����ϴ�!");

        return null; 
    }
}

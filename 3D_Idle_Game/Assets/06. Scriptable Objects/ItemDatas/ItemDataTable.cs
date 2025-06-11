using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataTable", menuName = "Scriptable Object/ItemDataTable", order = 1)]
public class ItemDataTable : ScriptableObject
{
    public ItemData[] items;
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    None, 
    Equipable,
    Consumable
}

public enum StatType
{
    Health, 
    Mana,
    AttackPower
}

public enum ConsumableType
{
    None, 
    Health,
    Mana
}

[Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}

[Serializable]
public class StatModifier
{
    public StatType type;
    public int value;
}

[CreateAssetMenu(fileName = "Item", menuName ="Scriptable Object/ItemData", order = 0)]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType itemType;
    public Sprite icon;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumale Effects")]
    public ItemDataConsumable[] consumableEffects;

    [Header("Equipable Stats")]
    public StatModifier[] statModifiers;
}

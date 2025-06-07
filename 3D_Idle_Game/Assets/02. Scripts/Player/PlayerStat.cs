using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public static PlayerStat Instance;

    [Header("Base HP Stats")]
    public int baseHP = 100;
    public int currentHP;
    public int maxHP;

    [Header("Base MP Stats")]
    public int baseMP = 100;
    public int currentMP;
    public int maxMP;

    [Header("Base Attack Power Stats")]
    public int baseAttackPower = 10;
    public int currentAttackPower;

    [Header("Equipped Item Display")]
    public TextMeshProUGUI equippedItemListText;
    private List<string> equippedItemNames = new List<string>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        maxHP = baseHP;
        currentHP = maxHP;
        currentAttackPower = baseAttackPower;
        currentMP = maxMP;

        CharacterManager.Instance.Player.condition.hp.maxValue = maxHP;
        CharacterManager.Instance.Player.condition.hp.Set(currentHP);
        CharacterManager.Instance.Player.condition.mp.maxValue = maxMP;
        CharacterManager.Instance.Player.condition.mp.Set(currentMP);
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        CharacterManager.Instance.Player.condition.hp.Set(currentHP);

        if(currentHP <= 0)
        {
            CharacterManager.Instance.Player.Die();
        }
    }

    //  아이템에 장착 시 해당 아이템의 스탯 추가 적용
    public void Heal(int amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        if (CharacterManager.Instance.Player != null && CharacterManager.Instance.Player.condition != null)
        {
            CharacterManager.Instance.Player.condition.hp.Set(currentHP);
        }
    }

    public void ManaHeal(int amount)
    {
        if (CharacterManager.Instance.Player != null && CharacterManager.Instance.Player.condition != null)
        {
            CharacterManager.Instance.Player.condition.mp.Add(amount); // ← 반드시 Add()
        }
    }

    public void ApplyModifiers(StatModifier[] modifiers)
    {
        ApplyModifiers(modifiers, "");
    }

    public void ApplyModifiers(StatModifier[] modifiers, string itemName)
    {
        foreach (var mod in modifiers)
        {
            switch (mod.type)
            {
                case StatType.Health:
                    maxHP += mod.value;
                    currentHP = Mathf.Clamp(currentHP, 0, maxHP);
                    CharacterManager.Instance.Player.condition.hp.maxValue = maxHP;
                    CharacterManager.Instance.Player.condition.hp.Set(currentHP);
                    break;
                case StatType.Mana:
                    maxMP += mod.value;
                    currentMP = Mathf.Clamp(currentMP, 0, maxMP);
                    CharacterManager.Instance.Player.condition.mp.maxValue = maxMP;
                    CharacterManager.Instance.Player.condition.mp.Set(currentMP);
                    break;
                case StatType.AttackPower:
                    currentAttackPower += mod.value;
                    break;
            }
        }

        if (!string.IsNullOrEmpty(itemName))
        {
            equippedItemNames.Add(itemName);
            UpdateEquippedItemText();
        }
    }

    public void RemoveModifiers(StatModifier[] modifiers)
    {
        RemoveModifiers(modifiers, "");
    }

    public void RemoveModifiers(StatModifier[] modifiers, string itemName)
    {
        foreach (var mod in modifiers)
        {
            switch (mod.type)
            {
                case StatType.Health:
                    maxHP -= mod.value;
                    maxHP = Mathf.Max(1, maxHP);
                    currentHP = Mathf.Clamp(currentHP, 0, maxHP);
                    CharacterManager.Instance.Player.condition.hp.maxValue = maxHP;
                    CharacterManager.Instance.Player.condition.hp.Set(currentHP);
                    break;
                case StatType.Mana:
                    maxMP -= mod.value;
                    maxMP = Mathf.Max(1, maxMP);
                    currentMP = Mathf.Clamp(currentMP, 0, maxMP);
                    CharacterManager.Instance.Player.condition.mp.maxValue = maxMP;
                    CharacterManager.Instance.Player.condition.mp.Set(currentMP);
                    break;
                case StatType.AttackPower:
                    currentAttackPower -= mod.value;
                    currentAttackPower = Mathf.Max(0, currentAttackPower);
                    break;
            }
        }

        if (!string.IsNullOrEmpty(itemName))
        {
            equippedItemNames.Remove(itemName);
            UpdateEquippedItemText();
        }
    }

    private void UpdateEquippedItemText()
    {
        if(equippedItemListText != null)
        {
            equippedItemListText.text = "";

            foreach(var item in equippedItemNames)
            {
                equippedItemListText.text += item + "\n";
            }
        }
    }
}

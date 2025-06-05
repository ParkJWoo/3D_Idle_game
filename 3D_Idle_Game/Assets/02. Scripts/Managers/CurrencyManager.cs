using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    public int gold = 0;
    public TextUIs uiTexts;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        else
        {
            Destroy(gameObject);
        }
    }

    public void AddGold(int amount)
    {
        gold += amount;
        uiTexts.SetGold(gold);
    }

    public bool SpendGold(int amount)
    {
        if(gold < amount)
        {
            return false;
        }

        gold -= amount;
        uiTexts.SetGold(gold);
        return true;
    }

}

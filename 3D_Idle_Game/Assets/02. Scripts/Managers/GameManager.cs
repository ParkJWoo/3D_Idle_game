using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int Gold { get; private set; } = 0;
    public int Level { get; private set; } = 1;
    public int CurrentStage { get; private set; } = 1;
    public int Exp { get; private set; } = 0;
    public int ExpToLevelUP = 100;

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

    public void AddGold(int amount)
    {
        Gold += amount;
    }

    public void AddExp(int amount)
    {
        Exp += amount;

        if(Exp >= ExpToLevelUP)
        {
            LevelUP();
        }
    }

    private void LevelUP()
    {
        Level++;
        Exp -= ExpToLevelUP;
        Debug.Log("·¹º§ ¾÷");
    }

    public void SetStage(int stage)
    {
        CurrentStage = stage;
    }
}

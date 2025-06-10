using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager instance;
    public static CharacterManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<CharacterManager>();

                if(instance == null)
                {
                    Debug.LogError("[CharacterManager] 씬에 CharacterManager가 없습니다!");
                }
            }

            return instance;
        }
    }

    [SerializeField] private Player player;
    public Player Player => player;

    private int gold;
    private int level = 1;
    private int currentExp = 0;
    private int expToNextLevel = 100;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    //  상점에서 아이템 구매 시, 아이템의 가격만큼 플레이어의 보유 골드를 차감하는 메서드
    public bool SpendGold(int amount)
    {
        if(gold >= amount)
        {
            gold -= amount;
            UIManager.Instance?.UpdateGoldText(gold);
            return true;
        }

        return false;
    }

    //  적 처치 후 플레이어의 보유 골드 수치를 올려주는 메서드
    public void AddGold(int amount)
    {
        gold += amount;
        UIManager.Instance?.UpdateGoldText(gold);
    }

    //  적 처치 후 플레이어의 보유 경험치 수치를 올려주는 메서드
    public void AddExp(int amount)
    {
        currentExp += amount;

        if(Player != null && Player.condition != null && Player.condition.uiCondition != null)
        {
            Player.condition.uiCondition.exp.Set(currentExp);       //  exp 게이지 연동
        }

        if(currentExp >= expToNextLevel)
        {
            LevelUP();
        }
    }

    //  일정 경험치에 도달했을 때 레벨을 올려주는 메서드
    private void LevelUP()
    {
        currentExp = 0;
        level++;
        expToNextLevel += 50;

        UIManager.Instance?.UpdateLevelText(level);
    }

    //  외부에서 Player를 수동 등록할 수 있도록 허용하기 위한 메서드
    public void RegisterPlayer(Player player)
    {
        this.player = player;
    }

}

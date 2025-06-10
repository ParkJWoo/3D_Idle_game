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
                    Debug.LogError("[CharacterManager] ���� CharacterManager�� �����ϴ�!");
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
    
    //  �������� ������ ���� ��, �������� ���ݸ�ŭ �÷��̾��� ���� ��带 �����ϴ� �޼���
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

    //  �� óġ �� �÷��̾��� ���� ��� ��ġ�� �÷��ִ� �޼���
    public void AddGold(int amount)
    {
        gold += amount;
        UIManager.Instance?.UpdateGoldText(gold);
    }

    //  �� óġ �� �÷��̾��� ���� ����ġ ��ġ�� �÷��ִ� �޼���
    public void AddExp(int amount)
    {
        currentExp += amount;

        if(Player != null && Player.condition != null && Player.condition.uiCondition != null)
        {
            Player.condition.uiCondition.exp.Set(currentExp);       //  exp ������ ����
        }

        if(currentExp >= expToNextLevel)
        {
            LevelUP();
        }
    }

    //  ���� ����ġ�� �������� �� ������ �÷��ִ� �޼���
    private void LevelUP()
    {
        currentExp = 0;
        level++;
        expToNextLevel += 50;

        UIManager.Instance?.UpdateLevelText(level);
    }

    //  �ܺο��� Player�� ���� ����� �� �ֵ��� ����ϱ� ���� �޼���
    public void RegisterPlayer(Player player)
    {
        this.player = player;
    }

}

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

    //  �ܺο��� Player�� ���� ����� �� �ֵ��� ����ϱ� ���� �޼���
    public void RegisterPlayer(Player player)
    {
        this.player = player;
    }

}

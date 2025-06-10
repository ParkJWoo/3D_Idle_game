using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextUIs textUIs;

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

    //  골드 획득 시 Gold Text UI를 갱신하는 메서드
    public void UpdateGoldText(int gold)
    {
        if(textUIs != null)
        {
            textUIs.SetGold(gold);
        }
    }

    //  레벨 업 시 Level Text UI를 갱신하는 메서드
    public void UpdateLevelText(int level)
    {
        if (textUIs != null)
        {
            textUIs.SetLevel(level);
        }
    }
}

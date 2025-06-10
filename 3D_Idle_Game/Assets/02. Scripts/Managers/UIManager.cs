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

    //  ��� ȹ�� �� Gold Text UI�� �����ϴ� �޼���
    public void UpdateGoldText(int gold)
    {
        if(textUIs != null)
        {
            textUIs.SetGold(gold);
        }
    }

    //  ���� �� �� Level Text UI�� �����ϴ� �޼���
    public void UpdateLevelText(int level)
    {
        if (textUIs != null)
        {
            textUIs.SetLevel(level);
        }
    }
}

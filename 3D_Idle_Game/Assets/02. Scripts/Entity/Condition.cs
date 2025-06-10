using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public int curValue;
    public int startValue;
    public int maxValue;
    public float passiveValue;  //  ���� ȸ�� � ���
    public Image uiBar;
    public UICondition uiCondition; //  UI ������
    public CharacterData characterData;

    private void Start()
    {
        if(characterData != null)
        {
            maxValue = characterData.maxHP;
            startValue = characterData.maxHP;
            curValue = characterData.maxHP;
        }

        else
        {
            Debug.LogWarning("[Condition] CharacterData�� �������� �ʾҽ��ϴ�.");
        }

        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    //  ���ϴ� UI�� ���µ��� �����ϴ� �޼���
    private void UpdateUI()
    {
        if(uiBar != null)
        {
            uiBar.fillAmount = GetPercentage();
        }
    }

    private float GetPercentage()
    {
        return (float)curValue / maxValue;
    }

    //  ü��, ����, ����ġ ���� ��ġ�� �÷��ִ� �޼���
    public void Add(int value)
    {
        curValue = Mathf.Min(curValue + value, maxValue);
        UpdateUI();
    }

    //  ü��, ����, ����ġ ���� ��ġ�� ��� �޼���
    public void Subtract(int value)
    {
        curValue = Mathf.Max(curValue - value, 0);
        UpdateUI();
    }

    //  ü��, ����, ����ġ���� ��ġ�� �����ϴ� �޼���
    public void Set(int value)
    {
        curValue = Mathf.Clamp(value, 0, maxValue);
        UpdateUI();
    }
}

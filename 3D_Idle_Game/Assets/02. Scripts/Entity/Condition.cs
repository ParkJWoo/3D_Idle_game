using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public int curValue;
    public int startValue;
    public int maxValue;
    public float passiveValue;  //  마나 회복 등에 사용
    public Image uiBar;
    public UICondition uiCondition; //  UI 연동용
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
            Debug.LogWarning("[Condition] CharacterData가 설정되지 않았습니다.");
        }

        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    //  변하는 UI의 상태들을 갱신하는 메서드
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

    //  체력, 마나, 경험치 등의 수치를 올려주는 메서드
    public void Add(int value)
    {
        curValue = Mathf.Min(curValue + value, maxValue);
        UpdateUI();
    }

    //  체력, 마나, 경험치 등의 수치를 깎는 메서드
    public void Subtract(int value)
    {
        curValue = Mathf.Max(curValue - value, 0);
        UpdateUI();
    }

    //  체력, 마나, 경험치들의 수치를 조정하는 메서드
    public void Set(int value)
    {
        curValue = Mathf.Clamp(value, 0, maxValue);
        UpdateUI();
    }
}

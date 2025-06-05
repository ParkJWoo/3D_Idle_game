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

    private void Start()
    {
        curValue = startValue;
    }

    private void Update()
    {
        uiBar.fillAmount = GetPercentage();
    }

    private float GetPercentage()
    {
        return (float)curValue / maxValue;
    }

    public void Add(int value)
    {
        curValue = Mathf.Min(curValue + value, maxValue);
    }

    public void Subtract(int value)
    {
        curValue = Mathf.Max(curValue - value, 0);
    }

    public void Set(int value)
    {
        curValue = Mathf.Clamp(value, 0, maxValue);
    }
}

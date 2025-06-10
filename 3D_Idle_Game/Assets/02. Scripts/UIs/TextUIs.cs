using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextUIs : MonoBehaviour
{
    [Header("Text UIs")]
    [SerializeField] private TextMeshProUGUI stageText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI levelText;

    private void Start()
    {
    }

    //  ��� UI ���� ���� �޼���
    public void SetGold(int gold)
    {
        if (goldText != null)
        {
            goldText.text = $"Gold: {gold}";
        }
    }

    //  ���� UI ���� ���� �޼���
    public void SetLevel(int level)
    {
        if(levelText != null)
        {
            levelText.text = $"Lv. {level}";
        }
    }
}

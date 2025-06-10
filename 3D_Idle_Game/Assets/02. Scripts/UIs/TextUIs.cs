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

    public void SetStage(int stageNum)
    {
        if(stageText != null)
        {
            stageText.text = $"Stage {stageNum}";
        }
    }

    public void SetGold(int gold)
    {
        if (goldText != null)
        {
            goldText.text = $"Gold: {gold}";
        }
    }

    public void SetLevel(int level)
    {
        if(levelText != null)
        {
            levelText.text = $"Lv. {level}";
        }
    }
}

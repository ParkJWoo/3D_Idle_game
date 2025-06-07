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
    [SerializeField] private TextMeshProUGUI equippedItemListText;

    private void Start()
    {
        //  PlayerStat이 존재하면 연결해준다!
        if(PlayerStat.Instance != null)
        {
            PlayerStat.Instance.equippedItemListText = equippedItemListText;
        }
    }

    public void SetStage(int stageNum)
    {
        stageText.text = $"Stage {stageNum}";
    }

    public void SetGold(int gold)
    {
        goldText.text = $"Gold: {gold}";
    }

    public void SetLevel(int level)
    {
        levelText.text = $"Lv. {level}";
    }
}

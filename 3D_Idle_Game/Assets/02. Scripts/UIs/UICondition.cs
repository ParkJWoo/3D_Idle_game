using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition hp;
    public Condition mp;
    public Condition exp;

    private void Start()
    {
        if (CharacterManager.Instance == null)
        {
            Debug.LogError("CharacterManager.Instance is null!");
        }

        else if (CharacterManager.Instance.Player == null)
        {
            Debug.LogError("CharacterManager.Instance.Player is null!");
        }

        else if (CharacterManager.Instance.Player.condition == null)
        {
            Debug.LogError("CharacterManager.Instance.Player.condition is null!");

        }

        else
        {
            CharacterManager.Instance.Player.condition.uiCondition = this;
        }
    }

}

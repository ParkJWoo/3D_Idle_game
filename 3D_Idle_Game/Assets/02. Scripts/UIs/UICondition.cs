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
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }

}

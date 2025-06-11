using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : Character
{
    public PlayerCondition condition;
    public ItemData itemData;
    public Action addItem;

    private void Awake()
    {
        condition = GetComponent<PlayerCondition>();

        if(condition == null)
        {
            Debug.LogError("Player¿¡ PlayerCondition ÄÄÆ÷³ÍÆ®°¡ ¾ø½À´Ï´Ù!");
        }

        //  ½Ì±ÛÅæ¿¡ µî·Ï
        CharacterManager.Instance.RegisterPlayer(this);

        condition = GetComponent<PlayerCondition>();
    }

    public override void Die()
    {
        Debug.Log("[Player] »ç¸Á Ã³¸®");

        base.Die();
    }
}

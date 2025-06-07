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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : Character
{
    public PlayerCondition condition;
    //public ItemData itemDAta;
    public Action addItem;

    private void Awake()
    {
        //  �̱��濡 ���
        CharacterManager.Instance.RegisterPlayer(this);

        condition = GetComponent<PlayerCondition>();
    }

    protected override void Die()
    {
        Debug.Log("[Player] ��� ó��");

        base.Die();
    }
}

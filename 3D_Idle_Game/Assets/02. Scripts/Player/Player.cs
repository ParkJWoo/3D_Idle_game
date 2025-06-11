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
            Debug.LogError("Player�� PlayerCondition ������Ʈ�� �����ϴ�!");
        }

        //  �̱��濡 ���
        CharacterManager.Instance.RegisterPlayer(this);

        condition = GetComponent<PlayerCondition>();
    }

    public override void Die()
    {
        Debug.Log("[Player] ��� ó��");

        base.Die();
    }
}

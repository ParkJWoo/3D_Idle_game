using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public PlayerCondition condition;
    //public ItemData itemDAta;
    public Action addItem;

    private void Awake()
    {
        //  ½Ì±ÛÅæ¿¡ µî·Ï
        CharacterManager.Instance.RegisterPlayer(this);

        condition = GetComponent<PlayerCondition>();
    }
}

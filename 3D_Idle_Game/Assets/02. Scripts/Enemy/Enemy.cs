using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [Header("Enemy Rewards")]
    public int rewardGold = 100;
    public int rewardExp = 10;

    protected override void Die()
    {
        base.Die();

        Debug.Log("[Enemy] »ç¸Á Ã³¸®");
        Debug.Log($"¢º °ñµå +{rewardGold}, °æÇèÄ¡ +{rewardExp}");
    }
}

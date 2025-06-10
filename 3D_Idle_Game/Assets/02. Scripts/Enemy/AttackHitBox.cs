using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponentInParent<Player>();

        //  플레이어가 맞았을 때, 데미지 차감
        if(player != null)
        {
            player.TakeDamage(10);
        }
    }
}

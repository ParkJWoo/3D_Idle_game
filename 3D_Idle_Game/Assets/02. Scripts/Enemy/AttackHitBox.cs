using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponentInParent<Player>();

        if(player != null)
        {
            player.TakeDamage(10);
        }
    }
}

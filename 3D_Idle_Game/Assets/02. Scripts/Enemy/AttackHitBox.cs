using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger 발생 : {other.name}");

        var player = other.GetComponentInParent<Player>();

        if(player != null)
        {
            Debug.Log("플레이어 감지됨!");
            player.TakeDamage(10);
        }
    }
}

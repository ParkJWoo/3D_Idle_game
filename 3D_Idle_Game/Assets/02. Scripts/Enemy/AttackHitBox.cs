using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger �߻� : {other.name}");

        var player = other.GetComponentInParent<Player>();

        if(player != null)
        {
            Debug.Log("�÷��̾� ������!");
            player.TakeDamage(10);
        }
    }
}

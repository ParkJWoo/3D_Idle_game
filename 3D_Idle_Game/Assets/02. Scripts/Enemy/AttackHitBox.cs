using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Character target = other.GetComponent<Character>();
            if (target != null)
            {
                target.TakeDamage(10); // 고정 데미지
            }
        }
    }
}

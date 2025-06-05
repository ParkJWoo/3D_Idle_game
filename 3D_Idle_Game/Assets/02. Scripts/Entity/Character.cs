using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [Header("Character Stats")]
    public int maxHP = 100;
    public int currentHP;

    protected Animator animator;

    protected virtual void Start()
    {
        currentHP = maxHP;
        animator = GetComponent<Animator>();
    }

    public virtual void TakeDamage(int amount)
    {
        currentHP -= amount;
        Debug.Log($"[{gameObject.name}] 피해 입음: {amount}, 남은 HP: {currentHP}");

        if(currentHP <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log($"[{gameObject.name}] 사망 처리");

        if(animator != null)
        {
            animator.SetTrigger("Die");
            Destroy(gameObject, 1.5f);
        }

        else
        {
            Destroy(gameObject);
        }
    }
}

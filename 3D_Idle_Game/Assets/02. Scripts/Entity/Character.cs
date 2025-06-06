using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [Header("Character Data")]
    public CharacterData characterData;

    protected int maxHP;
    protected int currentHP;
    protected int attackPower;

    protected Animator animator;

    protected virtual void Start()
    {
        if(characterData != null)
        {
            maxHP = characterData.maxHP;
            attackPower = characterData.attackPower;
        }

        else
        {
            Debug.LogWarning($"{gameObject.name}의 CharacterData가 설정되지 않았습니다.");
            maxHP = 100;
            attackPower = 10;
        }

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

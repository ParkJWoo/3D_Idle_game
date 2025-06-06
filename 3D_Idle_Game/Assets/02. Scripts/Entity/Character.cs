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
            Debug.LogWarning($"{gameObject.name}�� CharacterData�� �������� �ʾҽ��ϴ�.");
            maxHP = 100;
            attackPower = 10;
        }

        currentHP = maxHP;
        animator = GetComponent<Animator>();
    }

    public virtual void TakeDamage(int amount)
    {
        currentHP -= amount;
        Debug.Log($"[{gameObject.name}] ���� ����: {amount}, ���� HP: {currentHP}");

        if(currentHP <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log($"[{gameObject.name}] ��� ó��");
        
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

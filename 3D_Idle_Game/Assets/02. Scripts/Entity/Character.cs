using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [Header("Character Data")]
    public CharacterData characterData;

    [HideInInspector] public int maxHP;
    [HideInInspector] public int currentHP;
    public int attackPower;

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

        //  플레이어의 경우: UI도 갱신
        if(this is Player player && player.condition != null && player.condition.hp != null)
        {
            player.condition.hp.Subtract(amount);
        }

        if(currentHP <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        if(animator != null)
        {
            animator.SetTrigger("IsDead");
            Destroy(gameObject, 1.5f);
        }

        else
        {
            Destroy(gameObject);
        }
    }
}

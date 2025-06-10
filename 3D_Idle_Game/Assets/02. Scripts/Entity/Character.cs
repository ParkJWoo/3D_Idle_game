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
    protected bool isDead = false;

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
        if(isDead)
        {
            return;
        }

        isDead = true;

        if(animator != null)
        {
            animator.SetTrigger("IsDead");
        }
    }

    public virtual void ResetCharacter()
    {
        if(characterData != null)
        {
            maxHP = characterData.maxHP;
            attackPower = characterData.attackPower;
        }

        currentHP = maxHP;
        isDead = false;

        gameObject.SetActive(true);

        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if(animator != null)
        {
            animator.Rebind();          //  애니메이션 초기화
            animator.Update(0f);
        }

        EnemyFSM enemyFSM = GetComponent<EnemyFSM>();

        if(enemyFSM != null)
        {
            enemyFSM.ResetState();
        }
    }
}

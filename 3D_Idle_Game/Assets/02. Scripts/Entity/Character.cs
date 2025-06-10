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
            Debug.LogWarning($"{gameObject.name}�� CharacterData�� �������� �ʾҽ��ϴ�.");
            maxHP = 100;
            attackPower = 10;
        }

        currentHP = maxHP;
        animator = GetComponent<Animator>();
    }

    //  ������ ��� �޼���
    public virtual void TakeDamage(int amount)
    {
        currentHP -= amount;

        //  �÷��̾��� ���: UI�� ����
        if(this is Player player && player.condition != null && player.condition.hp != null)
        {
            player.condition.hp.Subtract(amount);
        }

        if(currentHP <= 0)
        {
            Die();
        }
    }

    //  ��� ó�� �޼���
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

    //  ĳ���͸� �ʱ�ȭ�ϴ� �޼��� �� ������ ���, ���� �� ������ �� 0 ���Ϸ� �Ǿ��ִ� ������ ü�� ���� �ʱ�ȭ�Ͽ� �����ϵ��� �ϱ� ���� �޼����Դϴ�.
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
            animator.Rebind();          //  �ִϸ��̼� �ʱ�ȭ
            animator.Update(0f);
        }

        EnemyFSM enemyFSM = GetComponent<EnemyFSM>();

        if(enemyFSM != null)
        {
            enemyFSM.ResetState();
        }
    }
}

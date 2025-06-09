using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    Move,
    Attack
}

public class EnemyFSM : MonoBehaviour
{
    private EnemyState currentState;

    private NavMeshAgent agent;
    private Animator animator;
    private PlayerFSM playerFSM;
    public float attackRange = 1.5f;
    public float attackDelay = 2f;
    private float lastAttackTime;

    [Header("Weapon")]
    public GameObject attackHitBox;     //  칼에 달린 히트박스

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerFSM = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerFSM>();
        currentState = EnemyState.Idle;

        StartCoroutine(CheckState());
        StartCoroutine(ActionState());
    }

    private IEnumerator CheckState()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while(true)
        {
            if(playerFSM == null || playerFSM.IsDead())
            {
                currentState = EnemyState.Idle;
                agent.ResetPath();
                yield break;
            }

            //  상태가 추적/공격일 때만 거리를 검사한다.
            if(currentState == EnemyState.Move || currentState == EnemyState.Attack)
            {
                float distance = Vector3.Distance(transform.position, playerFSM.transform.position);

                if (distance > attackRange)
                {
                    currentState = EnemyState.Move;
                }

                else
                {
                    currentState = EnemyState.Attack;
                }
            }

            yield return wait;
        }
    }

    private IEnumerator ActionState()
    {
        while(true)
        {
            switch(currentState)
            {
                case EnemyState.Idle:
                    animator.SetBool("IsMoving", false);
                    agent.ResetPath();
                    break;

                case EnemyState.Move:

                    if(playerFSM != null && !playerFSM.IsDead())
                    {
                        agent.SetDestination(playerFSM.transform.position);
                        animator.SetBool("IsMoving", true);
                    }
                    break;

                case EnemyState.Attack:

                    if(Time.time - lastAttackTime >= attackDelay)
                    {
                        transform.LookAt(playerFSM.transform);
                        lastAttackTime = Time.time;
                        animator.SetTrigger("Attack");
                        StartCoroutine(EnableHitbox());
                    }

                    break;
            }

            yield return null;
        }
    }

    public void DamageAndChase()
    {
        if(playerFSM == null)
        {
            playerFSM = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerFSM>();
        }

        if(playerFSM != null && !playerFSM.IsDead())
        {
            currentState = EnemyState.Move;
        }
    }

    private IEnumerator EnableHitbox()
    {
        if (attackHitBox != null)
        {
            Collider collider = attackHitBox.GetComponent<Collider>();
            collider.enabled = true;
            yield return new WaitForSeconds(0.3f);
            collider.enabled = false;
        }
    }

    // 애니메이션 이벤트에서 호출되어도 에러 방지
    public void DealDamage() { }
}

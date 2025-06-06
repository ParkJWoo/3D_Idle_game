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
        currentState = EnemyState.Move;
        StartCoroutine(StateLoop());
    }

    private IEnumerator StateLoop()
    {
        while (true)
        {
            if (playerFSM == null || playerFSM.IsDead())
            {
                currentState = EnemyState.Idle;
                agent.ResetPath();
                yield break;
            }

            switch (currentState)
            {
                case EnemyState.Idle:
                    animator.SetBool("IsMoving", false);
                    break;

                case EnemyState.Move:
                    agent.SetDestination(playerFSM.transform.position);
                    animator.SetBool("IsMoving", true);

                    if (Vector3.Distance(transform.position, playerFSM.transform.position) <= attackRange)
                    {
                        agent.ResetPath();
                        animator.SetBool("IsMoving", false);
                        currentState = EnemyState.Attack;
                    }
                    break;

                case EnemyState.Attack:
                    transform.LookAt(playerFSM.transform);

                    if (Time.time - lastAttackTime >= attackDelay)
                    {
                        lastAttackTime = Time.time;
                        animator.SetTrigger("Attack");
                        StartCoroutine(EnableHitbox());
                    }

                    if (Vector3.Distance(transform.position, playerFSM.transform.position) > attackRange)
                    {
                        currentState = EnemyState.Move;
                    }
                    break;
            }

            yield return null;
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

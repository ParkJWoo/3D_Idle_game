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
    public GameObject attackHitBox;

    private bool isFSMRunning = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        //  FSM 초기화는 피격 시까지 보류
        currentState = EnemyState.Idle;
        isFSMRunning = true;

        if(playerFSM == null)
        {
            playerFSM = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerFSM>();
        }

    }

    private void OnDisable()
    {
        StopAllCoroutines();
        isFSMRunning = false;
    }

    private IEnumerator CheckState()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            if (playerFSM == null || playerFSM.IsDead())
            {
                currentState = EnemyState.Idle;
                agent.ResetPath();
                yield break;
            }

            float distance = Vector3.Distance(transform.position, playerFSM.transform.position);

            if (distance > attackRange)
            {
                currentState = EnemyState.Move;
            }

            else
            {
                currentState = EnemyState.Attack;
            }

            yield return wait;
        }
    }

    private IEnumerator ActionState()
    {
        while (true)
        {
            switch (currentState)
            {
                case EnemyState.Idle:
                    agent.ResetPath();
                    animator.SetBool("IsMoving", false);
                    break;

                case EnemyState.Move:
                    if (playerFSM != null && !playerFSM.IsDead())
                    {
                        agent.SetDestination(playerFSM.transform.position);
                        animator.SetBool("IsMoving", true);
                    }
                    break;

                case EnemyState.Attack:
                    if (Time.time - lastAttackTime >= attackDelay)
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

    //  플레이어가 쏜 화살이 맞으면은 FSM 실행!
    public void DamageAndChase()
    {
        if(isFSMRunning)
        {
            return;
        }

        if(playerFSM == null)
        {
            playerFSM = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerFSM>();
        }

        if(playerFSM != null && !playerFSM.IsDead())
        {
            currentState = EnemyState.Move;
            StartCoroutine(CheckState());
            StartCoroutine(ActionState());

            isFSMRunning = true;
        }
    }

    public void ResetState()
    {
        currentState = EnemyState.Idle;

        if (agent != null)
        {
            agent.ResetPath();
            agent.isStopped = false;
        }

        if (animator != null)
        {
            animator.SetBool("IsMoving", false);
        }

        isFSMRunning = false;
    }

    // 애니메이션 이벤트 호출용
    public void DealDamage() { }
}

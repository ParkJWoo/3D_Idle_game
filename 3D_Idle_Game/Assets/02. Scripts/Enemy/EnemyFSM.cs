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

    //  상태를 체크하는 메서드
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

    //  상태 전환을 실행하는 메서드
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

    //  적에게 달린 히트 박스 → 적이 공격 시 달려있는 히트 박스도 공격 궤적에 같이 움직이도록 하기 위함.
    private IEnumerator EnableHitbox()
    {
        if (attackHitBox != null)
        {
            Collider collider = attackHitBox.GetComponent<Collider>();
            collider.enabled = true;                                    //  휘두를 때 히트 박스 켜둠
            yield return new WaitForSeconds(0.3f);
            collider.enabled = false;                                   //  휘두름이 끝난 후 히트 박스 꺼둠
        }
    }

    //  플레이어가 쏜 화살에 맞으면 플레이어 추격을 시작하는 메서드
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

        if(playerFSM != null && !playerFSM.IsDead())        //  추격하고자 하는 플레이어가 존재할 때 추격 시작
        {
            currentState = EnemyState.Move;
            StartCoroutine(CheckState());
            StartCoroutine(ActionState());

            isFSMRunning = true;
        }
    }

    //  FSM 초기화 메서드 → 오브젝트 풀링을 사용하여 적을 생성하기 때문에 리스폰 시 FSM 초기화 진행!
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

    // 애니메이션 이벤트 호출용   → 사용하지 않는 메서드 (없으면 사전에 작업된 애니메이션 쪽에서 에러가 발생해서 추가해두었습니다)
    public void DealDamage() { }
}

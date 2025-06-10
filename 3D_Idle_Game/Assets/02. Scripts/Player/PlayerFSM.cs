using System.Collections;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public enum PlayerState
{
    Idle,
    Move,
    Attack,
    Skill
}

public class PlayerFSM : MonoBehaviour
{
    [Header("Player Settings")]
    public float attackRange = 1.5f;
    public float attackDelay = 1f;
    public float skillDelay = 5f;
    public float skillManaCost = 10f;

    [Header("Referances")]
    public GameObject arrowPrefab;
    public GameObject skillProjectilePrefab;
    public Transform firePoint;
    public Condition hpCondition;
    public Condition mpCondition;

    private NavMeshAgent agent;
    private Animator animator;
    private Transform currentTarget;

    private float lastAttackTime;
    private float lastSkillTime;

    private PlayerState currentState;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        lastAttackTime = Time.time - attackDelay;
        lastSkillTime = Time.time - skillDelay;
        currentState = PlayerState.Idle;

        StartCoroutine(CheckState());
        StartCoroutine(ActionState());
    }

    //  상태를 체크하는 메서드
    private IEnumerator CheckState()
    {
        var wait = new WaitForSeconds(0.1f);

        while (true)
        {
            FindClosestTarget();

            if (currentTarget == null)
            {
                currentState = PlayerState.Idle;
            }

            else
            {
                float distance = Vector3.Distance(transform.position, currentTarget.position);

                currentState = distance > attackRange + agent.stoppingDistance ? PlayerState.Move : PlayerState.Attack;
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
                case PlayerState.Idle:
                    HandleIdle();
                    break;

                case PlayerState.Move:
                    HandleMove();
                    break;

                case PlayerState.Attack:
                    HandleAttack();
                    break;

                case PlayerState.Skill:
                    UseSkill();
                    lastSkillTime = Time.time;
                    currentState = PlayerState.Attack;
                    break;
            }

            yield return null;
        }
    }

    //  Idle 상태 관리 메서드
    private void HandleIdle()
    {
        animator.SetBool("IsMoving", false);
        animator.SetFloat("MoveSpeed", 0f);
        agent.ResetPath();
    }

    //  Move 상태 관리 메서드
    private void HandleMove()
    {
        if (currentTarget == null)
        {
            return;
        }

        agent.SetDestination(currentTarget.position);           //  가까이 있는 적을 추적
        animator.SetBool("IsMoving", true);
        animator.SetFloat("MoveSpeed", agent.velocity.magnitude);
    }

    //  Attack 상태 관리 메서드
    private void HandleAttack()
    {
        if (currentTarget == null)
        {
            return;
        }

        agent.ResetPath();                                      //  적 추적 정지
        animator.SetBool("IsMoving", false);
        transform.LookAt(currentTarget);                        //  적을 향해 바라보면서

        if (Time.time - lastAttackTime >= attackDelay)          //  공격 시행
        {
            lastAttackTime = Time.time;
            animator.SetTrigger("Attack");
            StartCoroutine(DelayFire());
        }

        if (Time.time - lastSkillTime >= skillDelay && mpCondition.curValue >= skillManaCost)       //  스킬의 경우, 마나가 부족하지 않은 한 일정 시간에 한 번 시행하도록 구현했습니다.
        {
            currentState = PlayerState.Skill;
        }
    }

    //  공격 후 즉시 발사하지 않고 일정 딜레이 후 다시 발사하도록 하는 메서드
    private IEnumerator DelayFire()
    {
        yield return new WaitForSeconds(0.3f);
        FireArrow();
    }

    //  가장 가까이 있는 적을 탐색하는 메서드
    private void FindClosestTarget()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if(enemies.Length == 0)
        {
            currentTarget = null;
            return;
        }

        currentTarget = enemies.Where(e => e.activeInHierarchy).OrderBy(e => Vector3.Distance(transform.position, e.transform.position)).FirstOrDefault()?.transform;
    }

    //  화살 발사 메서드
    private void FireArrow()
    {
        if(currentTarget == null || firePoint == null)
        {
            return;
        }

        Vector3 direction = (currentTarget.position - firePoint.position).normalized;
        Arrow.SpawnAndFire(firePoint.position, Quaternion.LookRotation(direction), direction, 20f);
    }

    //  스킬 사용 메서드
    private void UseSkill()
    {
        if (mpCondition.curValue < skillManaCost)
        {
            return;
        }

        mpCondition.Subtract((int)skillManaCost);

        for (int i = -2; i <= 2; i++)
        {
            GameObject arrow = Instantiate(skillProjectilePrefab, firePoint.position, Quaternion.identity); //  화살 여러 발을 부채꼴 형식으로 발사하는 스킬
            arrow.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + i * 10f, 0f);
            arrow.GetComponent<Rigidbody>().velocity = arrow.transform.forward * 15f;
        }
    }

    public bool IsDead() => hpCondition.curValue <= 0;

    //  애니메이션 이벤트용 (쓰지 않음)
    public void FireProjectile() { }
}

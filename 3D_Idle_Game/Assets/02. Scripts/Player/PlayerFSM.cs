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

    private void HandleIdle()
    {
        animator.SetBool("IsMoving", false);
        animator.SetFloat("MoveSpeed", 0f);
        agent.ResetPath();
    }

    private void HandleMove()
    {
        if (currentTarget == null)
        {
            return;
        }

        agent.SetDestination(currentTarget.position);
        animator.SetBool("IsMoving", true);
        animator.SetFloat("MoveSpeed", agent.velocity.magnitude);
    }

    private void HandleAttack()
    {
        if (currentTarget == null)
        {
            return;
        }

        agent.ResetPath();
        animator.SetBool("IsMoving", false);
        transform.LookAt(currentTarget);

        if (Time.time - lastAttackTime >= attackDelay)
        {
            lastAttackTime = Time.time;
            animator.SetTrigger("Attack");
            StartCoroutine(DelayFire());
        }

        if (Time.time - lastSkillTime >= skillDelay && mpCondition.curValue >= skillManaCost)
        {
            currentState = PlayerState.Skill;
        }
    }

    private IEnumerator DelayFire()
    {
        yield return new WaitForSeconds(0.3f);
        FireArrow();
    }

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

    private void FireArrow()
    {
        if(currentTarget == null || firePoint == null)
        {
            return;
        }

        Vector3 direction = (currentTarget.position - firePoint.position).normalized;
        Arrow.SpawnAndFire(firePoint.position, Quaternion.LookRotation(direction), direction, 20f);
    }

    private void UseSkill()
    {
        if (mpCondition.curValue < skillManaCost)
        {
            return;
        }

        mpCondition.Subtract((int)skillManaCost);

        for (int i = -2; i <= 2; i++)
        {
            GameObject arrow = Instantiate(skillProjectilePrefab, firePoint.position, Quaternion.identity);
            arrow.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + i * 10f, 0f);
            arrow.GetComponent<Rigidbody>().velocity = arrow.transform.forward * 15f;
        }
    }

    public bool IsDead() => hpCondition.curValue <= 0;

    //  애니메이션 이벤트용 (쓰지 않음)
    public void FireProjectile() { }
}

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
    private PlayerState currentState;

    public float attackRange = 2f;
    public float attackDelay = 1f;
    public float skillDelay = 5f;
    public float skillManaCost = 10f;

    private float lastAttackTime;
    private float lastSkillTime;

    private NavMeshAgent agent;
    private Animator animator;

    public Transform currentTarget;
    public Condition hpCondition;
    public Condition mpCondition;

    public GameObject arrowPrefab;
    public GameObject skillProjectilePrefab;
    public Transform firePoint;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentState = PlayerState.Idle;
        lastAttackTime = Time.time - attackDelay;
        lastSkillTime = Time.time - skillDelay;
        StartCoroutine(StateLoop());
    }

    private IEnumerator StateLoop()
    {
        while(true)
        {
            switch(currentState)
            {
                case PlayerState.Idle:
                    animator.SetFloat("MoveSpeed", 0f);
                    animator.SetBool("IsMoving", false);
                    FindClosestTarget();
                    if(currentTarget!= null)
                    {
                        currentState = PlayerState.Move;
                    }
                    break;

                case PlayerState.Move:
                    if(currentTarget == null)
                    {
                        currentState = PlayerState.Idle;
                        animator.SetFloat("MoveSpeed", 0f);
                        break;
                    }

                    agent.SetDestination(currentTarget.position);
                    animator.SetBool("IsMoving", true);
                    animator.SetFloat("MoveSpeed", agent.velocity.magnitude);

                    if(!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                    {
                        agent.ResetPath();
                        animator.SetFloat("MoveSpeed", 0f);
                        animator.SetBool("IsMoving", false);
                        currentState = PlayerState.Attack;
                    }
                    break;

                case PlayerState.Attack:
                    if(currentTarget == null)
                    {
                        currentState = PlayerState.Idle;
                        break;
                    }

                    float distance = Vector3.Distance(transform.position, currentTarget.position);

                    if(distance > attackRange + agent.stoppingDistance)
                    {
                        currentState = PlayerState.Move;
                        break;
                    }

                    transform.LookAt(currentTarget);
                    animator.SetBool("IsMoving", false);

                    if(Time.time - lastAttackTime >= attackDelay)
                    {
                        animator.SetTrigger("Attack");
                        StartCoroutine(DelayFire());
                        lastAttackTime = Time.time;
                    }

                    if(Time.time - lastSkillTime >= skillDelay && mpCondition.curValue >= skillManaCost)
                    {
                        currentState = PlayerState.Skill;
                    }

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

    private IEnumerator DelayFire()
    {
        yield return new WaitForSeconds(0.3f);
        Attack();
    }

    private void FindClosestTarget()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //Debug.Log($"타겟 탐색: 발견한 적 수 : {enemies.Length}");

        if(enemies.Length == 0)
        {
            currentTarget = null;
            return;
        }

        currentTarget = enemies.OrderBy(e => Vector3.Distance(transform.position, e.transform.position)).First().transform;
    }

    private void Attack()
    {
        if(currentTarget != null)
        {
            Vector3 direction = (currentTarget.position - firePoint.position).normalized;

            Arrow.SpawnAndFire(firePoint.position, Quaternion.LookRotation(direction), direction, 20f);
        }

    }

    private void UseSkill()
    {
        if(mpCondition.curValue < skillManaCost)
        {
            return;
        }

        mpCondition.Subtract((int)skillManaCost);

        for(int i = -2; i <= 2; i++)
        {
            GameObject arrow = Instantiate(skillProjectilePrefab, firePoint.position, Quaternion.identity);
            arrow.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + i * 10, 0);
            arrow.GetComponent<Rigidbody>().velocity = arrow.transform.forward * 15f;
        }
    }

    public bool IsDead()
    {
        return hpCondition.curValue <= 0;
    }

    //  불필요하지만 에러 방지용 코드
    public void FireProjectile()
    {
    }
}

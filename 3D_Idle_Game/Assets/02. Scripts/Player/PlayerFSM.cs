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
        lastAttackTime = Time.time;
        lastSkillTime = Time.time;
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

                    if(Vector3.Distance(transform.position, currentTarget.position) <= attackRange)
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

                    transform.LookAt(currentTarget);

                    animator.SetBool("IsMoving", false);

                    if(Time.time - lastAttackTime >= attackDelay)
                    {
                        animator.SetTrigger("Attack");
                        Attack();
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

    private void FindClosestTarget()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

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
            GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
            arrow.transform.LookAt(currentTarget);
            arrow.GetComponent<Rigidbody>().velocity = (currentTarget.position - firePoint.position).normalized * 20f;
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

        animator.SetTrigger("Skill");
    }

    public bool IsDead()
    {
        return hpCondition.curValue <= 0;
    }
}

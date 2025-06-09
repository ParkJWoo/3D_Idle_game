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

        StartCoroutine(CheckState());
        StartCoroutine(ActionState());

    }

    private IEnumerator CheckState()     // ���� ���� �޼��常 �߰��� �� + �ڷ�ƾ 1���� �߰�(CheckState) / ActionState �޼��� 
    {
        while(true)
        {
            FindClosestTarget();

            if(currentTarget == null)
            {
                currentState = PlayerState.Idle;
            }

            else
            {
                float distance = Vector3.Distance(transform.position, currentTarget.position);

                if(distance > attackRange + agent.stoppingDistance)
                {
                    currentState = PlayerState.Move;
                }

                else
                {
                    currentState = PlayerState.Attack;
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator ActionState()
    {
        while(true)
        {
            switch(currentState)
            {
                case PlayerState.Idle:

                    animator.SetFloat("MoveSpeed", 0f);
                    animator.SetBool("IsMoving", false);
                    break;

                case PlayerState.Move:

                    if(currentTarget != null)
                    {
                        agent.SetDestination(currentTarget.position);
                        animator.SetBool("IsMoving", true);
                        animator.SetFloat("MoveSpeed", agent.velocity.magnitude);
                    }
                    break;

                case PlayerState.Attack:
                    agent.ResetPath();
                    animator.SetBool("IsMoving", false);
                    transform.LookAt(currentTarget);

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
        //Debug.Log($"Ÿ�� Ž��: �߰��� �� �� : {enemies.Length}");

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

    //  ���ʿ������� ���� ������ �ڵ�
    public void FireProjectile()
    {
    }
}

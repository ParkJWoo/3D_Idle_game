using System.Collections;
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
        while(true)
        {
            if(playerFSM == null || playerFSM.IsDead())
            {
                currentState = EnemyState.Idle;
                agent.ResetPath();
                yield break;
            }

            switch(currentState)
            {
                case EnemyState.Idle:
                    break;

                case EnemyState.Move:
                    agent.SetDestination(playerFSM.transform.position);

                    animator.SetBool("IsMoving", true);

                    if (Vector3.Distance(transform.position, playerFSM.transform.position) <= attackRange)
                    {
                        agent.ResetPath();
                        currentState = EnemyState.Attack;
                    }
                    break;

                case EnemyState.Attack:
                    transform.LookAt(playerFSM.transform);

                    if (Time.time - lastAttackTime >= attackDelay)
                    {
                        animator.SetTrigger("Attack");
                        lastAttackTime = Time.time;
                    }

                    if(Vector3.Distance(transform.position, playerFSM.transform.position) > attackRange)
                    {
                        currentState = EnemyState.Move;
                    }
                    break;
            }
            yield return null;
        }
    }

    public void DealDamage()
    {
        //if(playerFSM != null && !playerFSM.IsDead())
        //{
        //    float distance = Vector3.Distance(transform.position, playerFSM.transform.position);

        //    if(distance <= attackRange)
        //    {
        //        playerFSM.hpCondition.Subtract(10);
        //        Debug.Log("적이 플레이어에게 데미지를 입힘!");
        //    }
        //}
    }
}

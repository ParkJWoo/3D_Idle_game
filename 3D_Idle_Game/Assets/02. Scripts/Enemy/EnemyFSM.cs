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

                    if (Vector3.Distance(transform.position, playerFSM.transform.position) <= attackRange)
                    {
                        agent.ResetPath();
                        currentState = EnemyState.Attack;
                    }
                    break;

                case EnemyState.Attack:
                    if (Time.time - lastAttackTime >= attackDelay)
                    {
                        Attack();
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

    private void Attack()
    {
        animator.SetTrigger("Attack");
    }
}

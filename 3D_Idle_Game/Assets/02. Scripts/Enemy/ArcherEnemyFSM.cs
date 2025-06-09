using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArcherEnemyFSM : MonoBehaviour
{
    private EnemyState currentState;

    private NavMeshAgent agent;
    private Animator animator;
    private PlayerFSM playerFSM;

    [Header("General Settings")]
    public float attackRange = 10f;
    public float attackDelay = 2f;
    private float lastAttackTime;

    [Header("Ranged Attack Settings")]
    public GameObject arrowPrefab;
    public Transform firePoint;
    private float arrowSpeed = 15f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerFSM = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerFSM>();

        currentState = EnemyState.Idle;
        StartCoroutine(CheckState());
        StartCoroutine(ActionState());
    }

    private IEnumerator CheckState()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while(true)
        {
            if(playerFSM == null || playerFSM.IsDead())
            {
                currentState = EnemyState.Idle;
                agent.ResetPath();
                yield break;
            }

            float distance = Vector3.Distance(transform.position, playerFSM.transform.position);
            currentState = distance <= attackRange ? EnemyState.Attack : EnemyState.Move;

            yield return wait;

        }
    }

    private IEnumerator ActionState()
    {
        while(true)
        {
            switch(currentState)
            {
                case EnemyState.Idle:
                    agent.ResetPath();
                    animator.SetBool("IsMoving", false);
                    break;

                case EnemyState.Move:
                    if(playerFSM != null)
                    {
                        agent.SetDestination(playerFSM.transform.position);
                        animator.SetBool("IsMoving", true);
                    }

                    break;

                case EnemyState.Attack:
                    transform.LookAt(playerFSM.transform);
                    agent.ResetPath();
                    animator.SetBool("IsMoving", false);

                    if(Time.time - lastAttackTime >= attackDelay)
                    {
                        animator.SetTrigger("Attack");
                        StartCoroutine(FireArrowWithDelay(0.3f));
                        lastAttackTime = Time.time;
                    }

                    break;
            }

            yield return null;
        }
    }

    private IEnumerator FireArrowWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if(playerFSM == null)
        {
            yield break;
        }

        Vector3 direction = (playerFSM.transform.position - firePoint.position).normalized;
        EnemyArrow.SpawnAndFire(firePoint.position, Quaternion.LookRotation(direction), direction, arrowSpeed);
    }

    public void DamageAndChase()
    {
        if(playerFSM == null)
        {
            playerFSM = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerFSM>();
        }

        if(playerFSM != null && !playerFSM.IsDead())
        {
            currentState = EnemyState.Move;
        }
    }

    public void DealDamage() { }
}

using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Pool;

public class Arrow : MonoBehaviour
{
    [Header("Arrow Settings")]
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private float damage = 10f;

    private Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        //  초기화
        CancelInvoke();
        Invoke(nameof(ReleasePool), lifeTime);
    }

    private void OnDisable()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            Character target = other.GetComponent<Character>();
            EnemyFSM enemyFSM = other.GetComponent<EnemyFSM>();
            ArcherEnemyFSM archerEnemyFSM = other.GetComponent<ArcherEnemyFSM>();

            if(target != null)
            {
                target.TakeDamage((int)damage);
            }

            else
            {
                Debug.LogWarning("[Arrow] Enemy에 Character 컴포넌트가 없습니다.");
            }

            if(enemyFSM != null)
            {
                enemyFSM.DamageAndChase();
            }

            ReleasePool();
        }

        //else if(other.CompareTag("Wall") || other.CompareTag("Ground"))
        //{
        //    ReleasePool();
        //}
    }

    private void ReleasePool()
    {
        ObjectPool.Instance.ReturnToPool("Arrow", this.gameObject);
    }

    public void Fire(Vector3 direction, float speed)
    {
        transform.rotation = Quaternion.LookRotation(direction);
        rigidbody.velocity = direction.normalized * speed;
    }

    public static void SpawnAndFire(Vector3 position, Quaternion rotation, Vector3  direction, float speed)
    {
        GameObject arrow = ObjectPool.Instance.SpawnFromPool("Arrow", position, rotation);
        Arrow arrowScript = arrow.GetComponent<Arrow>();

        if(arrowScript != null)
        {
            arrowScript.Fire(direction, speed);
        }
    }
   
}

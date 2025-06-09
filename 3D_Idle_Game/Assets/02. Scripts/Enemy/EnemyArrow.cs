using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : MonoBehaviour
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
        //  √ ±‚»≠
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
        if (other.CompareTag("Player"))
        {
            Character target = other.GetComponent<Character>();

            if (target != null)
            {
                target.TakeDamage((int)damage);
            }

            ReleasePool();
        }

        //else if (other.CompareTag("Wall") || other.CompareTag("Ground"))
        //{
        //    ReleasePool();
        //}
    }

    private void ReleasePool()
    {
        ObjectPool.Instance.ReturnToPool("EnemyArrow", this.gameObject);
    }

    public void Fire(Vector3 direction, float speed)
    {
        transform.rotation = Quaternion.LookRotation(direction);
        rigidbody.velocity = direction.normalized * speed;
    }

    public static void SpawnAndFire(Vector3 position, Quaternion rotation, Vector3 direction, float speed)
    {
        GameObject arrow = ObjectPool.Instance.SpawnFromPool("EnemyArrow", position, rotation);
        EnemyArrow arrowScript = arrow.GetComponent<EnemyArrow>();

        if (arrowScript != null)
        {
            arrowScript.Fire(direction, speed);
        }
    }
}

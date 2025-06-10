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
        //  �ʱ�ȭ
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
            //  �ʿ��� ������Ʈ���� �� ������ �����´�
            if(other.TryGetComponent(out Character target))
            {
                target.TakeDamage((int)damage);
            }

            //  EnemyFSM�� ������ ��쿡�� �߰� ����
            if(other.TryGetComponent(out EnemyFSM enemyFSM))
            {
                enemyFSM.DamageAndChase();      //  ���� ���� �߰��Ѵ�
            }

            ReleasePool();
        }
    }

    //  ȭ�쵵 ���������� ������Ʈ Ǯ�� ����� Ȱ���Ͽ��⿡ ������ ȭ�� Ǯ�� �ǵ����� �޼���
    private void ReleasePool()
    {
        ObjectPool.Instance.ReturnToPool("Arrow", this.gameObject);
    }

    //  �߻� ��, ���� ���� ���ư����� ���ִ� �޼���
    public void Fire(Vector3 direction, float speed)
    {
        transform.rotation = Quaternion.LookRotation(direction);
        rigidbody.velocity = direction.normalized * speed;
    }

    //  ȭ���� �����ϰ� �߻��ϴ� �޼���
    public static void SpawnAndFire(Vector3 position, Quaternion rotation, Vector3  direction, float speed)
    {
        GameObject arrow = ObjectPool.Instance.SpawnFromPool("Arrow", position, rotation);
        
        if(arrow != null && arrow.TryGetComponent(out Arrow arrowScript))
        {
            arrowScript.Fire(direction, speed);
        }

        else
        {
            Debug.LogWarning("[Arrow] ObjectPool���� ȭ���� �������ų� Arrow ������Ʈ�� �����ϴ�.");
        }
    }
}

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
            //  필요한 컴포넌트들을 한 번씩만 가져온다
            if(other.TryGetComponent(out Character target))
            {
                target.TakeDamage((int)damage);
            }

            //  EnemyFSM이 존재할 경우에만 추격 시작
            if(other.TryGetComponent(out EnemyFSM enemyFSM))
            {
                enemyFSM.DamageAndChase();      //  맞은 적만 추격한다
            }

            ReleasePool();
        }
    }

    //  화살도 마찬가지로 오브젝트 풀링 방식을 활용하였기에 지정한 화살 풀로 되돌리는 메서드
    private void ReleasePool()
    {
        ObjectPool.Instance.ReturnToPool("Arrow", this.gameObject);
    }

    //  발사 시, 적을 향해 날아가도록 해주는 메서드
    public void Fire(Vector3 direction, float speed)
    {
        transform.rotation = Quaternion.LookRotation(direction);
        rigidbody.velocity = direction.normalized * speed;
    }

    //  화살을 생성하고 발사하는 메서드
    public static void SpawnAndFire(Vector3 position, Quaternion rotation, Vector3  direction, float speed)
    {
        GameObject arrow = ObjectPool.Instance.SpawnFromPool("Arrow", position, rotation);
        
        if(arrow != null && arrow.TryGetComponent(out Arrow arrowScript))
        {
            arrowScript.Fire(direction, speed);
        }

        else
        {
            Debug.LogWarning("[Arrow] ObjectPool에서 화살을 가져오거나 Arrow 컴포넌트가 없습니다.");
        }
    }
}

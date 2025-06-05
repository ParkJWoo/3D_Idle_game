using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int damage = 10;
    public float lifeTime = 5f;
    public float speed = 20f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
        GetComponent<Rigidbody>().velocity = transform.rotation * Vector3.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<Enemy>();

            if(enemy != null)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }

    //public static GameObject Spawn(GameObject prefab, Transform firePoint)
    //{
    //    Quaternion rotation = Quaternion.LookRotation(firePoint.forward);

    //    return Instantiate(prefab, firePoint.position, rotation);
    //}
}

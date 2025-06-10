using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public string tag;
    public GameObject prefab;
    public int size;
}

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [Header("Pooling Settings")]
    public List<Pool> pools = new List<Pool>();

    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePools();
        }

        else
        {
            Destroy(gameObject);
        }
    }

    //  Pool �ʱ�ȭ �޼���
    private void InitializePools()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(var pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for(int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    //  Ǯ �� ������ ������ �����ϴ� �޼���
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"[ObjectPool] '{tag}' �±��� Ǯ�� ã�� �� �����ϴ�");
            return null;
        }

        var queue = poolDictionary[tag];

        if(queue.Count == 0)
        {
            Debug.LogWarning($"[ObjectPool] '{tag}' Ǯ�� ��� ������ ������Ʈ�� �����ϴ�.");
            return null;
        }

        GameObject obj = queue.Dequeue();

        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        return obj;
    }

    //  ���̺� �� ������ �׾��� ��, Destroy���� �ʰ� Ǯ�� �ǵ��� ��, ��Ȱ��ȭ ó�����ִ� �޼���
    public void ReturnToPool(string tag, GameObject obj)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"[ObjectPool] '{tag}' �±��� Ǯ�� �������� �ʽ��ϴ�. ��ü�� �����մϴ�.");
            Destroy(obj);
            return;
        }

        obj.SetActive(false);
        poolDictionary[tag].Enqueue(obj);
    }
}

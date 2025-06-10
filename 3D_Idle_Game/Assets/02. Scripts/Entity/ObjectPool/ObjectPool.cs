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

    //  Pool 초기화 메서드
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

    //  풀 내 설정한 적들을 스폰하는 메서드
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"[ObjectPool] '{tag}' 태그의 풀을 찾을 수 없습니다");
            return null;
        }

        var queue = poolDictionary[tag];

        if(queue.Count == 0)
        {
            Debug.LogWarning($"[ObjectPool] '{tag}' 풀에 사용 가능한 오브젝트가 없습니다.");
            return null;
        }

        GameObject obj = queue.Dequeue();

        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        return obj;
    }

    //  웨이브 내 적들이 죽었을 때, Destroy하지 않고 풀로 되돌린 후, 비활성화 처리해주는 메서드
    public void ReturnToPool(string tag, GameObject obj)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"[ObjectPool] '{tag}' 태그의 풀이 존재하지 않습니다. 객체를 제거합니다.");
            Destroy(obj);
            return;
        }

        obj.SetActive(false);
        poolDictionary[tag].Enqueue(obj);
    }
}

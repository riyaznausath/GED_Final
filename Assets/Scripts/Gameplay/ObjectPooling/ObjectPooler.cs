using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectPooler : MonoBehaviour
{

    public static ObjectPooler instance;
    public List<Pool> pools;

    Queue<GameObject> ObjectPool;

    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        if (!instance)
            instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            ObjectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.enemyObject);
                obj.SetActive(false);
                ObjectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, ObjectPool);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject getFromPool(string tag, Vector3 pos, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning(tag + " does not exist !");
            return null;
        }
        GameObject returnobject = poolDictionary[tag].Dequeue();
       returnobject.SetActive(true);
        returnobject.transform.position = pos;
        returnobject.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(returnobject);

        return returnobject;
    }
}

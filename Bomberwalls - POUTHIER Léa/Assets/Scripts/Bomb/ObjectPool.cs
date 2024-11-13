using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; set; }

    [SerializeField]
    private GameObject _objectToPool;
    [field : SerializeField]
    public int AmountToPool {get;private set;}

    public List<GameObject> PooledObjects { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        else
        {
            Instance = this;
        }

        PooledObjects = new List<GameObject>();
        GameObject temp;

        for (int i = 0; i < AmountToPool; i++)
        {
            temp = Instantiate(_objectToPool);
            temp.transform.rotation = _objectToPool.transform.rotation;
            temp.SetActive(false);

            temp.name = $"{_objectToPool.name}";
            PooledObjects.Add(temp);
        }
    }

    public List<GameObject> GetAllObjects()
    {
        for(int i = 0; i <= AmountToPool; i++)
        {

        }
        return PooledObjects;
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < AmountToPool; i++)
        {
            if (!PooledObjects[i].activeInHierarchy)
            {
                var obj = PooledObjects[i];
                PooledObjects.Remove(obj);
                return obj;
            }
        }
        return null;
    }

    public void AddPoolObjectBack(GameObject pooledObject)
    {
        PooledObjects.Add(pooledObject);
    }
}


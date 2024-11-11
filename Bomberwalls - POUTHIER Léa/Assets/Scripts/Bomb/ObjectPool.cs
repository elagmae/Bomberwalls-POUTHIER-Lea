using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; set; }

    [SerializeField]
    private GameObject _objectToPool;
    [field : SerializeField]
    public int AmountToPool {get;private set;}

    private List<GameObject> _pooledObjects;

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

        _pooledObjects = new List<GameObject>();
        GameObject temp;

        for (int i = 0; i < AmountToPool; i++)
        {
            temp = Instantiate(_objectToPool);
            temp.transform.rotation = _objectToPool.transform.rotation;
            temp.SetActive(false);

            temp.name = $"{_objectToPool.name}";
            _pooledObjects.Add(temp);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < AmountToPool; i++)
        {
            if (!_pooledObjects[i].activeInHierarchy)
            {
                var obj = _pooledObjects[i];
                _pooledObjects.Remove(obj);
                return obj;
            }
        }
        return null;
    }

    public void AddPoolObjectBack(GameObject pooledObject)
    {
        _pooledObjects.Add(pooledObject);
    }
}


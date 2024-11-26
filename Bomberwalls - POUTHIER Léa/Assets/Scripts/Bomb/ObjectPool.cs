using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [field: SerializeField]
    public int AmountToPool { get; private set; }
    public static ObjectPool Instance { get; set; }
    public List<GameObject> PooledObjects { get; private set; } = new();
    public List<GameObject> ActivatedObjects { get; private set; } = new();

    [SerializeField]
    private GameObject _objectToPool;

    // Création d'une object pool en singleton.

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        

        for (int i = 0; i < AmountToPool; i++)
        {
            GameObject temp = Instantiate(_objectToPool);
            temp.transform.rotation = _objectToPool.transform.rotation;
            temp.SetActive(false);

            temp.name = $"{_objectToPool.name} {i+1}";
            PooledObjects.Add(temp);
        }
    }

    //Récupère un objet de la pool.
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < PooledObjects.Count; i++)
        {
            if (!PooledObjects[i].activeInHierarchy)
            {
                var obj = PooledObjects[i];
                obj.SetActive(true);
                PooledObjects.Remove(obj);
                ActivatedObjects.Add(obj);
                return obj;
            }
        }
        return null;
    }

    // Replace un objet à l'intérieur de la pool.
    public void AddPoolObjectBack(GameObject pooledObject)
    {
        PooledObjects.Add(pooledObject);
        ActivatedObjects.Remove(pooledObject);
        pooledObject.SetActive(false);
    }
}


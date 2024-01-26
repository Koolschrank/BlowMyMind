using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    [SerializeField] string tag; // Identifier for the pool
    [SerializeField] GameObject prefab; // The object to be pooled
    [Tooltip("get inactive object rather then the next object. If a object can stay for a long period of time then check this. This cost more recources")]
    [SerializeField] bool checkForInactive = true; // return inactive objects before using a used one
    [SerializeField] int size; // Initial pool size
    [SerializeField] bool childOfPooler = true; // if true, the object will be a child of the object pooler
    int currentIndex = -1; // its -1 because we increment it before we use it
    List<GameObject> objectList = new List<GameObject>();

    // geter for tag
    public string Tag
    {
        get { return tag; }
    }

    // geter for prefab
    public GameObject Prefab
    {
        get { return prefab; }
    }

    // geter for checkForInactive
    public bool CheckForInactive
    {
        get { return checkForInactive; }
    }

    // geter for size
    public int Size
    {
        get { return size; }
    }

    // geter for childOfPooler
    public bool ChildOfPooler
    {
        get { return childOfPooler; }
    }

    public void SetIndex()
    {
        currentIndex++;
        if (currentIndex >= size)
            currentIndex = 0;
    }

    // get the next object in the pool
    public GameObject GetNext()
    {
        SetIndex();
        return objectList[currentIndex];
    }

    public void AddObjectToList(GameObject obj)
    {
        objectList.Add(obj);
    }



}

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance; // Singleton instance
    [SerializeField] List<Pool> pools;
    [SerializeField] Dictionary<string, Pool> poolDictionary;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        poolDictionary = new Dictionary<string, Pool>();

        // Create and initialize the pools
        foreach (Pool pool in pools)
        {

            for (int i = 0; i < pool.Size; i++)
            {
                GameObject obj = Instantiate(pool.Prefab);
                obj.SetActive(false);
                pool.AddObjectToList(obj);
                if (pool.ChildOfPooler)
                    obj.transform.parent = transform;
            }

            poolDictionary.Add(pool.Tag, pool);
        }
    }

    public GameObject GetObjectAndSetToTransform(string tag, Transform transform)
    {
        return GetObjectAndSetToTransform(tag, transform);
    }

    public GameObject GetObjectAndSetToTransform(string tag, Transform transform, bool child)
    {
        GameObject obj = GetPooledObject(tag);
        if (obj == null)
        {
            // print error
            Debug.LogWarning("Object pooler does not have " + tag + " in it");

            return null;
        }
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        if (child)
            obj.transform.parent = transform;
        obj.SetActive(true);
        return obj;
    }

    public GameObject GetPooledObject(string tag)
    {
        


        if (poolDictionary.ContainsKey(tag))
        {
            var pool_current = poolDictionary[tag];

            GameObject temp = pool_current.GetNext();

            
            if (pool_current.CheckForInactive)
            {
                for(int i = 0; i < pool_current.Size; i++)
                {
                    if (!temp.activeInHierarchy)
                        break;

                    temp = pool_current.GetNext();
                }
            }

             
            temp.SetActive(false);

            return temp;

        }

        return null;
    }
}



using System.Collections;
using System.Collections.Generic;
using SystemScripts;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public float spawnTime = 5f;
    public GameObject[] items;
    float spawnTimeCounter = 0f;

    public Vector3 minPos = new Vector3(-5f, 10f, -5f);
    public Vector3 maxPos = new Vector3(5f, 10f, 5f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerSystem.instance.IsGameStarted())
        {
            return;
        }
        
        // spawn item
        spawnTimeCounter += Time.deltaTime;
        if (spawnTimeCounter >= spawnTime)
        {
            spawnTimeCounter = 0f;
            SpawnItem();
        }
        
    }

    void SpawnItem()
    {
        // random position
        Vector3 pos = new Vector3(Random.Range(minPos.x, maxPos.x), Random.Range(minPos.y, maxPos.y), Random.Range(minPos.z, maxPos.z));
        // random item
        GameObject item = items[Random.Range(0, items.Length)];
        // spawn item
        Instantiate(item, pos, Quaternion.identity);
    }
}

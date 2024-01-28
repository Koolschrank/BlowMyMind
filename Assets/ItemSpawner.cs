using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SystemScripts;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public float spawnTime = 5f;
    public GameObject[] items;
    public float[] probabilities;

    public Vector3 minPos = new (-5f, 10f, -5f);
    public Vector3 maxPos = new (5f, 10f, 5f);
    
    private float _spawnTimeCounter = 0f;
    private float _probabilitySum;
    private List<(float, ProbabilitySet)> _probabilitySets = new ();
    
    void Start()
    {
        if(items.Length != probabilities.Length)
            Debug.LogError("ItemSpawner: number of items and probabilities do not match!");

        _probabilitySum = probabilities.Sum();
        var probabilityVal = 0f;
        for (var i = 0; i < probabilities.Length; i++)
        {
            var endProbability = probabilityVal + probabilities[i];
            _probabilitySets.Add((probabilityVal, new ProbabilitySet(endProbability, items[i])));
            probabilityVal = endProbability;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerSystem.instance.IsGameStarted())
        {
            return;
        }
        
        // spawn item
        _spawnTimeCounter += Time.deltaTime;
        if (_spawnTimeCounter >= spawnTime)
        {
            _spawnTimeCounter = 0f;
            SpawnItem();
        }
        
    }

    void SpawnItem()
    {
        var randomVal = Random.Range(0, _probabilitySum);
        GameObject pickupItem = null;
        for (var i = 0; i < _probabilitySets.Count; i++)
        {
            if (_probabilitySets[i].Item1 <= randomVal)
            {
                if (_probabilitySets[i].Item2.EndProbability > randomVal)
                {
                    pickupItem = _probabilitySets[i].Item2.Item;
                }
            }
        }
        
        if(pickupItem == null)
        {
            Debug.LogError("Error on random item generation");
            return;
        }
        
        // random position
        Vector3 pos = new Vector3(Random.Range(minPos.x, maxPos.x), Random.Range(minPos.y, maxPos.y), Random.Range(minPos.z, maxPos.z));
        // spawn item
        Instantiate(pickupItem, pos, Quaternion.identity);
    }

    private struct ProbabilitySet
    {
        public float EndProbability;
        public GameObject Item;

        public ProbabilitySet(float endProbability, GameObject item)
        {
            EndProbability = endProbability;
            Item = item;
        }
    }
}

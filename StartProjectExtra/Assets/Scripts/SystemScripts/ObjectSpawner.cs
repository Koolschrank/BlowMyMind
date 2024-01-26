using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : PoolObject
{
    public PoolSpawnValue objectToSpawn;
    public int amountToSpawn;

    public float spawnRoationChange_z;
    public bool rotaionChange_random;

    // Start is called before the first frame update
    void OnEnable()
    {
        if(!CheckPooled()) return;

        // spawn objects with objectPooler
        for (int i = 0; i < amountToSpawn; i++)
        {
            // spawn the object
            var spawnedObject = objectToSpawn.Play(transform);
            spawnedObject.transform.position = transform.position;
            // forward is the direction the object is facing
            spawnedObject.transform.forward = transform.forward;
            // change the rotation of the object
            if (rotaionChange_random)
            {
                spawnedObject.transform.Rotate(0, Random.Range(-spawnRoationChange_z/2, spawnRoationChange_z/2),0);
            }
            else
            {
                // first object is -spawnRoationChange_z/2 last object is spawnRoationChange_z/2
                spawnedObject.transform.Rotate(0, -spawnRoationChange_z/2 + (spawnRoationChange_z / amountToSpawn) * i, 0);
            }
            
            

            spawnedObject.SetActive(true);
        }
        gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}



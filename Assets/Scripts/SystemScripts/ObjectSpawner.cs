using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : PoolObject
{
    public PoolSpawnValue objectToSpawn;
    public int amountToSpawn;

    public float spawnRoationChange_z;
    public bool rotaionChange_random;

    public Vector3 spawnPosition_min;
    public Vector3 spawnPosition_max;
    public bool spawnPosition_random;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (!CheckPooled()) return;

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
                spawnedObject.transform.Rotate(0, Random.Range(-spawnRoationChange_z / 2, spawnRoationChange_z / 2), 0);
            }
            else
            {
                // first object is -spawnRoationChange_z/2 last object is spawnRoationChange_z/2
                //                                                -1/2     +       1               / 2           * 1    = 0
                spawnedObject.transform.Rotate(0, -spawnRoationChange_z / 2 + spawnRoationChange_z / Mathf.Max(amountToSpawn - 1, 1) * i, 0); // mathf.max to prevent division by 0
            }
            // change the position of the object
            if (spawnPosition_random)
            {
                spawnedObject.transform.position += new Vector3(Random.Range(spawnPosition_min.x, spawnPosition_max.x), Random.Range(spawnPosition_min.y, spawnPosition_max.y), Random.Range(spawnPosition_min.z, spawnPosition_max.z));
            }
            else
            {
                // first object is spawnPosition_min last object is spawnPosition_max
                spawnedObject.transform.position += new Vector3(spawnPosition_min.x + (spawnPosition_max.x - spawnPosition_min.x) / Mathf.Max(amountToSpawn - 1, 1) * i, spawnPosition_min.y + (spawnPosition_max.y - spawnPosition_min.y) / Mathf.Max(amountToSpawn - 1, 1) * i, spawnPosition_min.z + (spawnPosition_max.z - spawnPosition_min.z) / Mathf.Max(amountToSpawn - 1, 1) * i);
            }



            spawnedObject.SetActive(true);
        }
        gameObject.SetActive(false);

    }
}



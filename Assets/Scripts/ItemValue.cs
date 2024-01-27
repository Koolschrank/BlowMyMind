using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemValue", menuName = "ScriptableObjects/ItemValue", order = 1)]
public class ItemValue : ScriptableObject
{
    public GameObject itemToSpawn;
    public int moveToSpawn;


    public int Activate()
    {
        return moveToSpawn;
    }
}

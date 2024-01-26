using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{

    bool isPooled = false;
    
    // pooled objects call this function when enabled, on first enable they shouldt do anything so this function returns flase
    public bool CheckPooled()
    {

        if (isPooled)
        {
            return true;
        }
        isPooled = true;
        return false;
    }
}

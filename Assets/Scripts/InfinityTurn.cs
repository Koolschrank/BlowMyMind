using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using Vector3 = UnityEngine.Vector3;

public class InfinityTurn : MonoBehaviour
{
    [SerializeField] private float turnSpeed = 5f;
    
    void Update()
    {
        transform.RotateAround(Vector3.up, turnSpeed * Time.deltaTime);
    }
}

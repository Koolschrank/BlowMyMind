using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCube : MonoBehaviour
{
    public DamageNumberValue damageNumberValue;


    public float cooldown = 1f;
    float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= cooldown)
        {
            timer = 0f;
            damageNumberValue.Play(transform);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 2f;
    float lifeTimer = 0f;
    // Start is called before the first frame update
    private void OnEnable()
    {
        lifeTimer = lifeTime;
    }
    

    // Update is called once per frame
    void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            gameObject.SetActive(false);
            return;
        }
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        
    }


}

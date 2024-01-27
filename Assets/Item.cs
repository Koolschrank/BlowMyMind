using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemValue itemValue;
    public int itemInt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // if other is playerCaracter activate item pick up
        if (other.GetComponent<PlayerCaracter>())
        {
            // activate item pick up
           other.GetComponent<PlayerCaracter>().ItemPickUp(itemInt);

            DestroyItem();
        }



        
    }

    // destroy item
    public void DestroyItem()
    {
        Destroy(gameObject);
    }
}

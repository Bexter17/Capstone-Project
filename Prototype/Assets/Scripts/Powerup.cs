using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    
    // activating power up for player
    void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            
            Pickup();
        }

    }


    void Pickup()
    {
        
        Debug.Log("Power up picked up");
        Destroy(gameObject);

    }



}

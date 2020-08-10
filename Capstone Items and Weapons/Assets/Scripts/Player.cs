using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float health;
    public float speed;
    public float armor;
    public float damage;

    

    // Start is called before the first frame update
    void Start()
    {

    }

    //each power up has its own tag to do different things.
     void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Health")
        {
            health++;
            Debug.Log("health increased");
        }
        if(collision.gameObject.tag == "Armor")
        {
            armor++;
            Debug.Log("Armor increased");
        }
        if (collision.gameObject.tag == "Speed")
        {
            armor++;
            Debug.Log("Speed increased");
        }
        if (collision.gameObject.tag == "Damage")
        {
            armor++;
            Debug.Log("Damage increased");
        }
    }


}

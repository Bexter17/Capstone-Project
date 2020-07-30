using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // The player that the enemy will chase
    public Transform target;

    // How fast enemy moves
    public float enemyMovement;

    // The distance the enemy will begin to chase player
    public float chaseRange;
    public float attackRange;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;
        if(enemyMovement <= 0)
        {
            enemyMovement = 10f;
        }
        if(chaseRange <= 0)
        {
            chaseRange = 5f;
        }
        if(attackRange <= 0)
        {
            attackRange = 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if the distance between enemy and player
        // is less then chaseRange
        if (Vector3.Distance(target.position, gameObject.transform.position) < chaseRange && Vector3.Distance(target.position, gameObject.transform.position) > attackRange)
        {
            Chase();

        }
    }
    private void Chase()
    {
        // Rotates to always face the player
        transform.LookAt(target);
        // Move forward
        transform.position += transform.forward * enemyMovement * Time.deltaTime;
    }
    //private void Patrol()
    //{
    //    // Generate random number from 0-360
    //    int randomRotation = Random.Range(0, 361);

    //    // rotate to random position
    //    transform.position += transform.forward * enemyMovement * Time.deltaTime;
    //}
    private void Attack()
    {

    }
}

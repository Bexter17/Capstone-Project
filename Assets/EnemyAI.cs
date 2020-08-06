using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // The player that the enemy will chase
    public Transform target;
    public Transform initialPos;
    bool isInitPos = true;

    // How fast enemy moves
    public float enemyMovement;
    public Rigidbody rb;

    // The distance the enemy will begin to chase player
    public float chaseRange;
    public float attackRange;

    bool isMoving = true;
    bool isPatrolling = false;
    public float patrolDuration = 2f;
    public float patrolPause = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody>();
        initialPos = gameObject.transform;

        if (enemyMovement <= 0)
        {
            enemyMovement = 10f;
        }
        if(chaseRange <= 0)
        {
            chaseRange = 5f;
        }
        if(attackRange <= 0)
        {
            attackRange = 2f;
        }
        if(patrolDuration <= 0)
        {
            patrolDuration = 2f;
        }
        if(patrolPause < 0)
        {
            patrolPause = 0.5f;
        }
        Patrol();
        //MoveContinuouslyForward();
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if enemy has returned to starting position
        if (gameObject.transform == initialPos)
        {
            isInitPos = true;
        }
        // Checks if the distance between enemy and player
        // is less then chaseRange
        if (Vector3.Distance(target.position, gameObject.transform.position) < chaseRange)
        {
            Chase();
        }
        else if (Vector3.Distance(target.position, gameObject.transform.position) < attackRange)
        {
            isMoving = false;
        }
        else if (!isInitPos)
        {
            GoHome();
        }
        else if(!isPatrolling && isInitPos)
        {
            Patrol();
        }
        MoveForward();


    }
    private void MoveForward()
    {
        if (isMoving)
        {
            transform.position += transform.forward * enemyMovement * Time.deltaTime;
        }
    }
    private void GoHome()
    {
        transform.LookAt(initialPos, Vector3.up);
    }
    //private void MoveContinuouslyForward()
    //{
    //    rb.velocity = gameObject.transform.rotation * Vector3.forward * enemyMovement * Time.deltaTime;
    //}
    private void Chase()
    {
        isPatrolling = false;
        isInitPos = false;
        // Rotates to always face the player
        transform.LookAt(target, Vector3.up);
        // Move forward
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
    private void Patrol()
    {
        isPatrolling = true;
        isMoving = true;
        transform.Rotate(0, 180, 0);
        Invoke("PatrolPause", patrolDuration);
    }
    private void PatrolPause()
    {
        isMoving = false;
        Invoke("Patrol", patrolPause);
    }
}

        //Go in circles
        //transform.Rotate(0, 1, 0);
        //transform.position += transform.forward* enemyMovement * Time.deltaTime;
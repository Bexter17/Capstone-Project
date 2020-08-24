﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // The player that the enemy will chase
    public Transform target;
    public Vector3 initialPos;
    bool isInitPos = true;

    // How fast enemy moves
    public float enemyMovement;
    public Rigidbody rb;

    // The distance the enemy will begin to chase player
    public float chaseRange;
    public float attackRange;

    bool isMoving = true;
    bool isPatrolling = false;
    public float patrolDuration;
    public float patrolPause;

    NavMeshAgent agent;
    Vector3 patrolPos1;
    Vector3 patrolPos2;
    //float randomRotation;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        initialPos = agent.transform.position;

        if (enemyMovement <= 0)
        {
            enemyMovement = 10f;
        }
        if (chaseRange <= 0)
        {
            chaseRange = 5f;
        }
        if (attackRange <= 0)
        {
            attackRange = 2f;
        }
        if (patrolDuration <= 0)
        {
            patrolDuration = 2f;
        }
        if (patrolPause <= 0)
        {
            patrolPause = 0.5f;
        }
        Patrol();
        //MoveContinuouslyForward();
        patrolPos1 = new Vector3(initialPos.x, initialPos.y, initialPos.z + 5);
        patrolPos2 = new Vector3(initialPos.x, initialPos.y, initialPos.z - 5);
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if enemy has returned to starting position
        if (Vector3.Distance(initialPos, gameObject.transform.position) < 0.5)
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
        else if (!isPatrolling && isInitPos)
        {
            Patrol();
        }
        //MoveForward();


    }
    //private void MoveForward()
    //{
    //    if (isMoving)
    //    {
    //        transform.position += transform.forward * enemyMovement * Time.deltaTime;
    //    }
    //}
    private void GoHome()
    {
        Debug.Log("GOING HOME");
        //transform.LookAt(initialPos, Vector3.up);
        agent.SetDestination(initialPos);
    }
    //private void MoveContinuouslyForward()
    //{
    //    rb.velocity = gameObject.transform.rotation * Vector3.forward * enemyMovement * Time.deltaTime;
    //}
    private void Chase()
    {
        Debug.Log("CHASE");
        isPatrolling = false;
        isInitPos = false;
        // Rotates to always face the player
        //transform.LookAt(target, Vector3.up);
        // Sets player as destination
        agent.SetDestination(target.transform.position);
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
    // During Patrol moves one way for patrolDuration then stops for patrolPause
    // Turns around and continues
    private void Patrol()
    {
        Debug.Log("PATROL");
        //At the begin of patrolling sets first patrol destination
        if (!isPatrolling)
        {
            agent.SetDestination(patrolPos1);
            isPatrolling = true;
        }
        isMoving = true;
        //transform.Rotate(0, 180, 0);
        //Invoke("PatrolPause", patrolDuration);
        // If agent is close to finish it will switch destination
        if (agent.remainingDistance < 1)
        {
            if (agent.destination != patrolPos1)
            {
                agent.SetDestination(patrolPos1);
            }else if(agent.destination != patrolPos2)
            {
                agent.SetDestination(patrolPos2);
            }
        }


    }
    //private void PatrolPause()
    //{
    //    isMoving = false;
    //    Invoke("Patrol", patrolPause);
    //}
    //private void RandNum()
    //{
    //    // Generate random number from 0-360
    //    randomRotation = Random.Range(0, 361);
    //}
}

//Go in circles
//transform.Rotate(0, 1, 0);
//transform.position += transform.forward* enemyMovement * Time.deltaTime;
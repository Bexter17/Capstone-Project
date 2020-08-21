using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Character Mechanics Prototype #3
//Made By Craig Walker

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class Character_Mechanics : MonoBehaviour
{
    //Creates a charactercontoller variable named "controller"
    CharacterController controller;

    //Tracks player health
    int Health = 5;

    //Tracks player's lives
    int Lives = 3;

    public bool isAlive = true;
    //Contains the ball game object to be spawned
    public GameObject Ballprefab;

    public bool isAttacking = false;

    //Location where the ball will spawn
    public Transform BallSpawn;

    //temporary copy of the ball that can be deleted 
    private GameObject temp;

    //Tracks if GodMode is Active
    public bool isGodMode;

    //Sets the length of the Mode
    public float timerGodMode;

    //Variable to amplify the jumpBoost
    public float jumpBoost;

    //How long the jump boost lasts
    public float timerJumpBoost;

    //How much we are boosting the speed by
    public float speedBoost;

    //How long the speed boost lasts
    public float timerSpeedBoost;

    //Determines how fast the character moves
    public float speed;

    //Variable for how high the character will jump
    public float jumpSpeed;

    //Rotation speed of the character
    public float rotationSpeed; // Used when not using MouseLook.CS to rotate character
    
    //Amount of gravity set on the player
    public float gravity;

    //Allows you to toggle hold to crouch or press to crouch
    public bool crouchIsToggle;

    //Tracks if the player is actively hold crouch key
    public bool isCrouched = false;

    //Boolean to track if the player is on the ground or in the air
    public bool isGrounded;

    //Variable used to add force or direction to the character
    Vector3 moveDirection;

    //Switch to allow mouse or keyboard control for the character
    enum ControllerType { ThirdPersonMove, FirstPersonMove };

    //Enables the controllerType to be seen in the inpsector
    [SerializeField] ControllerType type;

    //creates the animator version of the Animator component
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            //Accesses the CharacterController component on the character object 
            controller = GetComponent<CharacterController>();

            //Accesses the Animator component
            animator = GetComponent<Animator>();

            //Automatically disables Root Motion (to avoid adding motion twice)
            animator.applyRootMotion = false;

            //Sets variables to a defualt value incase not set in Unity inspector
            if (speed <= 0)
            {
                speed = 6.0f;
                Debug.Log("Speed not set on " + name + " defaulting to " + speed);
            }

            if (jumpSpeed <= 0)
            {
                jumpSpeed = 8.0f;
                Debug.Log("JumpSpeed not set on " + name + " defaulting to " + jumpSpeed);
            }

            if (rotationSpeed <= 0)
            {
                rotationSpeed = 10.0f;
                Debug.Log("RotationSpeed not set on " + name + " defaulting to " + rotationSpeed);
            }

            if (gravity <= 0)
            {
                gravity = 9.81f;
                Debug.Log("Gravity not set on " + name + " defaulting to " + gravity);
            }

            //Assigns a value to the variable
            moveDirection = Vector3.zero;

            // Manually throw the Exception or the System will throw an Exception
            // throw new ArgumentNullException("Whoops");
        }
        catch (NullReferenceException e)
        {
            Debug.LogWarning(e.Message);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            //If health drops to or below zero, the player dies
            if (Health <= 0)
            {
                animator.SetTrigger("Die");
                isAlive = false;
            }

            switch (type)
            {
                //Use if not using MouseLook.CS
                case ControllerType.ThirdPersonMove:

                    //Character rotation
                    transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed, 0);

                    //Character movement
                    Vector3 forward = transform.TransformDirection(Vector3.forward);

                    //Movement speed
                    float curSpeed = Input.GetAxis("Vertical") * speed;

                    //Character controller movement
                    controller.SimpleMove(transform.forward * (Input.GetAxis("Vertical") * speed));

                    break;

                case ControllerType.FirstPersonMove:
                    if (controller.isGrounded)
                    {
                        //Assign "moveDirection" to track vertical movement
                        moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));

                        //Applies moveDirection to the character
                        moveDirection = transform.TransformDirection(moveDirection);

                        //Applies the speed variable
                        moveDirection *= speed;

                        //Enables the character to jump
                        if (Input.GetButtonDown("Jump"))
                            moveDirection.y = jumpSpeed;
                    }

                    //Applies the gravity to the y axis of the character
                    moveDirection.y -= gravity * Time.deltaTime;

                    //Applies the direction of movement to the character's controller component
                    controller.Move(moveDirection * Time.deltaTime);




                    break;
            }
            Debug.Log("Speed: " + moveDirection);
            //animator.SetFloat("Speed", transform.TransformDirection(controller.velocity).z);
            //animator.SetFloat("Speed", transform.InverseTransformDirection(controller.velocity).z);
            //animator.SetFloat("Speed", curSpeed);
            animator.SetFloat("Speed", Input.GetAxis("Vertical"));
            animator.SetBool("isGrounded", controller.isGrounded);

            //Attack
            if (Input.GetButtonDown("Fire1"))
            {
                Debug.Log("Attack has been pressed");
                animator.SetTrigger("Ability 1");
                isAttacking = true;
            }

            //Sword attack instead of punch if is holding sword
            //if (Input.GetButtonDown("Fire1"))
            //{
            //Debug.Log("Sword Attack has been pressed");
            //animator.SetTrigger("Sword Attack");
            //isAttacking = true;
            //}

            //Block
            if (Input.GetButtonDown("Fire2"))
            {
                Debug.Log("Kick has been pressed");
                animator.SetTrigger("Ability 2");
                isAttacking = true;
            }

            //Throw attack
            //if (Input.GetButtonDown("Throw"))
            //{
            //Debug.Log("Throw has been pressed");
            //animator.SetTrigger("Throw");
            //isAttacking = true;
            //}

            //Mirror throw attack if holding a sword
            //if (Input.GetButtonDown("Throw") && SwordEquipped)
            //{
            //Debug.Log("Mirror Throw has been pressed");
            //animator.SetTrigger("Mirror Throw");
            //isAttacking = true;
            //}

            //Enables the player to jump
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpSpeed;
                animator.SetTrigger("Jump");
            }

            //Enables the player to toggle crouch on
            if (Input.GetButtonDown("Fire3"))
            {
                isCrouched = true;
                Debug.Log("Crouch is on");
                animator.SetBool("Crouching", isCrouched);
            }


            //Enables the player to toggle crouch off
            if (Input.GetButtonDown("Fire3") && crouchIsToggle && isCrouched)
            {
                isCrouched = false;
                Debug.Log("Crouch is off");
                animator.SetBool("Crouching", isCrouched);

            }

            if (Input.GetButtonUp("Fire3") && !crouchIsToggle)
            {
                isCrouched = false;
                Debug.Log("Crouch is off");
                animator.SetBool("Crouching", isCrouched);
            }

            if (Input.GetButtonDown("Kill Player"))
            {
                Health = 0;
            }
        }
    }
    //Triggers at the end of an animation to turn bool off
    public void AttackEnds()
    {
        isAttacking = false;
    }

    //Launches the ball when triggered by the throw animation 
    public void Throw()
    {
        temp = Instantiate(Ballprefab, BallSpawn.position, BallSpawn.rotation);
        Debug.Log("Ball has been thrown");
    }

    //Tracks player collision 
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("OnControllerColliderHit: " + hit.gameObject.name);

        if(gameObject.tag == "Floor")
        {
            isGrounded = true;
        }

        if (gameObject.tag == "Enemy")
        {
            Health--;
            animator.SetTrigger("Got Hit");
            Debug.Log("PLayer Hit by Enemy");
        }

        if (gameObject.tag == "Projectile")
        {
            Debug.Log("Player Hit by projectile");
            Health--;
            animator.SetTrigger("Got Hit");
        }
    }

    //Tracks triggers / pickups
    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Speed Boost")
        {
            speed *= speedBoost;
            Destroy(c.gameObject);
            Debug.Log("Speed Boost Applied");
            StartCoroutine(stopSpeedBoost());
        }

        if (c.gameObject.tag == "Jump Boost")
        {
            jumpSpeed += jumpBoost;
            Destroy(c.gameObject);
            Debug.Log("Jump Boost Applied");
            StartCoroutine(stopJumpBoost());
        }

        if (c.gameObject.tag == "GodMode Pickup")
        {
            isGodMode = true;

            GetComponentInChildren<Renderer>().material.color = Color.blue;

            Destroy(c.gameObject);

            StartCoroutine(stopGodmode());
        }
    }

    //Tracks collisions with objects
    public void OnCollisionEnter(Collision a)
    {
        if (a.gameObject.tag == "Enemy" && isAttacking)
        {
            //Destroy(a.gameObject);
            //a.gameObject.Health -= 1;
        }
    }

    //Trigger after death animation to fully kill player object
    public void Die()
    {
        Destroy(gameObject);
    }

    IEnumerator stopGodmode()
    {
        yield return new WaitForSeconds(timerGodMode);

        GetComponentInChildren<Renderer>().material.color = Color.white;

        isGodMode = false;
    }

    IEnumerator stopJumpBoost()
    {
        yield return new WaitForSeconds(timerJumpBoost);

        jumpSpeed -= jumpBoost;
    }

    IEnumerator stopSpeedBoost()
    {
        yield return new WaitForSeconds(timerSpeedBoost);

        speed -= speedBoost;
    }
}

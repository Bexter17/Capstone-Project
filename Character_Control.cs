using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Character Mechanics Prototype #2
//Made By Craig Walker

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class Character_Control : MonoBehaviour
{
    //Creates a charactercontoller variable named "controller"
    CharacterController controller;

    //Tracks player health
    int Health;

    //Tracks player's lives
    int Lives;

    //Contains the ball game object to be spawned
    public GameObject Ballprefab;

    public bool isAttacking = false;

    //Location where the ball will spawn
    public Transform BallSpawn;

    //temporary copy of the ball that can be deleted 
    private GameObject temp;

    //Contains the Sword game object to be spawned
    public GameObject SwordPrefab;

    //Location where the Sword will spawn
    public Transform SwordSpawn;

    //temporary copy of the Sword that can be deleted 
    private GameObject Swordtemp;

    //Tracks if the player has picked the sword up yet
    public bool hasSword = false;

    //Tracks if the player has the sword sheathed or not
    public bool SwordEquipped = false;

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
        //If health drops to or below zero, the player dies
        if (Health <= 0)
        {
            animator.SetTrigger("Die");
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

                

                //animator.SetFloat("Speed", transform.TransformDirection(controller.velocity).z);
                animator.SetFloat("Speed", transform.InverseTransformDirection(controller.velocity).z);
                animator.SetBool("isGrounded", controller.isGrounded);
                //animator.SetFloat("Speed", curSpeed);
                break;
        }

        //Punch if not holding sword
        if(Input.GetButtonDown("Fire1") && !hasSword || Input.GetButtonDown("Fire1") && !SwordEquipped)
        {
            Debug.Log("Punch has been pressed");
            animator.SetTrigger("Punch");
            isAttacking = true;
        }

        //Sword attack instead of punch if is holding sword
        if (Input.GetButtonDown("Fire1") && SwordEquipped)
        {
            Debug.Log("Sword Attack has been pressed");
            animator.SetTrigger("Sword Attack");
            isAttacking = true;
        }

        //Kick attack
        if (Input.GetButtonDown("Fire2"))
        {
            Debug.Log("Kick has been pressed");
            animator.SetTrigger("Kick");
            isAttacking = true;
        }

        //Throw attack
        if (Input.GetButtonDown("Throw"))
        {
            Debug.Log("Throw has been pressed");
            animator.SetTrigger("Throw");
            isAttacking = true;
        }

        //Mirror throw attack if holding a sword
        if (Input.GetButtonDown("Throw") && SwordEquipped)
        {
            Debug.Log("Mirror Throw has been pressed");
            animator.SetTrigger("Mirror Throw");
            isAttacking = true;
        }

        //Enables the player to jump
        if (Input.GetButtonDown("Jump"))
        {
            moveDirection.y = jumpSpeed;
            animator.SetTrigger("Jump");
        }

        //Enables the player to toggle crouch on
        if (Input.GetButtonDown("Fire3") && crouchIsToggle && !isCrouched)
        {
            Debug.Log("Crouch has been pressed");
            animator.SetTrigger("Crouch");
            isCrouched = true;
        }

        //Enables the player to toggle crouch off
        if (Input.GetButtonDown("Fire3") && crouchIsToggle && isCrouched)
        {
            Debug.Log("Stand has been pressed");
            animator.SetTrigger("Stand");
            isCrouched = false;
        }

        //Enables the player to hold to crouch
        while (Input.GetButtonDown("Fire3") && !crouchIsToggle)
        {
            isCrouched = true;
            Debug.Log("Crouch has been pressed");
            animator.SetBool("Crouch", isCrouched);
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
            Debug.Log("PLayer Hit");
        }

        if (gameObject.tag == "Ball")
        {
            Debug.Log("Player Hit by ball");
            Health--;
            animator.SetTrigger("Got Hit");
        }
    }

    //Tracks triggers / pickups
    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Sword Pickup")
        {
            hasSword = true;
            Swordtemp = Instantiate(SwordPrefab, SwordSpawn.position, SwordSpawn.rotation);
            Destroy(c.gameObject);
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
}

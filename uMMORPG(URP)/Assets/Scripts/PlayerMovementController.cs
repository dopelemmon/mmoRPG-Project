using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    Animator animator;

    CharacterController characterController;
    
    

    public Transform cam;

    float VelocityZ = 0f;
    float VelocityX = 0f;

    //speed of the player
    public float speed = 12f;

    public float acceleration = 8.0f;
    public float decceleration = 8.0f;

    //refactoring (Increase Performance)
    int velocityZHash;
    int velocityXHash;

    //Jumping

    [SerializeField ]bool isJumpPressed;
    public float jumpSpeed;
    private float ySpeed;

    public float verticalVelocity;
    // Start is called before the first frame update
    void Start()
    {
        // search the game object this script is attached to and get the animator component
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        

        //refactor (Increase Performance)
        velocityZHash = Animator.StringToHash("Velocity Z");
        velocityXHash = Animator.StringToHash("Velocity X");
    }

    void changeVelocity(bool forwardPressed, bool leftPressed, bool rightPressed, bool backPressed)
    {
        if(forwardPressed && VelocityZ < 2.0f)
        {
            VelocityZ += Time.deltaTime * acceleration;
        }

        if(leftPressed && VelocityX > -2.0f)
        {
            VelocityX -= Time.deltaTime * decceleration;
        }

        if(rightPressed && VelocityX < 2.0f)
        {
            VelocityX += Time.deltaTime * acceleration;
        }

        if(backPressed && VelocityZ > -2.0f)
        {
            VelocityZ -= Time.deltaTime * decceleration;
        }

        if(!forwardPressed && VelocityZ > 0.0f)
        {
            VelocityZ -= Time.deltaTime * decceleration;
        }

        if(!rightPressed && VelocityX > 0.0f)
        {
            VelocityX -= Time.deltaTime * decceleration;
        }

        if(!leftPressed && VelocityX < 0.0f)
        {
            VelocityX += Time.deltaTime * decceleration;
        }

        if(!backPressed && VelocityZ < 0.0f){
            VelocityZ += Time.deltaTime * decceleration;
        }
    }

    void resetVelocity(bool forwardPressed, bool rightPressed, bool leftPressed, bool backPressed)
    {
       if(!leftPressed && !rightPressed && VelocityX != 0.0f)
        {
            VelocityX = 0.0f;
        }

        if(!forwardPressed && !backPressed && VelocityZ != 0.0f)
        {
            VelocityZ = 0.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //************************************************************************
        /// Player Movement
        // input will be true if the player is pressing on passed in key parameter
        //get key input from player
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool leftPressed = Input.GetKey(KeyCode.A);
        bool rightPressed = Input.GetKey(KeyCode.D);
        bool backPressed = Input.GetKey(KeyCode.S);
        
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1f)
            {
                
                if (direction.magnitude > 0.1f)
                {
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                    Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                    characterController.Move(moveDirection.normalized * speed * Time.deltaTime);
                    transform.rotation = Quaternion.Euler(0f, cam.eulerAngles.y, 0f);
                }
                
            }

        // handles changes in velocity
        changeVelocity(forwardPressed, leftPressed, rightPressed, backPressed);
        //resetVelocity(forwardPressed, rightPressed, leftPressed, backPressed);
        
        //**************************************************************************
        // Jumping Code
        Jumping();

        if(characterController.isGrounded)
        {
            verticalVelocity = 0.0f;
        }
        else{
            verticalVelocity -= 1f * Time.deltaTime;
        }

        Vector3 downForce = new Vector3(0f, verticalVelocity, 0f).normalized;
        characterController.Move(downForce.normalized);


        animator.SetFloat(velocityZHash, VelocityZ);
        animator.SetFloat(velocityXHash, VelocityX);
    }

    public void Jumping()
    {

    }
}

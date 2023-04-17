using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public enum state {Normal, Fighting};
    public state playerState = state.Normal;
    Animator animator;
    CharacterController characterController;
    public Transform cam;

    public float speed = 100f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    float velocity;

    //********************* FIGHTING STATE *************************
    float velocityZ = 0.0f;
    float velocityX = 0.0f;

    public float maximumMoveVelocity = 1.0f;

    public float acceleration = 2.0f;
    public float deceleration = 2.0f;
    public float rotationSpeed = 1.0f;

    //REFACTOR
    int velocityZHash; 
    int velocityXHash;

    

    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        velocity = animator.GetFloat("Velocity");

        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerState == state.Normal)
            HandleMovement();
        if(playerState == state.Fighting)
        {
            FighitngMovement();
        }
    }

    void HandleMovement()
    {
        
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        bool movementPressed = direction.magnitude >= 0.1f;
        bool runPressed = Input.GetKey(KeyCode.LeftShift);
        if(movementPressed)
        {
            animator.SetBool("isMoving", movementPressed);
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.SimpleMove(moveDir.normalized * Time.deltaTime);
        }
        else{
            animator.SetBool("isMoving", movementPressed);
        }
        if(movementPressed && velocity <= 1f && !runPressed)
        {
            velocity += 2f * Time.deltaTime;

            
            
        }
        if(movementPressed && velocity >= 1f && !runPressed)
        {

            velocity -= 2f * Time.deltaTime;

            
        }
        if(movementPressed && velocity <= 2f && runPressed)
        {
            velocity += 2f * Time.deltaTime;
            
        }
        if(!movementPressed && velocity >= 0f && !runPressed)
        {
            velocity -= 2f * Time.deltaTime;
            
        }
        
        animator.SetFloat("Velocity", velocity);
        Debug.Log(runPressed);
    }

    void FighitngMovement()
    {
        
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        // bool forwardPressed = Input.GetKey(KeyCode.W);
        // bool backPressed = Input.GetKey(KeyCode.S);
        // bool leftPressed = Input.GetKey(KeyCode.A);
        // bool rightPressed = Input.GetKey(KeyCode.D);

        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f;
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(cameraForward.x, cameraForward.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.SimpleMove(moveDir.normalized * Time.deltaTime);
        }





        
        // if(forwardPressed && velocityZ < 1f)
        // {
        //     velocityZ += Time.deltaTime * acceleration;
        // }
        // if(backPressed && velocityZ > -1f)
        // {
        //     velocityZ -= Time.deltaTime * acceleration;
        // }
        // if(leftPressed && velocityX > -1f)
        // {
        //     velocityX -= Time.deltaTime * acceleration;
        // }
        // if(rightPressed && velocityX < 1f)
        // {
        //     velocityX += Time.deltaTime * acceleration;
        // }
        // //RESET VELOCITY
        // if(!forwardPressed && velocityZ > 0.0f)
        // {
        //     velocityZ -= Time.deltaTime * deceleration;
        // }
        // if(!leftPressed && velocityX < 0.0f)
        // {
        //     velocityX += Time.deltaTime * deceleration;
        // }
        // if(!rightPressed && velocityX > 0.0f)
        // {
        //     velocityX -= Time.deltaTime * deceleration;
        // }
        // if(!backPressed && velocityZ < 0.0f)
        // {
        //     velocityZ += Time.deltaTime * deceleration;
        // }
        

        

        animator.SetFloat("Fighting Velocity Z", vertical);
        animator.SetFloat("Fighting Velocity X", horizontal);
        

    }

}

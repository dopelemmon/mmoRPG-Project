using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    Animator animator;
    CharacterController characterController;
    public Transform cam;

    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    float velocity;

    
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
        HandleMovement();

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

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.SimpleMove(moveDir.normalized * Time.deltaTime);
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
        if(!movementPressed && velocity >= 0f)
            velocity -= 2f * Time.deltaTime;
        
        animator.SetFloat("Velocity", velocity);
    }
}

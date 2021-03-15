using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    
    public CharacterController controller;
    public Transform cam;


    public float speed;
    public float runSpeed;
    private float slow;
    private float slowdown;
    public float slowdownRate;
    private Vector3 oldDir;
    private float inertia;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public Transform groundCheck;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    private bool isGrounded;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private Vector3 velocity;
    private bool wasMoving;


    void Start()
    {
        groundMask = ~groundMask;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //basic movement controls
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal,0f,vertical).normalized; //normalize limits the length of the vector to 1
        
        if (direction.magnitude >= 0.1f)//input is happening
        {
            float targetAngle = Mathf.Atan2(direction.x,direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y; //Atan2 returns the angle of a vector from 0 to (x,y)
            //also adds in the camera angle to make movement go in the right direction as a base
            
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); //smooths the angle 
            transform.rotation = Quaternion.Euler(0f, angle, 0f); //actually sets the angle

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; //adjusts angle of movement based on camera
            oldDir = moveDir;
            slow = speed;
            if (isGrounded) //move normally on the ground
            {
                
                if (Input.GetKey(KeyCode.LeftShift))
                {    
                    slow = slow*2;
                }
                
                //controller.Move(moveDir * slow * Time.deltaTime);
                
            }//else//lose momentum in the air smoothly
            {
                if (slow > 2)
                {
                    slow -= Time.deltaTime * (slowdownRate/3);
                }
                controller.Move(moveDir * slow * Time.deltaTime);
            }
            
        } else if (!isGrounded) //input is not happening so inertia needs to take over, at least in the air
        {
            if (slow > 0.2)
            {
                slow -= Time.deltaTime * (slowdownRate); //decay past input whenever not recieving input
            }
            controller.Move(oldDir * slow * Time.deltaTime);
        } else if (isGrounded)
        {
            slow = 0;
        }

        //jumping and gravity
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); //returns true if it collides with something
        
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -9f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump(jumpHeight);
        }
        velocity.y += gravity * Time.deltaTime;
        
        controller.Move(velocity*Time.deltaTime);
    }

    void Jump(float height)
    {
        velocity.y = Mathf.Sqrt(height * -2f * gravity);
    }
}

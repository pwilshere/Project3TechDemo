using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImprovedPlayerMove : MonoBehaviour
{
    private CharacterController controller;
    public Transform cam;

    public float walkSpeed;
    public float runSpeed;
    private float currentSpeed;
    public float gravity = -9.81f;
    public float jumpHeight;
    public float stoppingWeight;

    private Vector3 momentumVector;
    private Vector3 fallVector = Vector3.zero;
    private float momentumSpeed = 0;
    private float slowdownSpeed = 0;


    //turning variables
    private float turnSmoothVelocity;
    private float turnSmoothTime = 0.1f;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        //Application.targetFrameRate = 120;
    }

    // Update is called once per frame
    void Update()
    {
        //get input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        //normalize input
        Vector3 moveDirection = new Vector3(horizontal,0f,vertical).normalized;//.normalized;

        //adjust for camera angle when accepting input
        
        float targetAngle = Mathf.Atan2(moveDirection.x,moveDirection.z)*Mathf.Rad2Deg + cam.eulerAngles.y;
        float moveAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f,moveAngle,0f);
        moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        
        if (Mathf.Abs(horizontal) >= 0.1f || Mathf.Abs(vertical) >= 0.1f) //if there is any movement input
        {
            momentumVector = moveDirection;

            if (controller.isGrounded)
            {
                if (Input.GetKey(KeyCode.LeftShift)) //running
                {
                    momentumSpeed = runSpeed;
                } else //walking
                {
                    momentumSpeed = walkSpeed;
                }
            }
            controller.Move(momentumVector * momentumSpeed * Time.deltaTime); //finally apply movement
            slowdownSpeed = momentumSpeed;
        } else //not getting input
        {
            if (slowdownSpeed > 1)
            {
                slowdownSpeed -= stoppingWeight * Time.deltaTime;
                controller.Move(momentumVector * slowdownSpeed * Time.deltaTime);
            }
        }
        if (controller.isGrounded)
        {
            fallVector.y = -5;
        }
        
        //jumping code
        if (controller.isGrounded && Input.GetButtonDown("Jump"))
        {
            fallVector.y = Jump(jumpHeight);
        }
        if (fallVector.y > 0)
        {
            fallVector.y += gravity * Time.deltaTime;
        } else
        {
            fallVector.y += gravity * Time.deltaTime * 2;
        }
        
        controller.Move(fallVector*Time.deltaTime);
    }
    float Jump(float height)
    {
        return Mathf.Sqrt(height * -2f * gravity);
    }
}

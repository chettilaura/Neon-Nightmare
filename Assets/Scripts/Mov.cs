using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mov : MonoBehaviour
{
    private Animator animator;
    float moveSpeed;
    public float walkSpeed;
    public float dashSpeed;
    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    public float groundDrag;

    private float _jumpButtonPressedTime;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool readyToJump;

    private float _lastGroundedTime;

    public Transform groundCheck;
    public Material PlayerMaterial;
    public LayerMask Ground;
    bool isGrounded;

    public float maxSlopeAngle;
    public RaycastHit slopeHit;
    private bool exitingSlope;
    public bool canDoubleJump=false;


    Vector3 moveDir;
    Rigidbody RB;

    public MovementState state;

    public enum MovementState
    {
        walking,
        dashing,
        air,
        idle
    }

    public bool dashing;
    public delegate void ReturnOnGroundDelegate() ;
    public event ReturnOnGroundDelegate returnOnGroundEvent;

    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();    
        RB.freezeRotation = true;
        readyToJump = true;
    }
    
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if(horizontalInput!= 0 || verticalInput != 0)
        {
            animator.SetBool("isWalking", true);
            state = MovementState.walking;
        } else
        {
            animator.SetBool("isWalking", false);
            state = MovementState.idle;
        }
        //if (Input.GetKey(KeyCode.Space) && readyToJump && isGrounded)
         if (Input.GetKeyDown(KeyCode.Space) && readyToJump && isGrounded)
        {
            _jumpButtonPressedTime = Time.time;
            readyToJump = false;
            Jump();
            Debug.Log("jump");
            canDoubleJump=true;
            animator.SetBool("isJumping", true);
           
            //Invoke(nameof(ResetJump), jumpCooldown);
            //Debug.Log(RB.velocity);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && canDoubleJump==true &&  RB.velocity.y!=0f && !isGrounded){ //doppio salto
            animator.SetBool("doubleJump", true);
            RB.velocity = new Vector3(RB.velocity.x, /*RB.velocity.y*doubleJumpOffset*/ jumpForce, RB.velocity.z);
            Debug.Log("double jump");
            canDoubleJump=false;
        } 
    }

     public void Jump()
    {
        
        exitingSlope = true;
        RB.velocity = new Vector3(RB.velocity.x, jumpForce, RB.velocity.z);
    }

    /*
    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }
    */

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, Ground);
        MyInput();
        SpeedLimiter();
        StateHandler();

        if (state == MovementState.walking) 
        {
            RB.drag = groundDrag;
        }
        else
        {
            RB.drag = 1f;
        }
        
        if(RB.velocity.y < 0.5f && state == MovementState.air)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);
            animator.SetBool("doubleJump", false);
        }

        if(state == MovementState.dashing)
        {
            animator.SetBool("dashing", true);
        } else
        {
            animator.SetBool("dashing", false);
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void StateHandler()
    {
        if (dashing)
        {
            state = MovementState.dashing;
            moveSpeed = dashSpeed;
        }

        else if (isGrounded)
        {
            _lastGroundedTime = Time.time;
            animator.SetBool("isFalling", false);
            animator.SetBool("doubleJump", false);
            if (RB.velocity.magnitude != 0f)
            {
                moveSpeed = walkSpeed;

                //post salto
                readyToJump = true;
                exitingSlope = false;

                //hook
                if (returnOnGroundEvent != null)
                {
                    returnOnGroundEvent.Invoke();
                }
            }

        }

        else
        {
            state = MovementState.air;
        }
    }

    private void MovePlayer()
    {
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(OnSlope() && !exitingSlope)
        {

            RB.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (RB.velocity.y > 0)
            {
                RB.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        else if(isGrounded)
            RB.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
        else if(!isGrounded)
            RB.AddForce(moveDir.normalized * moveSpeed * 7f * airMultiplier, ForceMode.Force);

        RB.useGravity = !OnSlope();
    }

    private void SpeedLimiter()
    {

        if (OnSlope() && !exitingSlope)
        {
            if (RB.velocity.magnitude > moveSpeed)
            {
                RB.velocity = RB.velocity.normalized * moveSpeed;
            }
        }
        else {
            Vector3 flatVelocity = new Vector3(RB.velocity.x, 0f, RB.velocity.z);

       if (flatVelocity.magnitude > moveSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
                RB.velocity = new Vector3(limitedVelocity.x, RB.velocity.y, limitedVelocity.z);
            }
        }
        
    }

   

    public bool OnSlope()
    {
        if(Physics.Raycast(groundCheck.position, Vector3.down, out slopeHit, 0.3f, Ground))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return (angle < maxSlopeAngle && angle != 0);
        }
        return false;
    }

    public bool OnRamp()
    {
        if (Physics.Raycast(groundCheck.position, Vector3.down, out slopeHit, 0.3f, Ground))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return (angle < maxSlopeAngle && angle >= 30);
        }
        return false;
    }

    public Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDir, slopeHit.normal).normalized;
    }

}

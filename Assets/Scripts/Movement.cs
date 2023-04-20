using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
     //values to set in the inspector
    public Rigidbody RB;
    public float moveSpeed;
    public float walkSpeed;
    public float dashSpeed;
    public Transform orientation;
    public Transform playerRot;
    public float groundDrag;
    public float jumpForce;
    public float maxSlopeAngle;
    public float jumpCooldown;
    public int doubleJumpOffset;
    public float airMultiplier;
    public Transform groundCheck;
    public Material PlayerMaterial;
    public LayerMask Ground;
    public LayerMask Magnetic;
    
    //input and utilities movement variables
    float horizontalInput;
    float verticalInput;
    private Vector3 myNormal;
    private Vector3 moveDir;
    
    //bools for checking
    private bool readyToJump;
    public bool isGrounded;
    public bool isMagnetic;
    public bool canDoubleJump=false;
    private bool exitingSlope; //when jumping on the slop, used not remove speed limitation while jumping on the slope 
    public bool dashing;


     //raycasts
    public RaycastHit slopeHit;
    RaycastHit magneticHit;
    

    //movement states
    public MovementState state;
    public enum MovementState
    {
        walking,
        dashing,
        air,
        magnetic
    }

   

    
    void Start()
    {
        RB = GetComponent<Rigidbody>();
        RB.freezeRotation = true;
        readyToJump = true;
        isMagnetic = false;
        myNormal = playerRot.up;
    }
    

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && readyToJump && isGrounded)
        {
            readyToJump = false;
            Jump();
            canDoubleJump=true;
            Invoke(nameof(ResetJump), jumpCooldown);

        }else if ( Input.GetKeyDown(KeyCode.Space) && canDoubleJump==true &&  RB.velocity.y>0.1f && !isGrounded){ //doppio salto
                RB.velocity = new Vector3(RB.velocity.x, RB.velocity.y*doubleJumpOffset, RB.velocity.z);
                canDoubleJump=false;
        }
    }


    
    void Update()
    {
        //check between child object "groundCheck" & ground/magnetic layer
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, Ground);
        isMagnetic = Physics.CheckSphere(groundCheck.position, 0.2f, Magnetic);
        MyInput();
        SpeedLimiter();
        StateHandler();

        if (state == MovementState.walking || state == MovementState.magnetic) 
        {
            RB.drag = groundDrag;
        }
        else
        {
            RB.drag = 1f;
        }
            
    }



    private void FixedUpdate()
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
        else if(isMagnetic)
        {
            RB.AddForce(PlayerRotationMagnetic() * moveSpeed * 10f, ForceMode.Force);
        }
        else if(isGrounded)
            RB.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
        else if(!isGrounded)
            RB.AddForce(moveDir.normalized * moveSpeed * 7f * airMultiplier, ForceMode.Force);
        if(OnSlope() || isMagnetic)
        {
            RB.useGravity = false;
        }
        else RB.useGravity = true;
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
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        else if (isMagnetic)
        {
            state = MovementState.magnetic;
            moveSpeed = walkSpeed;
        }

        else
        {
            state = MovementState.air;
        }
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

    public void Jump()
    {
        exitingSlope = true;
        RB.velocity = new Vector3(RB.velocity.x, jumpForce, RB.velocity.z);
    }


    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }


    //NB in order to check the distance from the ground we use one ray that starts from the center of the player and goes down a bit more of its half height
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


    Vector3 PlayerRotationMagnetic()
    {
        Physics.Raycast(groundCheck.position, -groundCheck.up, out magneticHit, 0.3f, Magnetic);
        myNormal = Vector3.Lerp(myNormal, magneticHit.normal, 10f * Time.deltaTime);
        Vector3 myForward = Vector3.Cross(playerRot.right, myNormal);
        Quaternion targetRot = Quaternion.LookRotation(myForward, myNormal);
        playerRot.rotation = Quaternion.Lerp(playerRot.rotation, targetRot, 10f * Time.deltaTime);
        RB.AddForce(-50f * magneticHit.normal);
        return Vector3.ProjectOnPlane(moveDir, magneticHit.normal).normalized;
    }

}

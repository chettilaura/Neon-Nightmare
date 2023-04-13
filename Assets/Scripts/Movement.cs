using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    float moveSpeed;
    public float walkSpeed;
    public float dashSpeed;
    public Transform orientation;
    public Transform playerRot;
    Vector3 myNormal;

    float horizontalInput;
    float verticalInput;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    public Transform groundCheck;
    public Material PlayerMaterial;
    public LayerMask Ground;
    public LayerMask Magnetic;
    bool isGrounded;
    public bool isMagnetic;

    public float maxSlopeAngle;
    public RaycastHit slopeHit;
    RaycastHit magneticHit;
    private bool exitingSlope;

    Vector3 moveDir;
    Rigidbody RB;

    public MovementState state;

    public enum MovementState
    {
        walking,
        dashing,
        air,
        magnetic
    }

    public bool dashing;

    // Start is called before the first frame update
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
        if (Input.GetKey(KeyCode.Space) && readyToJump && isGrounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    // Update is called once per frame
    void Update()
    {
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

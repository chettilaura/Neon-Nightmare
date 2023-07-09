using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

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
    private bool atterraggio_post_salto=false;
    private bool flag_start_partita = true;


    Vector3 moveDir;
    Rigidbody RB;

    public MovementState state;
    private ClosestEnemyNearby _fire;

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

    [SerializeField] private AudioClip[] stoneClips;
    [SerializeField] AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();    
        RB.freezeRotation = true;
        readyToJump = true;
        _fire = GetComponent<ClosestEnemyNearby>();
    }
    
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if (isGrounded)
        {
            if (horizontalInput != 0 || verticalInput != 0)
            {
                state = MovementState.walking;
            }
            else
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
        RB.AddForce(0f, jumpForce, 0f, ForceMode.Impulse);
        //RB.velocity = new Vector3(RB.velocity.x, jumpForce, RB.velocity.z);
        
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
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.02f, Ground);

        MyInput();
        SpeedLimiter();
        animations();
        StateHandler();
        if (state == MovementState.idle)
        {
            RB.velocity = Vector3.zero;
        }
        if (state == MovementState.walking) 
        {
            RB.drag = groundDrag;
        }
        else
        {
            RB.drag = 1f;
        }

        Debug.Log("onSlope = " + OnSlope());
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
            readyToJump = true;
            moveSpeed = walkSpeed;

            if (RB.velocity.magnitude != 0f)
            {
                //moveSpeed = walkSpeed;

                //post salto

                exitingSlope = false;

                //hook
                if (returnOnGroundEvent != null)
                {
                    returnOnGroundEvent.Invoke();
                    //suono atterraggio
                    AudioClip clip2 = stoneClips[0];
                    audioSource.PlayOneShot(clip2);
                }
            }

        }

        else
        {
            state = MovementState.air;
            flag_start_partita = false;

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
        else if(!isGrounded){
            RB.AddForce(moveDir.normalized * moveSpeed * 7f * airMultiplier, ForceMode.Force);
            atterraggio_post_salto=true;
        }

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

    private void animations()
    {
        if (isGrounded)
        {
            animator.SetBool("isFalling", false);
            //animator.SetBool("isLanding", false);
            animator.SetBool("doubleJump", false);
            if (atterraggio_post_salto==true && flag_start_partita==false){
                //rumore atterraggio
                AudioClip clip = stoneClips[0];
                audioSource.PlayOneShot(clip);
                atterraggio_post_salto=false;
            }
        } else if (Physics.CheckSphere(transform.position , 0.5f, Ground) && animator.GetBool("isFalling"))
        {
            animator.SetBool("isFalling", false);
            animator.SetBool("doubleJump", false);
        }

        if (RB.velocity.y < 0f && state == MovementState.air)
        {
            animator.SetBool("isFalling", true);
            animator.SetBool("isJumping", false);
            animator.SetBool("doubleJump", false);
        }

        if(state == MovementState.walking)
        {
            animator.SetBool("isWalking", true);
        } else
            animator.SetBool("isWalking", false);

        if (dashing)
        {
            animator.SetBool("dashing", true);
            
        }
        else
        {
            animator.SetBool("dashing", false);
        }

        if(state == MovementState.idle)
            animator.SetBool("isIdle", true);
        else
            animator.SetBool("isIdle", false);

        if((animator.GetBool("isJumping") || animator.GetBool("isFalling")) && (animator.GetBool("grappingHookStart") || animator.GetBool("hook")))
        {
            animator.SetLayerWeight(1, 1);
        } else if(!_fire.fire)
        {
            animator.SetLayerWeight(1, 0);
        }

        if (animator.GetBool("isWalking") && ((animator.GetBool("grappingHookStart") || animator.GetBool("hook"))))
            animator.SetLayerWeight(1, 1);


    }



}

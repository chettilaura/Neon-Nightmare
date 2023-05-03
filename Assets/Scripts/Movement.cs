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
    private bool onSlope;

     //raycasts
    public RaycastHit slopeHit;
    RaycastHit magneticHit;
    private Vector3 magneticMovement;

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

/*
    
    void Update2()
    {
        //check between child object "groundCheck" & ground/magnetic layer
        
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


 void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(groundCheck.position, 0.8f);
    }
    */


//private bool pauseForEveryoneToSee;
private void FixedUpdate(){ //mette le forze
     if(onSlope && !exitingSlope)
        {
            Debug.Log("è su slope");
            RB.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (RB.velocity.y > 0)
            {
                Debug.Log("slope");
                RB.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        else if(isMagnetic)
        {
            Debug.Log("è magnetico");
            RB.AddForce(/*PlayerRotationMagnetic()*/-myNormal /** moveSpeed **/* 10f);
        }
        else if(isGrounded){
            Debug.Log("è a terra");
            RB.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if(!isGrounded){
            Debug.Log("è in aria");
            RB.AddForce(moveDir.normalized * moveSpeed * 7f * airMultiplier, ForceMode.Force);
        }
}



    private void Update()
    {        
        CheckSurface(); //è check di tutte le superfici : controlla superficie che ha sotto

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


        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        //isGrounded = Physics.CheckSphere(groundCheck.position, 0.8f, Ground);
       // isMagnetic = Physics.CheckSphere(groundCheck.position, 0.8f, Magnetic);
       /*
        if(!pauseForEveryoneToSee&&isMagnetic){
            pauseForEveryoneToSee=true;
        }else if(pauseForEveryoneToSee&&!isMagnetic){
            Debug.Break();
            pauseForEveryoneToSee=false;
        }
        */


        if(onSlope || isMagnetic)
        {
            RB.useGravity = false;
            Debug.Log("no garvity");
        }
        else {RB.useGravity = true;
        Debug.Log("si garvity");
        }
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

        if (onSlope && !exitingSlope)
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
   /* public bool OnSlope()
    {
        if(Physics.Raycast(groundCheck.position, Vector3.down, out slopeHit, 0.3f, Ground))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return (angle < maxSlopeAngle && angle != 0);
        }
        return false;
    }*/


    public bool OnRamp() //usato in dash
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
        return Vector3.ProjectOnPlane(moveDir, magneticHit.normal).normalized;
    }


    [SerializeField]
    private LayerMask envLayers;
    [SerializeField]
    private LookAround lookAroundScript;
    [SerializeField]
    private float lerpSpeed=1;
    [SerializeField]
    private float turnSpeed=90;

public delegate void ReturnOnGroundDelegate();
public event ReturnOnGroundDelegate returnOnGroundEvent;
    private void CheckSurface(){
    if(Physics.SphereCast(groundCheck.position,0.5f,-groundCheck.up,out magneticHit,0.5f,envLayers)){ //lancia raggio e incontra diverse superfici
    float angle = Vector3.Angle(Vector3.up, magneticHit.normal);
            if (angle < maxSlopeAngle && angle != 0){
                onSlope=true;
            }
        /*if(magneticHit.transform.gameObject.layer==LayerMask.NameToLayer("Slope")){
            // lookAroundScript.enabled=true;
             //onSlope=true;
             
        }*//*else if(magneticHit.transform.gameObject.layer==LayerMask.NameToLayer("Magnetic")){
            isMagnetic=true;
            isGrounded=false;
            onSlope=false;
            lookAroundScript.enabled=false;

       

              //  playerRot.rotation=Quaternion.LookRotation(magneticMovement, magneticHit.normal);

            myNormal = Vector3.Lerp(myNormal, magneticHit.normal, lerpSpeed*Time.deltaTime); //normale capsula si allinea a normale terreno
            playerRot.Rotate(0, horizontalInput*turnSpeed*Time.deltaTime, 0); // capsula ruota su se stessa e poi si muove con davanti e dietro

            Vector3 myForward = Vector3.Cross(playerRot.right, myNormal); // find forward direction with new myNormal (prodotto vettoriale): 
            Quaternion targetRot = Quaternion.LookRotation(myForward, myNormal); // align character to the new myNormal while keeping the forward direction:
            playerRot.rotation = Quaternion.Lerp(playerRot.rotation, targetRot, lerpSpeed*Time.deltaTime); //rotazione su piano precedente spostata su nuovo piano
            playerRot.Translate(0, 0, verticalInput*moveSpeed*Time.deltaTime); // move the character forth/back with Vertical axis:

        }*/else //if(magneticHit.transform.gameObject.layer==LayerMask.NameToLayer("Ground"))
        {
            lookAroundScript.enabled=true;
            isGrounded=true;
            if(returnOnGroundEvent!=null){
                returnOnGroundEvent.Invoke();
            }
            isMagnetic=false;
            onSlope=false;
        }
}else{
    lookAroundScript.enabled=true;
        isGrounded=false;
         isMagnetic=false;
        onSlope=false;
    }

}

/*
    Vector3 PlayerRotationMagnetic2()
    {
        Physics.Raycast(groundCheck.position, -groundCheck.up, out magneticHit, 0.8f, Magnetic);
        Debug.Log(magneticHit.transform.gameObject.name);
        Vector3 direction = Vector3.ProjectOnPlane(moveDir, magneticHit.normal).normalized;
        Debug.DrawRay(groundCheck.position, direction, Color.red);
        Vector3 magneticForce= -magneticHit.normal* 100f;
                playerRot.rotation=Quaternion.LookRotation(direction, magneticHit.normal);

      //  direction+=magneticForce;
        
        Physics.Raycast(groundCheck.position, -groundCheck.up, out magneticHit, 0.3f, Magnetic);
        myNormal = Vector3.Lerp(myNormal, magneticHit.normal, 10f * Time.deltaTime);
        Vector3 myForward = Vector3.Cross(playerRot.right, myNormal);
        Quaternion targetRot = Quaternion.LookRotation(myForward, myNormal);
        playerRot.rotation = Quaternion.Lerp(playerRot.rotation, targetRot, 10f * Time.deltaTime);
        RB.AddForce(-50f * magneticHit.normal);
        return Vector3.ProjectOnPlane(moveDir, magneticHit.normal).normalized;
        

         return direction.normalized;
    }
    */

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    public Transform orientation;
    Rigidbody RB;
    private Movement movement;

    public float dashForce;
    public float dashDuration;

    public float dashCooldown;
    float dashCooldownTimer;
    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody>();
        movement = GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            dash();

        if(dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }

    private Vector3 GetDirection(Transform orientation)
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 direction = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (verticalInput == 0 && horizontalInput == 0)
            direction = orientation.forward;

        return direction.normalized;
    }

    private void dash()
    {
        if (dashCooldownTimer > 0) return;
        else dashCooldownTimer = dashCooldown;
        movement.dashing = true;
        Vector3 direction = GetDirection(orientation);
        Vector3 dashVector = direction * dashForce;
        Vector3 rampDashVector = movement.GetSlopeMoveDirection() * dashForce;
        if(movement.OnRamp())
        {
            delayDash = rampDashVector;
        }
        else
        {
            delayDash = dashVector;
        }
        
        Invoke(nameof(DelayedDashForce), 0.025f);
        Invoke(nameof(resetDash), dashDuration);
    }

    private Vector3 delayDash;
    private void DelayedDashForce()
    {
        if (movement.OnRamp())
            movement.Jump();
        RB.AddForce(delayDash, ForceMode.Impulse);
    }

    private void resetDash()
    {
        movement.dashing=false;
    }

}

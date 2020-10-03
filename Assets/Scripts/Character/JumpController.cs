using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class JumpController : MonoBehaviour
{
    public float minJumpHeight = 3f;
    public float maxJumpHeight = 7f;
    public int numAirJumps = 1;

    protected float maxJumpForce;
    protected float minJumpForce;

    private Controller2D controller;

    public bool CanJump
    {
        get
        {
            return controller.Grounded || remainingAirJumps > 0;
        }
    }

    protected int remainingAirJumps;

    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<Controller2D>();
        remainingAirJumps = numAirJumps;

        RecalculateJumpTime();
        controller.onGrounded += OnGrounded;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && CanJump)
        {
            if (!controller.Grounded)
            {
                //We are in the air
                remainingAirJumps--;
            }

            RecalculateJumpTime();
            controller.StartJump(maxJumpForce);
        }

        if (Input.GetButtonUp("Jump"))
        {
            controller.StopJump(maxJumpForce - minJumpForce);
        }
    }

    void RecalculateJumpTime()
    {
        float gravitySign = Mathf.Sign(controller.gravity);
        maxJumpForce = -gravitySign * Mathf.Sqrt(gravitySign * 2 * controller.gravity * maxJumpHeight);
        minJumpForce = -gravitySign * Mathf.Sqrt(gravitySign * 2 * controller.gravity * minJumpHeight);
    }

    void OnGrounded()
    {
        ResetAirJumps();
    }

    void ResetAirJumps()
    {
        remainingAirJumps = numAirJumps;
    }
}

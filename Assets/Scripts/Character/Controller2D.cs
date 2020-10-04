using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D), typeof(Animator))]
public class Controller2D : MonoBehaviour
{
    public enum FaceDirection
    {
        Left = -1,
        Right = 1
    }
    public float gravity;

    [Header("Ground settings")]
    public int numGroundRays = 5;
    public float groundRayLength = 0.1f;
    public LayerMask groundMask;

    [Header("Facing")]
    public FaceDirection initialFaceDirection = FaceDirection.Right;

    public Rigidbody2D Body { get; private set; }
    public Collider2D Collider { get; private set; }
    public Animator Animator { get; private set; }
    public FaceDirection Facing { get; private set; }

    public delegate void OnGrounded();
    public OnGrounded onGrounded;

    public bool Grounded { get; private set; } = false;
    public Collider2D Ground { get; private set; }
    public bool LockVelocity { get; set; }
    public bool LockInput { get; set; }

    private Vector2 _horizontalMovement;
    private Vector2 _gravity;
    private Vector2 _jumpForce;

    private void Awake()
    {
        Body = GetComponent<Rigidbody2D>();
        Body.freezeRotation = true;
        Body.gravityScale = 0;

        Collider = GetComponent<Collider2D>();
        SetFaceDirection(initialFaceDirection);

        Animator = GetComponent<Animator>();
    }

    public void SetHorizontalMovement(float amount)
    {
        if(LockInput)
        {
            return;
        }

        _horizontalMovement = Vector2.right * amount;

        if(amount != 0)
        {
            SetFaceDirection();
        }
    }

    public void StartJump(float maxJumpForce)
    {
        if (LockInput)
        {
            return;
        }

        Vector2 velocity = Body.velocity;
        velocity.y =  maxJumpForce;

        Body.velocity = velocity;
    }

    public void StopJump(float jumpForceDifference)
    {
        if (LockInput)
        {
            return;
        }

        Vector2 velocity = Body.velocity;

        float gravitySign = Mathf.Sign(gravity);
        float velocitySign = Mathf.Sign(velocity.y);

        if (velocity.y != 0 && gravitySign != velocitySign 
            && Mathf.Abs(velocity.y) > Mathf.Abs(jumpForceDifference))
        {
            velocity.y = 0;
        }

        Body.velocity = velocity;
    }

    public void SetVelocity(Vector2 velocity)
    {
        if (LockInput)
        {
            return;
        }

        Body.velocity = velocity;
    }

    void FixedUpdate()
    {
        if(LockVelocity)
        {
            return;
        }

        Vector2 move = Body.velocity;
        move.x = 0;

        CheckGrounded();
        if(!Grounded)
        {
            move += gravity * Vector2.up * Time.fixedDeltaTime;
        }
        else
        {
            if (move.y - 0.0001f < 0)
            {
                move.y = 0;
            }
        }

        move += _horizontalMovement;

        Animator.SetInteger("MoveX", move.x == 0 ? 0 : (int)Mathf.Sign(move.x));
        Animator.SetInteger("MoveY", move.y == 0 ? 0 : (int)Mathf.Sign(move.y));

        Body.velocity = move;
    }

    private void CheckGrounded()
    {
        Grounded = false;
        Ground = null;
        float gravitySign = Mathf.Sign(gravity);
        Vector2 rayBaseOrigin = (Vector2)transform.position + Vector2.left * Collider.bounds.size.x / 2
            + gravitySign * Vector2.up * Collider.bounds.size.y / 2;
        float raySpacing = Collider.bounds.size.x / (numGroundRays - 1);

        for(int i = 0; i < numGroundRays; i++)
        {
            Vector2 origin = rayBaseOrigin + Vector2.right * raySpacing * i;
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.up * gravitySign, groundRayLength, groundMask);
            if (hit.collider && !hit.collider.isTrigger)
            {
                Grounded = true;
                Ground = hit.collider;
                Debug.DrawRay(origin, Vector2.up * gravitySign * groundRayLength, Color.green);

                onGrounded?.Invoke();
                break;
            }

            Debug.DrawRay(origin, Vector2.up * gravitySign * groundRayLength, Color.red);
        }
    }

    private void SetFaceDirection(FaceDirection defaultFacing)
    {
        Facing = defaultFacing;

        Vector3 scale = transform.localScale;
        scale.x *= (int)Facing;
        transform.localScale = scale;
    }

    private void SetFaceDirection()
    {
        if(_horizontalMovement.x < 0)
        {
            SetFaceDirection(FaceDirection.Left);
        }
        else
        {
            SetFaceDirection(FaceDirection.Right);
        }
    }
    
}

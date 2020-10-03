using UnityEngine;
using System.Collections;

public class DashController : MonoBehaviour
{
    public float distance = 5;
    public float duration = .3f;
    public float lostControlDuration = .15f;
    public int numAirDashes;

    public bool CanDash
    {
        get => !_dashing && (controller.Grounded || _numAirDashesRemaining > 0);
    }
    private Controller2D controller;
    private bool _dashing;
    private int _numAirDashesRemaining;

    void Awake()
    {
        controller = GetComponent<Controller2D>();
        controller.onGrounded += OnGrounded;
        ResetAirDashes();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Dash") && CanDash)
        {
            StartCoroutine(DashCrt(Vector2.zero));
        }
    }

    public IEnumerator DashCrt(Vector2 direction)
    {
        _dashing = true;
        float force = distance / duration;

        if (direction == Vector2.zero)
        {
            direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        if (direction == Vector2.zero)
        {
            direction.x = (int)controller.Facing;
        }

        direction.x = direction.x != 0 ? Mathf.Sign(direction.x) : 0;
        direction.y = direction.y != 0 ? Mathf.Sign(direction.y) : 0;

        direction.Normalize();

        SetAnimatorState(direction);

        controller.SetVelocity(direction * force);
        controller.LockVelocity = true;

        if(!controller.Grounded)
        {
            _numAirDashesRemaining--;
        }

        yield return new WaitForSeconds(lostControlDuration);
        controller.LockVelocity = false;

        float gravitySign = Mathf.Sign(controller.gravity);

        if (direction.y != 0 && gravitySign != Mathf.Sign(direction.y))
        {
            controller.StopJump(- gravitySign * force * (lostControlDuration / duration));
        }

        _dashing = false;
        controller.Animator.SetBool("Dash", _dashing);
    }

    private void SetAnimatorState(Vector2 direction)
    {
        controller.Animator.SetBool("Dash", _dashing);

        float angle = Vector2.Angle(Vector2.right, direction);
        if(angle > 90 && angle < 180)
        {
            angle = 90 - angle;
        }
        if(angle == 180)
        {
            angle = 0;
        }
        controller.Animator.SetFloat("DashDirection", Mathf.Sin(angle * Mathf.Deg2Rad));
    }

    private void OnGrounded()
    {
        ResetAirDashes();
    }

    private void ResetAirDashes()
    {
        _numAirDashesRemaining = numAirDashes;
    }
}

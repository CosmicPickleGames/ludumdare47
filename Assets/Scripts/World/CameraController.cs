using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(2000)]
public class CameraController : MonoBehaviour
{
    public Controller2D Follow { get; private set; }
    public float followDistance = 20;

    public float dampTime = 1;
    public float lookAhead = 10;

    public Bounds levelBounds = new Bounds();

    public Camera Camera {
        get
        {
            if(_camera == null)
            {
                _camera = GetComponent<Camera>();
            }

            return _camera;
        }
    }
    private Bounds _cameraBounds;
    private Camera _camera;
    private Vector3 _targetPosition;
    private Vector3 _targetVelocity;

    private void Awake()
    {
        Follow = Player.Instance.Controller;
        FollowTarget(true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FollowTarget();
    }

    private void FollowTarget(bool immediate = false)
    {
        if(immediate)
        {
            _targetPosition = Follow.transform.position;
            _targetPosition.z = -followDistance;
        }
        else
        {
            UpdateTargetPosition();
        } 

        transform.position = _targetPosition;
        Vector3 bottomLeft = Camera.ViewportToWorldPoint(new Vector3(0, 0, followDistance));
        Vector3 topRight = Camera.ViewportToWorldPoint(new Vector3(1, 1, followDistance));

        Vector3 correction = Vector2.zero;
        if (bottomLeft.x < levelBounds.min.x)
        {
            correction.x = levelBounds.min.x - bottomLeft.x;
        }
        else if(topRight.x > levelBounds.max.x)
        {
            correction.x = levelBounds.max.x - topRight.x;
        }

        if(bottomLeft.y < levelBounds.min.y)
        {
            correction.y = levelBounds.min.y - bottomLeft.y;
        }
        else if (topRight.y > levelBounds.max.y)
        {
            correction.y = levelBounds.max.y - topRight.y;
        }

        _targetPosition += correction;
        transform.position = _targetPosition;

        Vector3 size = topRight - bottomLeft;
        _cameraBounds = new Bounds(bottomLeft + size / 2 + correction, size);
    }

    private void UpdateTargetPosition()
    {
        float distanceX = Follow.Body.velocity.x;
        float directionX = distanceX != 0 ? Mathf.Sign(distanceX) : 0;
        Vector3 aheadPoint = Follow.transform.position + new Vector3(directionX, 0, 0) * lookAhead;
        Vector3 delta = aheadPoint - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, followDistance));
        Vector3 destination = transform.position + delta;

        _targetPosition = Vector3.SmoothDamp(transform.position, destination, ref _targetVelocity, dampTime, Mathf.Infinity, Time.fixedDeltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(levelBounds.center, levelBounds.size);

        if (Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(_cameraBounds.center, _cameraBounds.size);
        }
        else
        {
            Vector3 bottomLeft = Camera.ViewportToWorldPoint(new Vector3(0, 0, followDistance));
            Vector3 topRight = Camera.ViewportToWorldPoint(new Vector3(1, 1, followDistance));
            Vector3 size = topRight - bottomLeft;
            var cameraBounds = new Bounds(bottomLeft + size / 2, size);

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(cameraBounds.center, cameraBounds.size);
        }
    }
}

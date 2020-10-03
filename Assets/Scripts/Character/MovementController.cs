using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class MovementController : MonoBehaviour
{
    public float movementSpeed = 10;

    private Controller2D controller;
    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<Controller2D>();
    }

    // Update is called once per frame
    void Update()
    {
        controller.SetHorizontalMovement(Input.GetAxisRaw("Horizontal") * movementSpeed);
    }
}

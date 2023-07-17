using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{

    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = .4f;
    public LayerMask groundMask;


    Vector3 velocity;
    bool grounded;

    // Update is called once per frame
    void Update()
    {

        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(grounded && velocity.y < 0) {

            velocity.y = -2f;

        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 moveDir = transform.right * x + transform.forward * z;

        controller.Move(moveDir * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && grounded) {

            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}

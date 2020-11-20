using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed;
    public Rigidbody myRb;
    public Transform _groundChecker;
    public LayerMask Ground;
    public float GroundDistance = 0.2f;
    public float JumpHeight = 0.07f;
    private bool _isGrounded;


    private float gravity = -0.49f;
    private Vector3 _velocity;

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical);

        _isGrounded = Physics.CheckSphere(_groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);        
        

        _velocity.x = direction.x * speed * Time.deltaTime;
        _velocity.z = direction.z * speed * Time.deltaTime;

        if (_isGrounded && (_velocity.y <= 0))
            _velocity.y = 0;
        else
        {
            _velocity.y += gravity * Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _velocity.y += Mathf.Sqrt(JumpHeight * -0.01f * gravity);
        }
            controller.Move(_velocity);
       
    }
    private void FixedUpdate()
    {
        
    }
}

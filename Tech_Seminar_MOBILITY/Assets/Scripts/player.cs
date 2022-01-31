using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public CharacterController Controller;
    public Transform Cam;
    public Transform GroundCheck;
    public LayerMask GroundMask;
    public float Speed = 6f;
    public float TurnSmoothTime = 0.1f;

    public Animator Anim;
    public float AnimSpeedAdjust = 1.5f;


    private float _turnSmoothVelocity;
    private Vector3 _velocity;


    // Update is called once per frame
    void Update()

       
    {
        //Animation
        float animSpeed = Controller.velocity.magnitude;
        Anim.SetFloat("Forward", animSpeed / Speed / AnimSpeedAdjust);


        //Chech If Grounded and Reset Gravity
        bool isGrounded = Physics.CheckSphere(GroundCheck.position, 0.4f, GroundMask);
        if (isGrounded && _velocity.y <0) { _velocity.y = -2f; }

        //Get Input
        float h = Input.GetAxisRaw("Mouse X");
        float v = Input.GetAxisRaw("Mouse Y");
        Vector3 dir = new Vector3(h, 0, v).normalized;

        //Gravity
        _velocity.y += 9.81f * Time.deltaTime;
        Controller.Move(_velocity * Time.deltaTime);
        if(dir.magnitude<= 0.1f) { return; }


        //Rotatio
        float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + Cam.eulerAngles.y; // angles between two axis, based on camera
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, TurnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f); //rotate player



        //Movement
        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; // get move direction
        Controller.Move(moveDir.normalized * Speed * Time.deltaTime); //moving player
    }



}

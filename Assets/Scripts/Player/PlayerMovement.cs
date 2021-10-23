using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Movement Variables
    private Camera mainCamera;
    public float Speed = 10f;

    // Animation Variables
    Animator animator;
    private Vector3 moveDirection = Vector3.zero;
    public bool is_dead = false;

    public static PlayerMovement Instance;
    private CharacterController controller;

    void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        controller = this.GetComponent<CharacterController>();
        if (!Instance)
        {
            Instance = this;
        }
    }

    void Update()
    {
        if (is_dead == true)
        {
            Move(0, 0);
        }
        else
        {
            // Movement
            var x = Input.GetAxisRaw("Horizontal");
            var z = Input.GetAxisRaw("Vertical");

            Move(x, z);

            // Rotation
            Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, transform.position);
            float rayLength = 0.0f;

            if (groundPlane.Raycast(cameraRay, out rayLength))
            {
                Vector3 pointToLook = cameraRay.GetPoint(rayLength);
                Quaternion targetRotation = Quaternion.LookRotation(pointToLook - transform.position);
                targetRotation.x = 0;
                targetRotation.z = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 7f * Time.deltaTime);
            }
        }
    }

    private void Move(float x, float z)
    {
        moveDirection = new Vector3(x, 0, z);

        if (moveDirection.magnitude > 1.0f)
        {
            moveDirection = moveDirection.normalized;
        }

        moveDirection = transform.InverseTransformDirection(moveDirection);

        animator.SetFloat("VelX", moveDirection.x, 0.05f, Time.deltaTime);
        animator.SetFloat("VelZ", moveDirection.z, 0.05f, Time.deltaTime);

        //transform.position += (Vector3.forward * Speed) * z * Time.deltaTime;
        //transform.position += (Vector3.right * Speed) * x * Time.deltaTime;
        Vector3 V = new Vector3( Speed * x * Time.deltaTime, 0, Speed * z * Time.deltaTime);
        controller.Move(V);
    }
}
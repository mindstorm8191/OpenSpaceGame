using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    float moveSpeed = 0.1f;
    float jumpForce = 30f;
    float turn;
    float sensitivity = 3f;
    float gravity = 1f;
    float jumpPower = 30f;
    float jumpSpeed;

    // Start is called before the first frame update
    void Start()
    {
        turn = 0f;
        jumpSpeed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Manage look controls in Update, since it only really affects display
        turn += Input.GetAxis("Mouse X") * sensitivity;
        transform.localRotation = Quaternion.Euler(0, turn, 0);

        float distance = GetComponent<Collider>().bounds.extents.y;  // this is the same for up & down
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, distance + 1f);
        Debug.Log(isGrounded);

        if (isGrounded)
        {
            jumpSpeed = 0;
            if (Input.GetButtonDown("Jump"))
            {
                jumpSpeed = jumpPower;
            }
        }
        else
        {
            jumpSpeed -= gravity;
        }

        transform.Translate(Input.GetAxis("Horizontal") * moveSpeed, jumpSpeed, Input.GetAxis("Vertical") * moveSpeed);
    }
}

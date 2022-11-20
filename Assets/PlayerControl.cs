using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    float moveSpeed = 0.1f;
    float turn;
    float sensitivity = 3f;
    float gravity = 0.02f;
    float jumpPower = 1.5f;
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
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, distance + 0.4f);
        //Debug.Log(isGrounded);

        if (isGrounded)
        {
            //jumpSpeed = 0;
            if (Input.GetButtonDown("Jump"))
            {
                jumpSpeed = jumpPower;
                transform.Translate(0f, jumpSpeed, 0f);
                //Debug.Log("Jump!");
            }else{
                jumpSpeed = 0;
            }
        }
        else
        {
            // Raycast to the ground, to determine how far it is to reach it. If it's close enough, we need to
            // land on the ground, instead of going through it
            Debug.Log(jumpSpeed);
            RaycastHit hitInfo;
            bool groundNear = Physics.Raycast(transform.position, Vector3.down, out hitInfo, distance + 0.4f + jumpSpeed +gravity);
            if(groundNear && jumpSpeed<0) {
                //hitInfo.point.y tells us where we will stop falling at
                transform.Translate(0f, (transform.position.y-distance) -hitInfo.point.y, 0f);
                //if(jumpSpeed<0)
                jumpSpeed = 0;
            }else{
                transform.Translate(0f, jumpSpeed, 0f);
                jumpSpeed -= gravity;
            }
        }
        transform.Translate(Input.GetAxis("Horizontal") * moveSpeed, 0f, Input.GetAxis("Vertical") * moveSpeed);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    float moveSpeed = 4f;
    float turn;
    float sensitivity = 3f;
    float gravity = 9.81f;
    float jumpPower = 1.5f;
    float jumpSpeed;
    float jumpHeight = 3f;
    bool isCrouching = false;
    float crouchTime;
    float crouchPercent = 0;


    // This is a module specifically for controlling walking characters, including the player
    private CharacterController controller;

    private Vector3 playerVelocity;
    public bool groundedPlayer;

    // Start is called before the first frame update
    void Start()
    {
        turn = 0f;
        jumpSpeed = 0f;
        controller = gameObject.AddComponent<CharacterController>();
    }

    private float mySign(bool value) {
        // Returns 1 if true, -1 if false. An easy way to change the direction of something based solely on a bool value
        if(value) return 1; else return -1;
    }

    // Update is called once per frame
    void Update()
    {
        // Handle character direction based on mouse
        turn += Input.GetAxis("Mouse X") * sensitivity;
        transform.localRotation = Quaternion.Euler(0, turn, 0);

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
            playerVelocity.y = 0f;

        //if(Input.GetKey(KeyCode.LeftControl)) crouchPercent += 0.05f; else crouchPercent -= 0.05f;
        //Debug.Log(Mathf.Sign(Input.GetKey(KeyCode.LeftControl)));
        //crouchPercent = Mathf.Clamp(crouchPercent, 0, 1);
        crouchPercent = Mathf.Clamp(crouchPercent +mySign(Input.GetKey(KeyCode.LeftControl))*0.05f, 0, 1);
        transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one/2, crouchPercent);
        float speed = Mathf.Lerp(moveSpeed, moveSpeed/2, crouchPercent);
        /*
        if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new(1f, .7f, 1f), 4f * Time.deltaTime);
            speed /= 2;
        }
        else
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, 4f * Time.deltaTime);
        */

        Vector3 move = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;

        // Enable jumping
        if (Input.GetButtonDown("Jump") && groundedPlayer)
            playerVelocity.y += Mathf.Sqrt(jumpHeight * 3f * gravity);

        // Ensure the player is falling if they are not on the ground
        playerVelocity.y -= gravity * Time.deltaTime;

        controller.Move(speed * Time.deltaTime * move + playerVelocity * Time.deltaTime);
        /*
        // Manage look controls in Update, since it only really affects display
        turn += Input.GetAxis("Mouse X") * sensitivity;
        transform.localRotation = Quaternion.Euler(0, turn, 0);

        bool isGrounded = controller.isGrounded;
        if(isGrounded && jumpSpeed<0) jumpSpeed = 0; else jumpSpeed -= 0.005f;

            //Vector3 motion = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, 0f, Input.GetAxis("Vertical") * moveSpeed);
        Vector3 motion = Input.GetAxis("Horizontal") * moveSpeed * transform.right + Input.GetAxis("Vertical") * moveSpeed * transform.forward;
        
            // Keep forward & lateral movements from exceeding normal walking speed
        Vector3.ClampMagnitude(motion, moveSpeed);
        controller.Move(motion * Time.deltaTime);


        // Also handle applying vertical motion
        if(isGrounded && Input.GetButtonDown("Jump")) jumpSpeed = 0.5f;
        controller.Move(new Vector3(0f, jumpSpeed, 0f));
        */
    }
}

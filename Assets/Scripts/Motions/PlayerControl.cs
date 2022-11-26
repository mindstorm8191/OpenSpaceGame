using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float sprintSpeedOffset = 3f;
    public float turn = 0f;
    public float sensitivity = 3f;
    public float gravity = 9.81f;
    public float jumpHeight = 2f;
    public float jumpTime = 4f;
    public bool groundedPlayer = false;

    public bool jetpackEquipped = true; // Allows jetpack-assisted jumps
    public float jetpackMaxFuel = 2.0f; // time it takes to recharge (not use) jetpack
    public float jetpackFuel;

    private float crouchPercent = 0f;
    private CharacterController controller;
    private Vector3 playerVelocity;

    void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
        controller.minMoveDistance = 0f;

        jetpackFuel = jetpackMaxFuel;
    }

    // Returns 1 if true, -1 if false. An easy way to change the direction of something based solely on a bool value
    private float Sign(bool value) => value ? 1f : -1f;
    void Update()
    {
        // Enabling rotation of player through mouse
        turn += Input.GetAxis("Mouse X") * sensitivity;
        transform.localRotation = Quaternion.Euler(0f, turn, 0f);

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0f)
            playerVelocity.y = 0f;

        // Handling sneaking
        crouchPercent = Mathf.Clamp(crouchPercent + Sign(Input.GetKey(KeyCode.LeftControl)) * Time.deltaTime * 3.75f, 0, 1);
        transform.localScale = Vector3.Lerp(Vector3.one, new(1f, .7f, 1f), crouchPercent);
        transform.localPosition -= new Vector3(0f, transform.localPosition.y, 0f);
        float speed = Mathf.Lerp(moveSpeed, moveSpeed / 2, crouchPercent);


        // Handling sprinting
        if (Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
            speed += sprintSpeedOffset;

        Vector3 move = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;

        // Constant increment of the fall speed
        // Applying gravity
        // Jumping & physics
        if (Input.GetButtonDown("Jump") && groundedPlayer) {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * 3f * gravity);
        }
        if(Input.GetButton("Jump") && !groundedPlayer) {
            if(jetpackFuel>0) {
                playerVelocity.y += 17 * Time.deltaTime;
                jetpackFuel -= Time.deltaTime * 2.5f;
            }
        }else{
            // Recharge jetpack
            jetpackFuel += Time.deltaTime;
        }
        // limit jetpack fuel to ranges
        jetpackFuel = Mathf.Clamp(jetpackFuel, 0, jetpackMaxFuel);
        Debug.Log(jetpackFuel);

        if (!groundedPlayer)
            jumpTime += Time.deltaTime / 30f;
        else
            jumpTime = 0f;

        const float fallIncrement = 1f;
        playerVelocity.y -= fallIncrement * gravity * Time.deltaTime;
        //playerVelocity.y -= fallIncrement * gravity * Mathf.Pow(jumpTime == 0f ? Time.deltaTime : jumpTime, 2);
        controller.Move(speed * Time.deltaTime * move + playerVelocity * Time.deltaTime);
    }
}
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float turn;
    public float sensitivity = 3f;
    public float gravity = 9.81f;
    public float jumpHeight = 4f;
    public float jumpTime = 4f;
    public bool groundedPlayer = false;

    private CharacterController controller;
    private Vector3 playerVelocity;

    void Start()
    {
        turn = 0f;
        controller = gameObject.AddComponent<CharacterController>();
        controller.minMoveDistance = 0f;
    }

    void Update()
    {
        // Enabling rotation of player through mouse
        turn += Input.GetAxis("Mouse X") * sensitivity;
        transform.localRotation = Quaternion.Euler(0, turn, 0);

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
            playerVelocity.y = 0f;

        float speed = moveSpeed;
        // Handling sneaking/sprinting
        if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new(1f, .7f, 1f), 4f * Time.deltaTime);
            speed /= 2;
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, 4f * Time.deltaTime);
            if (Input.GetKey(KeyCode.LeftShift))
                speed *= 1.5f;
        }

        Vector3 move = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;

        if (Input.GetButtonDown("Jump") && groundedPlayer)
            playerVelocity.y += Mathf.Sqrt(jumpHeight * 3f * gravity);

        if (!groundedPlayer)
            jumpTime += Time.deltaTime / 30f;
        else
            jumpTime = 0f;

        // Constant increment of the fall speed
        const float fallIncrement = .05f;
        // Applying gravity
        playerVelocity.y -= fallIncrement * gravity * (jumpTime == 0f ? Time.deltaTime : jumpTime);
        controller.Move(speed * Time.deltaTime * move + playerVelocity * Time.deltaTime);
    }
}
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float sensitivity = 4f;
    public float turn;
    public float clampAngle = 80f;
    // Start is called before the first frame update

    void Start()
    {
        turn = 0;
        Cursor.lockState = CursorLockMode.Locked; // hides the cursor
    }

    // Update is called once per frame
    void Update()
    {
        turn += Input.GetAxis("Mouse Y") * sensitivity;
        transform.localRotation = Quaternion.Euler(Mathf.Clamp(-turn, -clampAngle, clampAngle), 0, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    float sensitivity = 4f;
    float turn;
    float clampAngle = 80f;
    // Start is called before the first frame update

    private bool hasFired;

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

        // Decide when to shoot. We can emulate a semi-automatic gun by checking for mouseUp before firing again
        if(hasFired) {
            if(!Input.GetMouseButtonDown(0)) hasFired = false;
        }else{
            if(Input.GetMouseButtonDown(0)) {
                hasFired = true;

                // Get a point in front of the camera, where the bullet spawns from
                Camera looker = GetComponent<Camera>();
                Vector3 firePoint = looker.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 1f));
                RaycastHit hitInfo;
                if(Physics.Raycast(firePoint, transform.rotation * new Vector3(0,0,1f), out hitInfo, 500)) {
                    // Now determine what got hit. I think we typically determine this based on gameObject names. But
                    // right now we only have one target to shoot at
                    if(hitInfo.collider.gameObject.TryGetComponent<BasicEnemy1>(out BasicEnemy1 target)) {
                        target.takeDamage(1f);
                    }
                }else{
                    Debug.Log("Your aim sucks!");
                }
            }
        }

    }
}

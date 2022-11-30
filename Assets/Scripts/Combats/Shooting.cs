using System.Collections;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private bool hasFired = false;
    public AudioClip pistolShot, pistolReload;
    public AudioSource audioSource;
    public Camera cam;

    // Update is called once per frame
    void Update()
    {
        // Decide when to shoot. We can emulate a semi-automatic gun by checking for mouseUp before firing again
        if (hasFired)
        {
            if (!Input.GetMouseButtonDown(0)) hasFired = false;
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                hasFired = true;

                // Start the sound effects. The reload sound will play right after the shot sound. We will
                // use a coRoutine to wait until the first sound finishes before starting the second
                StartCoroutine(GunSound());

                // Get a point in front of the camera, where the bullet spawns from
                Vector3 firePoint = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 1f));
                if (Physics.Raycast(firePoint, transform.rotation * new Vector3(0, 0, 1f), out RaycastHit hitInfo, 500))
                {
                    // Now determine what got hit. I think we typically determine this based on gameObject names. But
                    // right now we only have one target to shoot at
                    if (hitInfo.collider.gameObject.TryGetComponent<BasicEnemy1>(out BasicEnemy1 target))
                        target.TakeDamage(1f);
                }

            }
        }
    }

    public IEnumerator GunSound()
    {
        audioSource.clip = pistolShot;
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        audioSource.clip = pistolReload;
        audioSource.Play();
    }
}
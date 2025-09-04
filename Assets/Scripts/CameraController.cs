using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float smoothSpeed = 2f;

    private Transform target;
    private Vector3 velocity = Vector3.zero;
    private bool shouldFocus = false;
    private Vector3 offset;

    private CameraMovement cameraMovement;

    void Start()
    {
        PlayerCharacter referencePlayer = FindFirstObjectByType<PlayerCharacter>();
        if (referencePlayer != null)
        {
            offset = transform.position - referencePlayer.transform.position;
        }

        cameraMovement = GetComponent<CameraMovement>();
    }

    void Update()
    {
        if (shouldFocus && target != null)
        {
            Vector3 targetPos = target.position + offset;
            transform.position = Vector3.SmoothDamp(
                transform.position,
                targetPos,
                ref velocity,
                1f / smoothSpeed
            );

            if (Vector3.Distance(transform.position, targetPos) < 0.05f)
            {
                shouldFocus = false;
                target = null;
            }
        }
    }

    public void FocusOnPlayer(Transform player)
    {
        if (cameraMovement != null) cameraMovement.enabled = false; //turn off WASD
        target = player;
        shouldFocus = true;
    }

    public void EnableWASD()
    {
        if (cameraMovement != null) cameraMovement.enabled = true; // turn on WASD after dialogue ends
    }
}

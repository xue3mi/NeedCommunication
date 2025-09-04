using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float cameraSpeed = 5;

    // boarder
    public float minX = -15f;
    public float maxX = 15f;
    public float minY = -20f;
    public float maxY = 20f;

    private DialogueManager dialogueManager;

    private void Start()
    {
        // reference DialogueManager
        dialogueManager = FindFirstObjectByType<DialogueManager>();
    }

    private void Update()
    {
        // if UI shown, ignore camera movement
        if (dialogueManager != null && dialogueManager.IsDialogueActive())
        {
            return;
        }

        Vector3 move = Vector3.zero;

        // WASD
        if (Input.GetKey(KeyCode.W))
        {
            move += new Vector3(0, cameraSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            move += new Vector3(0, -cameraSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            move += new Vector3(-cameraSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            move += new Vector3(cameraSpeed * Time.deltaTime, 0, 0);
        }

        transform.position += move;

        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}

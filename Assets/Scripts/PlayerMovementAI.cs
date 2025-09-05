using UnityEngine;

public class PlayerMovementAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float changeDirTime = 2f;
    public float minX = -15f;
    public float maxX = 15f;
    public float minY = -20f;
    public float maxY = 20f;

    private Vector3 moveDirection;
    private float timer;
    private bool isStopped = false;

    private void Start()
    {
        PickRandomDirection();
    }

    private void Update()
    {
        if (isStopped) return;

        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            PickRandomDirection();
        }

        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    private void PickRandomDirection()
    {
        float angle = Random.Range(0f, 360f);
        moveDirection = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0).normalized;
        timer = changeDirTime;
    }

    public void StopMovement()
    {
        isStopped = true;
    }

    public void ResumeMovement()
    {
        isStopped = false;
    }

    private void OnMouseDown()
    {
        StopMovement();
        DialogueManager dm = FindFirstObjectByType<DialogueManager>();
        if (dm != null && !dm.IsDialogueActive())
        {
            dm.OpenDialogue();
        }
    }
}

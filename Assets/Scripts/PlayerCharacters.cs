using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private DialogueManager dialogueManager;
    private CameraController cameraController;

    void Start()
    {
        dialogueManager = FindFirstObjectByType<DialogueManager>();
        cameraController = FindFirstObjectByType<CameraController>();
    }

    void OnMouseDown()
    {
        if (dialogueManager != null && dialogueManager.IsDialogueActive())
        {
            Debug.Log("Dialogue active, block PlayerCharacter click.");
            return;
        }

        if (dialogueManager != null)
        {
            dialogueManager.StartDialogue();
            Debug.Log("Player clicked, dialogue started.");

            if (cameraController != null)
            {
                cameraController.FocusOnPlayer(transform);
            }
        }
    }
}

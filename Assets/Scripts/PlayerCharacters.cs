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
        if (dialogueManager != null && !dialogueManager.IsDialogueActive())
        {
            dialogueManager.StartDialogue();
            Debug.Log("Player clicked, dialogue started.");

            if (cameraController != null)
            {
                cameraController.FocusOnPlayer(transform);
            }
        }
        else
        {
            Debug.Log("Dialogue already active, ignore player click.");
        }
    }
}

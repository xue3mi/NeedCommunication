using UnityEngine;

public class PlayerClick : MonoBehaviour
{
    private DialogueManager dialogueManager;

    void Start()
    {
        dialogueManager = Object.FindFirstObjectByType<DialogueManager>();
    }

    void OnMouseDown()
    {
        // if UI dialogue is already shown, ignore click
        if (dialogueManager != null && !dialogueManager.IsDialogueActive())
        {
            dialogueManager.StartDialogue();
            Debug.Log("Player clicked, dialogue started.");
        }
        else
        {
            Debug.Log("Dialogue already active, ignore player click.");
        }
    }
}

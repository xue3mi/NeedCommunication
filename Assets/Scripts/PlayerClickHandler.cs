using UnityEngine;

public class PlayerClickHandler : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public ArrowNavigator arrowNavigator;

    private PlayerData playerData;

    void Start()
    {
        playerData = GetComponent<PlayerData>();
    }

    void OnMouseDown()
    {
        //hide arrow
        if (arrowNavigator != null)
        {
            arrowNavigator.HideArrow();
        }

        //tell DialogueManager which prefab is clicked
        DialogueManager dm = FindFirstObjectByType<DialogueManager>();
        if (dm != null)
        {
            dm.SetCurrentPlayer(playerData);
            dm.OpenDialogue();
        }
    }
}

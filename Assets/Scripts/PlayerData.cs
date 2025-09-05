using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PlayerData : MonoBehaviour
{
    [SerializeField] public string who;
    [SerializeField] public string where;
    [SerializeField] public string doAction;

    [SerializeField] public List<string> chosenComplements = new List<string>();
    [SerializeField] public List<string> chosenCriticisms = new List<string>();

    [Header("Player Renderer & Sprites")]
    public SpriteRenderer playerRenderer;
    public Sprite smileSprite;
    public Sprite angrySprite;
    public Sprite defaultSprite;

    public string GetSentence()
    {
        return $"{who} {where} {doAction}";
    }

    // change sprite
    public void SetFaceSmile()
    {
        if (playerRenderer != null) playerRenderer.sprite = smileSprite;
    }

    public void SetFaceAngry()
    {
        if (playerRenderer != null) playerRenderer.sprite = angrySprite;
    }

    public void ClearFace()
    {
        if (playerRenderer != null) playerRenderer.sprite = defaultSprite;
    }

private void OnValidate()
    {
        // Inspector always refresh the newest value
    }
}

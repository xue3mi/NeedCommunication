using UnityEngine;

[ExecuteAlways]
public class PlayerData : MonoBehaviour
{
    [SerializeField] public string who;
    [SerializeField] public string where;
    [SerializeField] public string doAction;

    public string GetSentence()
    {
        return $"{who} {where} {doAction}";
    }

    // if change, refresh Inspector
    private void OnValidate()
    {
        // Inspector always refresh the newest value
    }
}

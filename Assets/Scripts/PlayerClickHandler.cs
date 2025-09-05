using UnityEngine;

public class PlayerClickHandler : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public ArrowNavigator arrowNavigator;

    void OnMouseDown()
    {
        if (arrowNavigator != null)
        {
            arrowNavigator.HideArrow();
        }
    }
}

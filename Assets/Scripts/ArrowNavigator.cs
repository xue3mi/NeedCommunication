using UnityEngine;
using UnityEngine.EventSystems;

public class ArrowNavigator : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public Transform target;
    public RectTransform arrowUI;

    private bool isVisible = false;

    void Start()
    {
        if (arrowUI != null) arrowUI.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!isVisible || target == null || arrowUI == null) return;

        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Vector3 targetScreenPos = Camera.main.WorldToScreenPoint(target.position);
        Vector3 dir = targetScreenPos - screenCenter;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        arrowUI.rotation = Quaternion.Euler(0, 0, angle - 90f);

        arrowUI.position = screenCenter;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        if (arrowUI != null)
        {
            arrowUI.gameObject.SetActive(true);
            isVisible = true;
        }
    }

    public void HideArrow()
    {
        if (arrowUI != null)
        {
            arrowUI.gameObject.SetActive(false);
            isVisible = false;
        }
    }
}

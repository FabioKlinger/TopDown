using UnityEngine;

public class RectTransformToggle : MonoBehaviour
{
    [Tooltip("Das UI-Element mit RectTransform")]
    public RectTransform target;

    [Header("Ziel-Skalierung")]
    public Vector3 targetScaleMultiplier = new Vector3(2, 2, 2);

    private Vector3 originalPosition;
    private Vector3 originalScale;
    private Vector2 originalAnchorMin;
    private Vector2 originalAnchorMax;
    private Vector2 originalPivot;

    private bool isTransformed = false;

    void Start()
    {
        if (target != null)
        {
            originalPosition = target.anchoredPosition3D;
            originalScale = target.localScale;
            originalAnchorMin = target.anchorMin;
            originalAnchorMax = target.anchorMax;
            originalPivot = target.pivot;
        }
    }

    void Update()
    {
        if (target == null) return;

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!isTransformed)
            {
                // In die Mitte setzen
                target.anchorMin = new Vector2(0.5f, 0.5f);
                target.anchorMax = new Vector2(0.5f, 0.5f);
                target.pivot = new Vector2(0.5f, 0.5f);
                target.anchoredPosition3D = Vector3.zero;

                // Skalieren
                Vector3 newScale = new Vector3(
                    originalScale.x * targetScaleMultiplier.x,
                    originalScale.y * targetScaleMultiplier.y,
                    originalScale.z * targetScaleMultiplier.z
                );
                target.localScale = newScale;

                isTransformed = true;
            }
            else
            {
                // Zurücksetzen
                target.anchorMin = originalAnchorMin;
                target.anchorMax = originalAnchorMax;
                target.pivot = originalPivot;
                target.anchoredPosition3D = originalPosition;
                target.localScale = originalScale;

                isTransformed = false;
            }
        }
    }
}
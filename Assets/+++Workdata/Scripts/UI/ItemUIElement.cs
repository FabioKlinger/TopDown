using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ItemUIElement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text_itemInfo;

    [SerializeField] private float fadeInTime = 0.5f;
    [SerializeField] private float displayTime = 1.5f;
    [SerializeField] private float fadeOutTime = 1f;
    
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetItemInfo(int value, string itemName)
    {
        string valueSign = value < 0 ? "-" : "+";
        text_itemInfo.SetText($"{valueSign} {value} {itemName}");

        StartCoroutine(FadeSequence());
    }
    
    IEnumerator FadeSequence()
    {
        canvasGroup.alpha = 0;

        float tIn = 0f;
        // Fade In
        while (tIn < fadeInTime)
        {
            tIn += Time.unscaledDeltaTime;
            canvasGroup.alpha = tIn / fadeInTime;
            yield return null;
        }

        canvasGroup.alpha = 1;

        yield return new WaitForSeconds(displayTime);
        
        float tOut = 0;
        // Fade out
        while (tOut < fadeOutTime)
        {
            tOut += Time.unscaledDeltaTime;
            canvasGroup.alpha = 1f - tOut / fadeInTime;
            yield return null;
        }

        canvasGroup.alpha = 0;
        Destroy(gameObject);
    }
}
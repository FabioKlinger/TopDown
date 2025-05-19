using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    public Transform player;
    public Animator fadeAnimator;
    public float freezeDuration;


    private void OnEnable()
    {
        player = FindFirstObjectByType<PlayerController>().transform;
    }

    public void TeleportPlayerToPs(Transform teleportPos)
    {
        StartCoroutine(TeleportWithPause(teleportPos));
    }

    private IEnumerator TeleportWithPause(Transform teleportPos)
    {
        
        if (fadeAnimator != null)
            fadeAnimator.SetTrigger("FadeIn"); 

        
        Time.timeScale = 0f;

       
        player.position = teleportPos.position;

        
        float t = 0f;
        while (t < freezeDuration)
        {
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        
        Time.timeScale = 1f;

        
        if (fadeAnimator != null)
            fadeAnimator.SetTrigger("FadeOut");
    }
}

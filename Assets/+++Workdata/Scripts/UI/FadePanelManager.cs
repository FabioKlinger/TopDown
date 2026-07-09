using System;
using UnityEngine;

public class FadePanelManager : MonoBehaviour
{
    public static FadePanelManager Instance;
    public Animator fadePanelAnim;


    private void Awake()
    {
        Instance = this;
    }

    public void FadeIn()
    {
        fadePanelAnim.Play("FadeIn");
    }

    public void FadeOut()
    {
        fadePanelAnim.Play("FadeOut");
    }
}

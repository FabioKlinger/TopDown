using System;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    public static TeleportManager Instance;
    
    private Transform player;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        player = FindFirstObjectByType<PlayerController>().transform;
    }

    public void TeleportPlayerToPos(Transform teleportPos)
    {
        player.position = teleportPos.position;
    }
}
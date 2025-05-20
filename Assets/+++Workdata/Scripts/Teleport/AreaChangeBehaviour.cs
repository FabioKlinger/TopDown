using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class AreaChangeBehaviour : MonoBehaviour
{
    public CinemachineCamera newCamera;
    public Transform teleportPos;
    
    public void StartTeleport()
    {
        StartCoroutine(InitiateTeleport());
    }

    IEnumerator InitiateTeleport()
    {
        FadePanelManager.Instance.FadeIn();
        yield return new WaitForSeconds(1f);
        
        CameraManager.Instance.ResetCurrentCamera();
        TeleportManager.Instance.TeleportPlayerToPos(teleportPos);
        CameraManager.Instance.SetNewCamera(newCamera, teleportPos.position);
        yield return new WaitForSeconds(1f);
        FadePanelManager.Instance.FadeOut();
    }
}
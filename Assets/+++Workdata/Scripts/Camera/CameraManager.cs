using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    
    public CinemachineCamera currentCamera;

    private Transform player;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        player = FindFirstObjectByType<PlayerController>().transform;
    }

    public void SetNewCamera(CinemachineCamera newCamera, Vector3 newPos)
    {
        //ResetCurrentCamera(); // Reset old camera
        newPos.z = -10;
        newCamera.transform.position = newPos;
        newCamera.Priority.Value = 10;
        newCamera.Follow = player;

        currentCamera = newCamera; // set new camera
    }

    public void ResetCurrentCamera()
    {
        currentCamera.Priority.Value = 0;
        currentCamera.Follow = null;
    }
}
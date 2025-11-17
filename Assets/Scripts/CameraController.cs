using UnityEngine;
using Unity.Cinemachine;

public class CameraController : Singleton<CameraController>
{
   private CinemachineCamera cinemachineCamera;
   public void SetPlayerCameraFollow()
    {
        cinemachineCamera = FindObjectOfType<CinemachineCamera>();
        cinemachineCamera.Follow = PlayerController.Instance.transform;
    }
}

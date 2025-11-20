using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    private CinemachineCamera cinemachineCamera;

    private void Awake()
    {
        cinemachineCamera = FindObjectOfType<CinemachineCamera>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene s, LoadSceneMode m)
    {
        SetPlayerCameraFollow();
    }

    public void SetPlayerCameraFollow()
    {
        if (cinemachineCamera == null)
            cinemachineCamera = FindObjectOfType<CinemachineCamera>();

        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
            cinemachineCamera.Follow = player.transform;
        else
            Debug.LogWarning("CameraController: Không tìm thấy Player trong scene!");
    }
}

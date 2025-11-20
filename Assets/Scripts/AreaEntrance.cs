using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AreaEntrance : MonoBehaviour
{
    [SerializeField] private string transitionName;

    private void Start()
    {
        // So sánh với transition từ SceneManagement
        if (SceneManagement.Instance.SceneTransitionName == transitionName)
        {
            // Tìm player
            var player = PlayerController.Instance;
            if (player != null)
            {
                player.transform.position = transform.position;
            }

            // Set camera follow
            var camCtrl = FindObjectOfType<CameraController>();
            if (camCtrl != null)
            {
                camCtrl.SetPlayerCameraFollow();
            }

            // Fade UI
            UIFade.Instance?.FadeToClear();
        }
    }
}

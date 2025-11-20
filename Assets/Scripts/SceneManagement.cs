using UnityEngine;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement Instance { get; private set; }

    public string SceneTransitionName { get; private set; }

    private void Awake()
    {
        // Singleton đúng chuẩn
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetTransitionName(string sceneTransitionName)
    {
        SceneTransitionName = sceneTransitionName;
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;

    [SerializeField] private GameObject gameOverPanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
       Time.timeScale = 1f;

    // 🔥 TẮT TẤT CẢ UI TRƯỚC KHI LOAD LẠI SCENE
    if (gameOverPanel != null)
        gameOverPanel.SetActive(false);

    var died = FindObjectOfType<YouDiedEffect>();
    if (died != null)
        died.ForceHidePanel();

    // Load lại scene
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

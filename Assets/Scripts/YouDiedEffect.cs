using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class YouDiedEffect : MonoBehaviour
{
    public static YouDiedEffect Instance;

    [SerializeField] private GameObject panel;
    [SerializeField] private Image bg;
    [SerializeField] private TMP_Text youDiedText;
    [SerializeField] private float fadeBGTime = 1.5f;
    [SerializeField] private float fadeTextTime = 2f;
    [SerializeField] private float slowMotionTime = 1f;
    [SerializeField] private AudioSource audioSource;

    private bool hasPlayed = false;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void PlayEffect()
    {
        if (hasPlayed) return;      // NGĂN NHÁY
        hasPlayed = true;

        panel.SetActive(true);

        // reset alpha
        bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, 0f);
        youDiedText.color = new Color(youDiedText.color.r, youDiedText.color.g, youDiedText.color.b, 0f);

        audioSource.Play();
        StartCoroutine(EffectRoutine());
    }

    private IEnumerator EffectRoutine()
    {
        // slow motion
        Time.timeScale = 0.2f;
        yield return new WaitForSecondsRealtime(slowMotionTime);
        Time.timeScale = 0f;

        // Fade BG
        float t = 0;
        Color bgColor = bg.color;

        while (t < fadeBGTime)
        {
            t += Time.unscaledDeltaTime;
            float a = Mathf.Lerp(0, 0.85f, t / fadeBGTime);
            bg.color = new Color(bgColor.r, bgColor.g, bgColor.b, a);
            yield return null;
        }

        // Fade YOU DIED
        t = 0;
        Color txtColor = youDiedText.color;

        while (t < fadeTextTime)
        {
            t += Time.unscaledDeltaTime;
            float a = Mathf.Lerp(0, 1, t / fadeTextTime);
            youDiedText.color = new Color(txtColor.r, txtColor.g, txtColor.b, a);
            yield return null;
        }

        // Delay then show restart menu
        yield return new WaitForSecondsRealtime(2f);
        GameOverManager.Instance.ShowGameOver();
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class YouDiedEffect : MonoBehaviour
{
    public static YouDiedEffect Instance;

    [Header("UI Elements")]
    public GameObject panel;        // YouDiedPanel
    public Image bg;                // BG
    public TMP_Text diedText;       // YouDiedText
    public AudioSource deadSound;   // optional

    [Header("Timings")]
    public float fadeBgTime = 1.2f;
    public float fadeTextTime = 1.4f;
    public float delayBeforeGameOver = 2f;

    private void Awake()
    {
        // Singleton an toàn
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Lấy lại reference nếu Inspector chưa điền
        RefreshReferences();

        // Ẩn panel lúc đầu
        if (panel != null)
            panel.SetActive(false);
    }

    // 🔥 Tự động tìm lại panel & UI khi restart scene
    public void RefreshReferences()
    {
        // Tìm Panel (kể cả khi panel bị inactive)
        if (panel == null)
        {
            var objs = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (var o in objs)
            {
                if (o.name == "YouDiedPanel")
                {
                    panel = o;
                    break;
                }
            }
        }

        if (panel == null)
        {
            Debug.LogError("❌ YouDiedEffect: PANEL is NULL!!!");
            return;
        }

        // Tìm BG
        if (bg == null)
        {
            Transform bgObj = panel.transform.Find("BG");
            if (bgObj != null)
                bg = bgObj.GetComponent<Image>();
        }

        // Tìm Text
        if (diedText == null)
        {
            Transform txt = panel.transform.Find("YouDiedText");
            if (txt != null)
                diedText = txt.GetComponent<TMP_Text>();
        }

        // Tắt panel
        panel.SetActive(false);
    }

    // 🔥 Hàm được PlayerHealth gọi khi animation chết xong
    public void PlayEffect()
    {
        RefreshReferences(); // đảm bảo UI luôn đúng

        if (panel == null)
        {
            Debug.LogError("❌ YouDiedEffect: PANEL vẫn NULL khi PlayEffect!");
            return;
        }

        panel.SetActive(true);

        if (deadSound != null)
            deadSound.Play();

        StartCoroutine(EffectRoutine());
    }

    private IEnumerator EffectRoutine()
    {
        Time.timeScale = 0.1f; // slow motion
        yield return new WaitForSecondsRealtime(0.25f);
        Time.timeScale = 0f;

        // Reset alpha
        SetAlpha(bg, 0f);
        SetAlpha(diedText, 0f);

        // Fade BG
        float t = 0f;
        while (t < fadeBgTime)
        {
            t += Time.unscaledDeltaTime;
            float a = Mathf.Lerp(0f, 0.85f, t / fadeBgTime);
            SetAlpha(bg, a);
            yield return null;
        }

        // Fade text
        t = 0f;
        while (t < fadeTextTime)
        {
            t += Time.unscaledDeltaTime;
            float a = Mathf.Lerp(0f, 1f, t / fadeTextTime);
            SetAlpha(diedText, a);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(delayBeforeGameOver);

        // Mở GameOverMenu
        GameOverManager.Instance.ShowGameOver();
    }

    private void SetAlpha(Graphic g, float a)
    {
        Color c = g.color;
        c.a = a;
        g.color = c;
    }
    public void ForceHidePanel()
{
    RefreshReferences();

    if (panel != null)
        panel.SetActive(false);

    if (bg != null)
    {
        Color c = bg.color;
        bg.color = new Color(c.r, c.g, c.b, 0f);
    }

    if (diedText != null)
    {
        Color c = diedText.color;
        diedText.color = new Color(c.r, c.g, c.b, 0f);
    }
}

}

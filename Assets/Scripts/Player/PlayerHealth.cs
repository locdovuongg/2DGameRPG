using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float damageRecoveryTime = 1f;

    [Header("UI")]
    [SerializeField] private Slider healthSlider; 
    const string HEALTH_SLIDER_TEXT = "HealthSlider";

    private int currentHealth;
    private bool canTakeDamage = true;
    private Animator myAnimator;
    private Rigidbody2D rb;
    private PlayerAttack playerAttack;
    private bool isDead = false;
     public static PlayerHealth Instance { get; private set; }
     private bool deathEffectPlayed = false;

    private void Awake()
    {
        Instance = this;
        myAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    private void Start()
    {
        if (healthSlider == null)
        healthSlider = GameObject.Find("HealthSlider")?.GetComponent<Slider>();
        currentHealth = maxHealth;
        UpdateHealthSlider();
      StartCoroutine(DelayedUIUpdate());
        // Reset Rigidbody nếu bị FreezeAll từ scene trước
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    private IEnumerator DelayedUIUpdate()
{
    yield return new WaitForSeconds(0.05f); // chờ UI load
    UpdateHealthSlider();
}

    // ---- Healing ----
    public void HealPlayer()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += 20;
            UpdateHealthSlider();
        }
    }

    // ---- Damage ----
    public void TakeDamage(int damage, Vector2 knockback)
    {
        if (isDead) return;
        if (!canTakeDamage) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);
        canTakeDamage = false;

        myAnimator.SetTrigger("TakeDamage");
        playerAttack?.DisableActions();

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(knockback, ForceMode2D.Impulse);

        UpdateHealthSlider();
        CheckIfPlayerDead();

        StartCoroutine(DamageRecoveryRoutine());
    }

    private void CheckIfPlayerDead()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }
public void ForceUpdateHealthUI()
{
    UpdateHealthSlider();
}
    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
        playerAttack?.EnableActions();
    }

    // ---- Death ----
    private void Die()
    {
        if (isDead) return;
        isDead = true;

        canTakeDamage = false;
        playerAttack?.DisableActions();

        var controller = GetComponent<PlayerController>();
        if (controller != null) controller.enabled = false;

        rb.linearVelocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        foreach (var c in GetComponentsInChildren<Collider2D>())
            c.enabled = false;

        myAnimator.SetTrigger("Die");
    }

    // Animation Event gọi hàm này
    public void OnDeathAnimationFinished()
    {
      if (deathEffectPlayed) return;  
    deathEffectPlayed = true; 

    YouDiedEffect.Instance.PlayEffect();
    }

    // ---- Update UI ----
    private void UpdateHealthSlider()
    {
        if (healthSlider == null)
        {
            GameObject sliderObj = GameObject.Find(HEALTH_SLIDER_TEXT);

            if (sliderObj != null)
                healthSlider = sliderObj.GetComponent<Slider>();
            else
            {
                Debug.LogError("Không tìm thấy Slider tên 'Health Slider' trong scene!");
                return;
            }
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    // ---- Reset ----
    public void ResetHealthToMax()
    {
        currentHealth = maxHealth;
        canTakeDamage = true;
        isDead = false;

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        foreach (var c in GetComponentsInChildren<Collider2D>())
            c.enabled = true;

        var controller = GetComponent<PlayerController>();
        if (controller != null) controller.enabled = true;

        UpdateHealthSlider();
    }
}

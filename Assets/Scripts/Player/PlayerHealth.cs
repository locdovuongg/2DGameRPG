using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerHealth : Singleton<PlayerHealth>
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float damageRecoveryTime = 1f;

    [Header("UI")]
    [SerializeField] private Slider healthSlider; 
    const string HEALTH_SLIDER_TEXT = "Health Slider";
    private int currentHealth;
    private bool canTakeDamage = true;
    private Animator myAnimator;
    private Rigidbody2D rb;
    private PlayerAttack playerAttack;
    private bool isDead = false;

    protected override void Awake()
    {
        base.Awake();

        myAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerAttack = GetComponent<PlayerAttack>();

        Debug.Log("[PlayerHealth] Awake: Player created.");
    }

    private void Start()
    {
        currentHealth = maxHealth;
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
        Debug.Log("[PlayerHealth] TakeDamage() called");

        if (!canTakeDamage) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);
        canTakeDamage = false;

        Debug.Log($"[PlayerHealth] HP: {currentHealth}");

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

    if (rb != null)
    {
        rb.linearVelocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    foreach (var c in GetComponentsInChildren<Collider2D>())
        c.enabled = false;

    // chạy animation chết
    myAnimator.SetTrigger("Die");
}

public void OnDeathAnimationFinished()
{
    YouDiedEffect.Instance.PlayEffect();  
}


    // ---- Update UI ----
    private void UpdateHealthSlider()
    {
        if (healthSlider == null)
        {
         healthSlider = GameObject.Find(HEALTH_SLIDER_TEXT).GetComponent<Slider>();
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

        UpdateHealthSlider();
        Debug.Log($"[PlayerHealth] Reset HP = {currentHealth}");
    }
    private IEnumerator WaitForDeathAnimation()
{
    yield return null;
    AnimatorStateInfo info = myAnimator.GetCurrentAnimatorStateInfo(0);  
    yield return new WaitForSeconds(info.length);
    YouDiedEffect.Instance.PlayEffect();
}
    
}

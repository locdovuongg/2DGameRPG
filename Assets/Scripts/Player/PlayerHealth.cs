using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float damageRecoveryTime = 1f;

    private int currentHealth;
    private bool canTakeDamage = true;
    private Animator myAnimator;
    private Rigidbody2D rb;
    private PlayerAttack playerAttack;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerAttack = GetComponent<PlayerAttack>();
         Debug.Log("[PlayerHealth] Awake: Player được tạo");
    }

    private void Start()
    {
        ResetHealthToMax();
    }

    public void ResetHealthToMax()
    {
        currentHealth = maxHealth;
        canTakeDamage = true;
        Debug.Log($"[PlayerHealth] Reset HP = {currentHealth}");
    }

    public void TakeDamage(int damage, Vector2 knockback)
    {
            Debug.Log("[PlayerHealth] TakeDamage() được gọi");

        if (!canTakeDamage) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth); // tránh âm máu
        canTakeDamage = false;

        Debug.Log($"[PlayerHealth] HP: {currentHealth}");

        myAnimator.SetTrigger("TakeDamage");
        playerAttack?.DisableActions();

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(knockback, ForceMode2D.Impulse);

        StartCoroutine(DamageRecoveryRoutine());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
        playerAttack?.EnableActions();
    }

    private void Die()
    {
        Debug.Log("Player died!");
        myAnimator.SetTrigger("Die");
        playerAttack?.DisableActions();
        // this.enabled = false; // hoặc bạn có thể respawn sau
    }
}
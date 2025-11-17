using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float interruptRecover = 0.6f; // thời gian “choáng” sau khi bị đánh (tuỳ chọn)

    private int currentHealth;
    private Knockback knockback;
    private Flash flash;
    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D col;
    private EnemyDamage enemyDamage;     // ⬅️ thêm tham chiếu

    private bool isDead = false;

    private void Awake()
    {
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        enemyDamage = GetComponent<EnemyDamage>(); // ⬅️ lấy component để hủy đòn
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        // 🔥 HỦY ĐÒN / CHOÁNG enemy ngay khi bị trúng
        if (enemyDamage != null)
        {
            enemyDamage.CancelAttack();                 // hủy animation/đòn đang ra
            // hoặc: enemyDamage.Stun(interruptRecover); // nếu bạn dùng cơ chế stun
        }

        // Knockback + flash (nếu có component)
        if (knockback != null) knockback.GetKnockedBack(PlayerController.Instance.transform, 2f);
        if (flash != null) StartCoroutine(flash.FlashRoutine());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        // Ngừng knockback & disable movement
        if (knockback != null) knockback.enabled = false;
        if (rb != null) rb.linearVelocity = Vector2.zero; 
        // Tắt collider để không nhận thêm hit
        if (col != null) col.enabled = false;

        // Gọi animation chết
        if (anim != null) anim.SetTrigger("Die");

        // Destroy sau khi animation chạy xong
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        // chờ 1 frame để Animator thực sự vào state "Die"
        yield return null;

        float wait = 0.5f; // fallback
        if (anim != null)
        {
            var info = anim.GetCurrentAnimatorStateInfo(0);
            wait = info.length > 0 ? info.length : wait;
        }
        yield return new WaitForSeconds(wait);

        Destroy(gameObject);
    }

    public void DetectDeath()
    {
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }
}

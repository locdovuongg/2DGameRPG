using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private float knockbackForce = 6f;
    [SerializeField] private float attackCooldown = 1.5f;

    [Header("Attack Detection")]
    [SerializeField] private Transform attackPoint;     // vị trí đánh
    [SerializeField] private float attackRange = 1f;    // bán kính vùng đánh
    [SerializeField] private LayerMask playerLayer;     // layer của player
    private bool isAttacking = false; 
    private Animator anim;
    private bool canAttack = true;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void TriggerAttack()
    {

    isAttacking = true;         
    canAttack = false;
    anim.SetTrigger("Attack"); 
    Invoke(nameof(ResetAttack), attackCooldown);
}
private void ResetAttack()
{
    canAttack = true;
    isAttacking = false;

}
    public void CancelAttack()
{
    if (!isAttacking) return;

    Debug.Log("[EnemyDamage] ❌ Attack bị hủy do bị chém!");
    isAttacking = false;
    anim.ResetTrigger("Attack");
    anim.Play("Idle"); // hoặc "Hurt" nếu có animation bị trúng đòn
}
   public void DealDamage()
{
    Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
    if (hit != null)
    {
        PlayerHealth player = hit.GetComponent<PlayerHealth>();
        if (player != null)
        {
            Vector2 knockDir = (player.transform.position - transform.position).normalized;
            player.TakeDamage(damageAmount, knockDir * knockbackForce);
            Debug.Log("[EnemyDamage] Player trúng đòn");
        }
        else
        {
            Debug.Log("[EnemyDamage] Không tìm thấy PlayerHealth");
        }
    }
    else
    {
        Debug.Log("[EnemyDamage] Không có gì trong vùng đánh");
    }
}


    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
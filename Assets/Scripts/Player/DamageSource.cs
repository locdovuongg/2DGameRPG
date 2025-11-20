using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [SerializeField] private int damageAmount = 20;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Chỉ xử lý nếu chính collider này là vũ khí
        if (!CompareTag("Weapon")) return;

        // 1) Enemy: gây damage + hủy đòn enemy nếu đang tấn công
        if (other.TryGetComponent(out EnemyHealth enemyHealth))
        {
            enemyHealth.TakeDamage(damageAmount);

            if (other.TryGetComponent(out EnemyDamage enemyDamage))
            {
                enemyDamage.CancelAttack(); // hủy animation/đòn đang ra
            }

            return;
        }

        // 2) Đồ phá được (bụi cỏ, hoa...)
        if (other.CompareTag("CanDestroy"))
        {
            Destroy(other.gameObject);
            Debug.Log($"{name} chém trúng bụi cỏ: {other.name}");
            return;
        }
    }
}

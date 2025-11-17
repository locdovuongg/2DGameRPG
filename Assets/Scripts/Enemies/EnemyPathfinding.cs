using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;

    private Rigidbody2D rb;
    private Vector2 moveDir;
    private Knockback knockback;

    private void Awake()
    {
        knockback = GetComponent<Knockback>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Nếu bị knockback -> không di chuyển
        if (knockback.gettingKnockedBack)
        {
            return;
        }

        // Di chuyển bình thường bằng velocity
        rb.linearVelocity = moveDir * moveSpeed;
    }

    public void MoveTo(Vector2 direction)
    {
        moveDir = direction.normalized;
    }
}

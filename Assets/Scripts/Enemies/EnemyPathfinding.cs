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

    rb.linearVelocity = moveDir * moveSpeed;

    // ------ Flip trái/phải tự động theo hướng di chuyển ------
    if (rb.linearVelocity.x < -0.1f)
        transform.localScale = new Vector3(-1, 1, 1);
    else if (rb.linearVelocity.x > 0.1f)
        transform.localScale = new Vector3(1, 1, 1);
}

    public void MoveTo(Vector2 direction)
    {
        moveDir = direction.normalized;
    }
}

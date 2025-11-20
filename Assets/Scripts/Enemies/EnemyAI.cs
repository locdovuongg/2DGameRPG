using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        Roaming,
        Chasing,
        Attacking
    }

    [Header("Settings")]
    [SerializeField] private float detectRange = 5f;
    [SerializeField] private float attackRange = 1.8f;
    [SerializeField] private float roamDelay = 2f;
    [SerializeField] private float attackCooldown = 1f;

    private State state;
    private EnemyPathfinding enemyPathfinding;
    private Transform player;
    private Animator animator;
    private bool canAttack = true;

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        animator = GetComponent<Animator>();

        // kiểm tra animator
        if (!animator)
            Debug.LogError("Enemy không có Animator!");

        // tự tìm player
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
        else
            Debug.LogWarning("Không tìm thấy Player! Gán Tag = Player cho nhân vật.");
    }

    private void Start()
    {
        state = State.Roaming;
        StartCoroutine(RoamingRoutine());
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // Attack ưu tiên cao nhất
        if (distance <= attackRange)
        {
            state = State.Attacking;
            enemyPathfinding.MoveTo(Vector2.zero); // đứng yên
            TryAttack();
            return;
        }

        // Chase
        if (distance <= detectRange)
        {
            state = State.Chasing;
            Vector2 dir;

    float dx = player.position.x - transform.position.x;
    float dy = player.position.y - transform.position.y;

    if (Mathf.Abs(dx) > Mathf.Abs(dy))
    {
    // Ưu tiên di chuyển ngang
        dir = new Vector2(Mathf.Sign(dx), 0);
    }
    else
    {
    // Ưu tiên di chuyển dọc
        dir = new Vector2(0, Mathf.Sign(dy));
    }

    enemyPathfinding.MoveTo(dir);
        }
        else
        {
            state = State.Roaming;
        }
    }

    // ---------------- Attack ----------------
    private void TryAttack()
    {
        if (!canAttack) return;

        canAttack = false;

        if (animator != null)
            animator.SetTrigger("Attack");
        else
            Debug.LogError("Animator không tồn tại trong EnemyAI!");

        StartCoroutine(AttackCooldownRoutine());
    }

    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    // ---------------- Roaming ----------------
    private IEnumerator RoamingRoutine()
    {
        while (true)
        {
            if (state == State.Roaming)
            {
                Vector2 roamDir = new Vector2(
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, 1f)
                ).normalized;

                enemyPathfinding.MoveTo(roamDir);
            }

            yield return new WaitForSeconds(roamDelay);
        }
    }
}

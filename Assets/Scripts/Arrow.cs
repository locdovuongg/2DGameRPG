using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private int damageAmount = 15;

    private Vector3 direction;

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Enemy
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damageAmount);
            Destroy(gameObject); // mũi tên biến mất sau khi trúng
            return;
        }

        // Tường hoặc vật cản (chạm gì không phải enemy)
        if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}

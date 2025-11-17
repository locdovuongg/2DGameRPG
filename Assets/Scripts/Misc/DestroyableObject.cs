using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    [Header("Prefab rơi ra khi bị chém")]
    [SerializeField] private GameObject dropPrefab;   // có thể là lá, cánh hoa, mảnh vỡ...
    [SerializeField] private int dropCount = 3;
    [SerializeField] private float spreadRadius = 0.3f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Chỉ bị phá nếu vũ khí chém vào
        if (other.CompareTag("Weapon"))
        {
            SpawnDrops();
            Destroy(gameObject);
        }
    }

    private void SpawnDrops()
    {
        if (dropPrefab == null) return;

        for (int i = 0; i < dropCount; i++)
        {
            Vector2 offset = Random.insideUnitCircle * spreadRadius;
            Vector3 spawnPos = transform.position + new Vector3(offset.x, offset.y, 0f);
            Quaternion rot = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

            GameObject drop = Instantiate(dropPrefab, spawnPos, rot);
            Destroy(drop, 10f); // tự mất sau 10s (tuỳ bạn)
        }
    }
}

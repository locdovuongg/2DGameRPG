using System.Collections;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private enum PickUpType 
    {
        HealthGlobe,
        GoldCoin,
        StaminaGlobe
    }

    [SerializeField] private PickUpType pickUpType;
    [SerializeField] private float pickUpDistance = .5f;
    [SerializeField] private float accelartionRate = .2f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float popDuration = 1f;
    [SerializeField] private float heightY = 1.5f;

    private Vector3 moveDir;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(AnimCurveSpawnRoutine());
    }

    private void Update()
    {
        Vector3 playerPos = PlayerController.Instance.transform.position;

        if (Vector3.Distance(transform.position, playerPos) < pickUpDistance)
        {
            moveDir = (playerPos - transform.position).normalized;
            moveSpeed += accelartionRate;
        }
        else
        {
            moveDir = Vector3.zero;
            moveSpeed = 0;
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveDir * moveSpeed;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            DetectPickupType();
            Destroy(gameObject);
        }
    }

    private IEnumerator AnimCurveSpawnRoutine()
    {
        Vector2 start = transform.position;
        float rx = transform.position.x + Random.Range(-2f, 2f);
        float ry = transform.position.y + Random.Range(-1f, 1f);
        Vector2 end = new Vector2(rx, ry);

        float t = 0f;
        while (t < popDuration)
        {
            float linearT = t / popDuration;
            float heightT = animCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0, heightY, heightT);

            transform.position = Vector2.Lerp(start, end, linearT) + new Vector2(0, height);

            t += Time.deltaTime;
            yield return null;
        }
    }

    private void DetectPickupType()
    {
        switch (pickUpType)
        {
            case PickUpType.HealthGlobe:
                PlayerHealth.Instance.HealPlayer();
                break;

            case PickUpType.GoldCoin:
            EconomyManager.Instance.UpdateCurrentGold();
                Debug.Log("Gold Picked Up");
                break;

            case PickUpType.StaminaGlobe:
                Debug.Log("Stamina Picked Up");
                break;
        }
    }
}

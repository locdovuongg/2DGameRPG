using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private TrailRenderer myTraiRenderer;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;
    private bool isDashing = false;
    public static PlayerController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        playerControls.Combat.Dash.performed += _ => Dash();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
         if (playerControls != null)
        playerControls.Disable();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        AdjustPlayerFacingDirection();
        Move();
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void Move()
    {
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        mySpriteRender.flipX = mousePos.x < playerScreenPoint.x;
    }

    private void Dash()
    {
        if (isDashing) return;

        isDashing = true;
        moveSpeed *= dashSpeed;

        if (myTraiRenderer != null)
            myTraiRenderer.emitting = true;

        StartCoroutine(EndDashRoutine());
    }

    private IEnumerator EndDashRoutine()
    {
        float dashTime = 0.3f;
        float dashCD = 5f;

        yield return new WaitForSeconds(dashTime);

        moveSpeed /= dashSpeed;

        if (myTraiRenderer != null)
            myTraiRenderer.emitting = false;

        yield return new WaitForSeconds(dashCD);

        isDashing = false;
    }
}

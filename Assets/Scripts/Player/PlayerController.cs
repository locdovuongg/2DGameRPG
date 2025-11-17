using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : Singleton<PlayerController>
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
[SerializeField] private float moveSpeed = 1f;
[SerializeField] private float dashSpeed = 4f;
[SerializeField] private TrailRenderer myTraiRenderer;
private PlayerControls playerControls;
private Vector2 movement;
private Rigidbody2D rb;
private Animator myAnimator;
private SpriteRenderer mySpriteRender;

private bool isDashing = false;
    protected override void Awake()
    {
        base.Awake();
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
       if (mousePos.x < playerScreenPoint.x)
        {
            mySpriteRender.flipX = true;
          
        }
        else
        {
            mySpriteRender.flipX = false;
            
        }
    }
    private void Dash()
    {
        if (!isDashing)
        {
           isDashing = true;
           moveSpeed *= dashSpeed;
           myTraiRenderer.emitting = true;
           StartCoroutine(EndDashRoutine());
        }
    }
    private IEnumerator EndDashRoutine()
    {
       float dashTime = 0.3f;
       float dashCD = 5f;
       yield return new WaitForSeconds(dashTime);
       moveSpeed/= dashSpeed;
       myTraiRenderer.emitting = false;
       yield return new WaitForSeconds(dashCD);
       isDashing = false;
    }
  
}
